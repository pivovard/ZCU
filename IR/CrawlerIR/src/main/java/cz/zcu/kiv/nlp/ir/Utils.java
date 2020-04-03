package cz.zcu.kiv.nlp.ir;

import java.io.*;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Map;

/**
 * Utils for I/O operations
 * Created by Tigi on 22.9.2014.
 */
public class Utils {
    public static final java.text.DateFormat SDF = new SimpleDateFormat("yyyy-MM-dd_HH_mm_SS");

    /**
     * Saves text to given file.
     *
     * @param file file to save
     * @param text text to save
     */
    public static void saveFile(File file, String text) {
        try {
            PrintStream printStream = new PrintStream(new BufferedOutputStream(new FileOutputStream(file)));
            printStream.print(text);
            printStream.close();
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
    }

    /**
     * Saves lines from the list into given file; each entry is saved as a new line.
     *
     * @param file    file to save
     * @param results text to save
     * @param idMap   lines
     */
    public static void saveFile(File file, Map<String, List<String>> results, Map<String, String> idMap) {
        try {
            PrintStream printStream = new PrintStream(new BufferedOutputStream(new FileOutputStream(file)));
            for (String key : idMap.keySet()) {
                List<String> resultList = results.get(key);
                String line = idMap.get(key);
                for (String result : resultList) {
                    printStream.println(line + "\t" + result);
                }
            }
            printStream.close();
        } catch (FileNotFoundException e) {

            e.printStackTrace();
        }
    }


    /**
     * Read lines from the stream; lines are trimmed and empty lines are ignored.
     *
     * @param inputStream stream
     * @return list of lines
     */
    public static List<String> readLines(InputStream inputStream) {
        if (inputStream == null) {
            throw new IllegalArgumentException("Cannot locate stream");
        }
        try {
            List<String> result = new ArrayList<String>();

            BufferedReader br = new BufferedReader(new InputStreamReader(inputStream));
            String line;

            while ((line = br.readLine()) != null) {
                if (!line.trim().isEmpty()) {
                    result.add(line.trim());
                }
            }
            inputStream.close();
            return result;
        } catch (IOException e) {
            throw new IllegalStateException(e);
        }
    }

    /**
     * Saves lines from the list into given file; each entry is saved as a new line.
     *
     * @param file file to save
     * @param list lines of text to save
     */
    public static void saveFile(File file, Collection<String> list) {
        try {
            PrintStream printStream = new PrintStream(new BufferedOutputStream(new FileOutputStream(file)));
            for (String text : list) {
                printStream.println(text);
            }
            printStream.close();
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
    }


    /**
     * Read lines from the stream; lines are trimmed and empty lines are ignored.
     *
     * @param inputStream stream
     * @return list of lines
     */
    public static List<String> readTXTFile(InputStream inputStream) {
        if (inputStream == null) {
            throw new IllegalArgumentException("Cannot locate stream");
        }
        try {
            List<String> result = new ArrayList<String>();

            BufferedReader br = new BufferedReader(new InputStreamReader(inputStream));
            String line;

            while ((line = br.readLine()) != null) {
                if (!line.trim().isEmpty()) {
                    result.add(line.trim());
                }
            }
            inputStream.close();

            return result;
        } catch (IOException e) {
            throw new IllegalStateException(e);
        }
    }
}
