package cz.zcu.kiv.nlp.ir.trec;

import cz.zcu.kiv.nlp.ir.trec.data.Document;
import cz.zcu.kiv.nlp.ir.trec.data.Result;
import org.apache.log4j.*;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

/**
 * Created by pivov on 05-Jun-19.
 */
public class IndexerMain {

    static Logger log = Logger.getLogger(TestTrecEval.class);
    static final String OUTPUT_DIR = "./TREC";

    static Index index;

    public static void main(String args[]) throws IOException {
        configureLogger();

        List<Document> documents = new ArrayList<Document>();
        if(args.length > 0){
            documents = loadDocuments(args[0]);
        }
        else{
            documents = loadDocuments(OUTPUT_DIR + "/czechData.bin");
        }

        index = new Index();
        index.index(documents);

        action();
    }

    private static void action(){
        Scanner scanner = new Scanner(System.in);

        while(true){
            System.out.println("\nQuery or command:");

            String input = scanner.nextLine();

            char command = input.charAt(0);

            switch (command){
                case 'q':
                    if(input.length() < 3) break;
                    search(input.substring(2));
                    break;
                case 'a':
                    index.index(loadDocuments(input.substring(2)));
                    break;
                case 'u':
                    index.update(loadDocuments(input.substring(2)));
                    break;
                case 'r':
                    index.delete(input.substring(2));
                    break;
                case 'e':
                    return;
                case 'h':
                default:
                    help();
                    break;
            }

        }
    }

    private static List<Document> loadDocuments(String path){
        List<Document> documents = new ArrayList<Document>();
        File serializedData = new File(path);

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

        return  documents;
    }

    private static void search(String query){
        List<Result> results = index.search(query);

        System.out.println("\nResults: " + results.size());
        for(int i = 0; i < (results.size() > 50 ? 50 : results.size()); i++){
            System.out.println(results.get(i).toString());
        }
    }

    private static void help(){
        System.out.println("q\t query\n"
                            + "a\t path\t add document\n"
                            + "u\t path\t update document\n"
                            + "r\t docId\t remove document\n"
                            + "h\t\t help\n"
                            + "e\t\t exit");
    }

    private static void configureLogger() {
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

}
