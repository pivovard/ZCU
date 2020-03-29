package cz.zcu.kiv.nlp.ir.trec;

import cz.zcu.kiv.nlp.ir.trec.data.*;
import org.apache.log4j.*;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.xpath.*;
import java.io.*;
import java.text.ParseException;
import java.util.*;


/**
 * @author tigi
 */

public class TestTrecEval {

    static Logger log = Logger.getLogger(TestTrecEval.class);
    static final String OUTPUT_DIR = "./TREC";

    protected static void configureLogger() {
        BasicConfigurator.resetConfiguration();
        BasicConfigurator.configure();

        File results = new File(OUTPUT_DIR);
        if (!results.exists()) {
            results.mkdir();
        }

        try {
            Appender appender = new WriterAppender(new PatternLayout(), new FileOutputStream(new File(OUTPUT_DIR + "/" + SerializedDataHelper.SDF.format(System.currentTimeMillis()) + " - " + ".log"), false));
            BasicConfigurator.configure(appender);
        } catch (IOException e) {
            e.printStackTrace();
        }

        Logger.getRootLogger().setLevel(Level.INFO);
    }

    public static void main(String args[]) throws IOException {
        configureLogger();

//        todo constructor
        Index index = new Index();

        List<Topic> topics = SerializedDataHelper.loadTopic(new File(OUTPUT_DIR + "/topicData.bin"));

        File serializedData = new File(OUTPUT_DIR + "/czechData.bin");

        List<Document> documents = new ArrayList<Document>();
        log.info("load");
        try {
            if (serializedData.exists()) {
                documents = SerializedDataHelper.loadDocument(serializedData);
            } else {
                log.error("Cannot find " + serializedData);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        log.info("Documents: " + documents.size());

        index.index(documents);

        List<String> lines = new ArrayList<String>();

        for (Topic t : topics) {
            List<Result> resultHits = index.search(t.getTitle() + " " + t.getDescription());

            Comparator<Result> cmp = new Comparator<Result>() {
                public int compare(Result o1, Result o2) {
                    if (o1.getScore() > o2.getScore()) return -1;
                    if (o1.getScore() == o2.getScore()) return 0;
                    return 1;
                }
            };

            Collections.sort(resultHits, cmp);
            for (Result r : resultHits) {
                final String line = r.toString(t.getId());
                lines.add(line);
            }
            if (resultHits.size() == 0) {
                lines.add(t.getId() + " Q0 " + "abc" + " " + "99" + " " + 0.0 + " runindex1");
            }
        }
        final File outputFile = new File(OUTPUT_DIR + "/results_" + SerializedDataHelper.SDF.format(System.currentTimeMillis()) + ".txt");
        IOUtils.saveFile(outputFile, lines);
        //try to run evaluation
        try {
            runTrecEval(outputFile.toString());
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private static String runTrecEval(String predictedFile) throws IOException {

        String commandLine = "./trec_eval.8.1/./trec_eval" +
                " ./trec_eval.8.1/czech" +
                " " + predictedFile;

        System.out.println(commandLine);
        Process process = Runtime.getRuntime().exec(commandLine);

        BufferedReader stdout = new BufferedReader(new InputStreamReader(process.getInputStream()));
        BufferedReader stderr = new BufferedReader(new InputStreamReader(process.getErrorStream()));

        String trecEvalOutput;
        StringBuilder output = new StringBuilder("TREC EVAL output:\n");
        for (String line; (line = stdout.readLine()) != null; ) output.append(line).append("\n");
        trecEvalOutput = output.toString();
        System.out.println(trecEvalOutput);

        int exitStatus = 0;
        try {
            exitStatus = process.waitFor();
        } catch (InterruptedException ie) {
            ie.printStackTrace();
        }
        System.out.println(exitStatus);

        stdout.close();
        stderr.close();

        return trecEvalOutput;
    }
}
