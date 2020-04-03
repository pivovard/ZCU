package cz.zcu.kiv.nlp.tigi.clustering;

import org.apache.log4j.BasicConfigurator;
import org.apache.log4j.Level;
import org.apache.log4j.Logger;

import java.io.File;
import java.io.IOException;


/**
 * @author Tigi
 */

public class RunClustering {

    static Logger log = Logger.getLogger(RunClustering.class);
    static final String OUTPUT_DIR = "output";

    /**
     * Logger configuration.
     */
    protected static void configureLogger() {
        BasicConfigurator.resetConfiguration();
        BasicConfigurator.configure();

        File results = new File(OUTPUT_DIR);
        if (!results.exists()) {
            results.mkdir();
        }

        Logger.getRootLogger().setLevel(Level.INFO);
    }

    public static void main(String args[]) throws IOException {
        configureLogger();
        String file = "data.txt";
        final String sspace = OUTPUT_DIR + "/model_cs.sspace";
        final String matrix = sspace + ".forcluto.mat";
        try {
            File matrixFile = new File(matrix);
            if (matrixFile.exists()) {
                matrixFile.delete();
            }
            edu.ucla.sspace.mains.CoalsMain.main(new String[]{"--docFile", file, "-n", "14000", "-t", "8", sspace});
            for (int NUM_CLUSTERS : new int[]{100, 500, 1000}) {

                File sspaceFile = new File(sspace);
                log.info(sspaceFile.exists());
                new WordClustering().cluster(sspaceFile, NUM_CLUSTERS);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }


}
