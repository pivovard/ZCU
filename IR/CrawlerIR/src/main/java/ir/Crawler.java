package ir;

import cz.zcu.kiv.nlp.ir.AbstractHTMLDownloader;
import cz.zcu.kiv.nlp.ir.HTMLDownloader;
import cz.zcu.kiv.nlp.ir.HTMLDownloaderSelenium;
import cz.zcu.kiv.nlp.ir.Utils;

import org.apache.commons.io.FileUtils;
import org.apache.log4j.BasicConfigurator;
import org.apache.log4j.Level;
import org.apache.log4j.Logger;

import java.io.*;
import java.net.URL;
import java.util.*;

/**
 * CrawlerVSCOM class acts as a controller. You should only adapt this file to serve your needs.
 * Created by Tigi on 31.10.2014.
 */
public class Crawler {
    /**
     * Xpath expressions to extract and their descriptions.
     */
    private final static Map<String, String> xpathMap = new HashMap<String, String>();

    static {
        xpathMap.put("allText", "//div[@class='row estate-detail-grid text-xs-center flexbox']/allText()");
        xpathMap.put("html", "//div[@class='row estate-detail-grid text-xs-center flexbox']/html()");
        xpathMap.put("tidyText", "//div[@class='row estate-detail-grid text-xs-center flexbox']/tidyText()");
    }

    private static final String STORAGE = "./storage/";
    private static final String OSTORAGE = "./storage/offers/";
    
    
    private static String SITE = "http://www.pubec.cz";
    private static String SITE_SUFFIX = "/prodej/";


    /**
     * Be polite and don't send requests too often.
     * Waiting period between requests. (in milisec)
     */
    private static final int POLITENESS_INTERVAL = 500;
    private static final Logger log = Logger.getLogger(Crawler.class);

    /**
     * Main method
     */
    public static void main(String[] args) {
        //Initialization
        BasicConfigurator.configure();
        Logger.getRootLogger().setLevel(Level.INFO);
        
        File outputDir = new File(STORAGE);
        if (!outputDir.exists()) {
            boolean mkdirs = outputDir.mkdirs();
            if (mkdirs) {
                log.info("Output directory created: " + outputDir);
            } else {
                log.error("Output directory can't be created! Please either create it or change the STORAGE parameter.\nOutput directory: " + outputDir);
            }
        }
//        HTMLDownloader downloader = new HTMLDownloader();
        AbstractHTMLDownloader downloader = new HTMLDownloaderSelenium();
        Map<String, Map<String, List<String>>> results = new HashMap<String, Map<String, List<String>>>();

        for (String key : xpathMap.keySet()) {
            Map<String, List<String>> map = new HashMap<String, List<String>>();
            results.put(key, map);
        }

//        Collection<String> urlsSet = new ArrayList<String>();
        Collection<String> urlsSet = new HashSet<>();
        Collection<String> offersSet = new HashSet<>();
        Map<String, PrintStream> printStreamMap = new HashMap<String, PrintStream>();

        //Try to load links
        File links = new File(STORAGE + "_urls.txt");
        if (links.exists()) {
            try {
                List<String> lines = Utils.readTXTFile(new FileInputStream(links));
                for (String line : lines) {
                    urlsSet.add(line);
                }
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            }
        } else {

        	//download links
            int max = 11;
            max = 1;
            for (int i = 1; i <= max; i++) {
                String link = SITE + SITE_SUFFIX + i;
                urlsSet.addAll(downloader.getLinks(link, "//div[@class='col-xs-12 col-sm-6 col-lg-4 col-xl-3 p-x-0 estate-list-item estate-list-prodej']/a/@href"));
            }
            Utils.saveFile(new File(STORAGE + Utils.SDF.format(System.currentTimeMillis()) + "_links_size_" + urlsSet.size() + ".txt"),
                    urlsSet);
            
            try {
                Thread.sleep(POLITENESS_INTERVAL);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }

        //prepare files
        for (String key : results.keySet()) {
            File file = new File(STORAGE + "/" + Utils.SDF.format(System.currentTimeMillis()) + "_" + key + ".txt");
            PrintStream printStream = null;
            try {
                printStream = new PrintStream(new FileOutputStream(file));
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            }
            printStreamMap.put(key, printStream);
        }

        //download data
        int count = 0;
        for (String url : urlsSet) {
            String link = url;
            if (!link.contains(SITE)) {
                link = SITE + url;
            }
            //Download and extract data according to xpathMap
            Map<String, List<String>> products = downloader.processUrl(link, xpathMap);
            count++;
            if (count % 100 == 0) {
                log.info(count + " / " + urlsSet.size() + " = " + count / (0.0 + urlsSet.size()) + "% done.");
            }
            for (String key : results.keySet()) {
                Map<String, List<String>> map = results.get(key);
                List<String> list = products.get(key);
                if (list != null) {
                    map.put(url, list);
                    log.info(Arrays.toString(list.toArray()));
                    //print
                    PrintStream printStream = printStreamMap.get(key);
                    for (String result : list) {
                        printStream.println(url + "\t" + result);
                    }
                }
            }
            //Download links of offer pdf files
            offersSet.addAll(downloader.getLinks(link, "//div[@class='row estate-detail-grid text-xs-center flexbox']//a/@href"));
            
            
            try {
                Thread.sleep(POLITENESS_INTERVAL);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
        
        //download pdfs
        for (String url : offersSet) {
            String link = url;
            if (!link.contains(SITE)) {
                link = SITE + url;
            }
            
            log.info("Downloading file: " + link);
            String[] name = url.split("/");
            
            try {
            	FileUtils.copyURLToFile(new URL(link), new File(OSTORAGE + name[name.length-1] + ".pdf"));
            }
            catch (IOException e) {
            	e.printStackTrace();
            }
            
            try {
                Thread.sleep(POLITENESS_INTERVAL);
            } catch (InterruptedException e) {
                e.printStackTrace();
                log.error("Failed to download file: " + link);
            }
        }

        //close print streams
        for (String key : results.keySet()) {
            PrintStream printStream = printStreamMap.get(key);
            printStream.close();
        }

        // Save links that failed in some way.
        // Be sure to go through these and explain why the process failed on these links.
        // Try to eliminate all failed links - they consume your time while crawling data.
        reportProblems(downloader.getFailedLinks());
        downloader.emptyFailedLinks();
        log.info("-----------------------------");


//        // Print some information.
//        for (String key : results.keySet()) {
//            Map<String, List<String>> map = results.get(key);
//            Utils.saveFile(new File(STORAGE + "/" + Utils.SDF.format(System.currentTimeMillis()) + "_" + key + "_final.txt"),
//                    map, idMap);
//            log.info(key + ": " + map.size());
//        }
        System.exit(0);
    }


    /**
     * Save file with failed links for later examination.
     *
     * @param failedLinks links that couldn't be downloaded, extracted etc.
     */
    private static void reportProblems(Set<String> failedLinks) {
        if (!failedLinks.isEmpty()) {

            Utils.saveFile(new File(STORAGE + Utils.SDF.format(System.currentTimeMillis()) + "_undownloaded_links_size_" + failedLinks.size() + ".txt"),
                    failedLinks);
            log.info("Failed links: " + failedLinks.size());
        }
    }


}
