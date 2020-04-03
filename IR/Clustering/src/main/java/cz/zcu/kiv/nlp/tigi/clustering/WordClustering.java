package cz.zcu.kiv.nlp.tigi.clustering;

import edu.ucla.sspace.common.SemanticSpace;
import edu.ucla.sspace.common.SemanticSpaceIO;
import edu.ucla.sspace.matrix.Matrices;
import edu.ucla.sspace.matrix.Matrix;
import edu.ucla.sspace.matrix.MatrixIO;
import edu.ucla.sspace.vector.DoubleVector;
import edu.ucla.sspace.vector.Vector;
import org.apache.log4j.Logger;

import java.io.*;
import java.util.*;

/**
 * (c) Tigi
 */
public class WordClustering {

    static Logger log = Logger.getLogger(WordClustering.class);

    public void cluster(File semspaceFile, int numberOfClusters) throws Exception {
        SemanticSpace semanticSpace = SemanticSpaceIO.load(semspaceFile);
        List<String> vocabulary = new ArrayList<String>(semanticSpace.getWords());

        // need dense matrix for Cluto (failed on Beagle sparse-matrix:
        // "IDF column model only applies to sparse matrices and non-correlation-coefficient similarities!"
        log.debug(vocabulary.size() + " * " + semanticSpace.getVectorLength() +
                " = " + ((long) (vocabulary.size()) * semanticSpace.getVectorLength()));
//        Matrix matrix = new ArrayMatrix(vocabulary.size(), semanticSpace.getVectorLength());

        log.debug("vocabulary: " + vocabulary.size());
        final String matrixFilePath = semspaceFile.getAbsolutePath() + ".forcluto.mat";
        File clutoMatrix = new File(matrixFilePath);
        Matrix matrix;
        if (!clutoMatrix.exists()) {
            matrix = Matrices.create(vocabulary.size(), semanticSpace.getVectorLength(), false);
            // creating matrix from words
            Iterator<String> iterator = vocabulary.iterator();
            for (int i = 0; i < vocabulary.size(); i++) {
                String word = iterator.next();

                Vector vector = semanticSpace.getVector(word);

                matrix.setRow(i, (DoubleVector) vector);
            }
            MatrixIO.writeMatrix(matrix, clutoMatrix, MatrixIO.Format.CLUTO_SPARSE);
        } else {
            matrix = MatrixIO.readMatrix(clutoMatrix, MatrixIO.Format.CLUTO_SPARSE);
        }
        // clustering
        log.info("Clustering (number of clusters: " + numberOfClusters + ")");
        runTrecEval(semspaceFile.getAbsolutePath() + "_" + numberOfClusters + ".clutoResult", matrixFilePath, numberOfClusters);
//        ClutoClustering clustering = new ClutoClustering();
//        Assignments assignments = clustering.cluster(matrix, numberOfClusters, ClutoClustering.Method.REPEATED_BISECTIONS_REPEATED, ClutoClustering.Criterion.I2);

        log.info("done");

        Map<String, Integer> mappingWordsToClusters = new HashMap<String, Integer>();
        Map<Integer, Set<String>> mappingClustersToWords = new HashMap<Integer, Set<String>>();

        final String clutoResult = semspaceFile.getAbsolutePath() + "_" + numberOfClusters + ".clutoResult";
        File clusters = new File(clutoResult);
        if (clusters.exists()) {
            BufferedReader bufferedReader;
            int row = 0;
            try {
                bufferedReader = new BufferedReader(new InputStreamReader(new FileInputStream(clusters), "UTF-8"));

                String line;
                while ((line = bufferedReader.readLine()) != null) {
                    String trimedLine = line.trim();
                    if (trimedLine.equals("")) continue;
                    int cluster = Integer.parseInt(trimedLine);
                    String word = vocabulary.get(row);

                    mappingWordsToClusters.put(word, cluster);

                    if (!mappingClustersToWords.containsKey(cluster)) {
                        mappingClustersToWords.put(cluster, new HashSet<String>());
                    }
                    mappingClustersToWords.get(cluster).add(word);

                    row++;
                }

                log.info(mappingWordsToClusters.size() + " :words; clusters: " + mappingClustersToWords.size() + ", rows: " + row);

            } catch (Exception e) {
                e.printStackTrace();
            }


            log.info(mappingWordsToClusters);
            log.info(mappingClustersToWords);

            BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(new FileOutputStream(semspaceFile.getAbsolutePath() + ".clustered." + numberOfClusters + ".txt"), "UTF-8"));
            for (Set<String> list : mappingClustersToWords.values()) {
                for (String word : list) bw.write(word + " ");
                bw.write("\n");
                bw.flush();
            }
            bw.close();

            File outputFile = new File(semspaceFile.getAbsolutePath() + ".clustered." + numberOfClusters + ".bin");
            ObjectOutputStream outputStream = new ObjectOutputStream(new FileOutputStream(outputFile));
            outputStream.writeObject(mappingWordsToClusters);
            log.info("Saved to " + outputFile);
        } else {
            log.error(clutoResult + " not found!");
        }

    }

    private static String runTrecEval(String outputFilePath, String matrixFilePath, int numberOfClusters) throws IOException {

        String commandLine = "vcluster.exe -clmethod rbr -clustfile \"" + outputFilePath + "\" -crfun i2 -sim cos \"" + matrixFilePath + "\" " + numberOfClusters;

        log.info(commandLine);
        Process process = Runtime.getRuntime().exec(commandLine);

        BufferedReader stdout = new BufferedReader(new InputStreamReader(process.getInputStream()));
        BufferedReader stderr = new BufferedReader(new InputStreamReader(process.getErrorStream()));

        String trecEvalOutput = null;
        StringBuilder output = new StringBuilder("Console output:\n");
        for (String line = null; (line = stdout.readLine()) != null; ) output.append(line).append("\n");
        trecEvalOutput = output.toString();
        log.info(trecEvalOutput);

        int exitStatus = 0;
        try {
            exitStatus = process.waitFor();
        } catch (InterruptedException ie) {
            ie.printStackTrace();
        }
        log.info(exitStatus);

        stdout.close();
        stderr.close();

        return trecEvalOutput;
    }
}
