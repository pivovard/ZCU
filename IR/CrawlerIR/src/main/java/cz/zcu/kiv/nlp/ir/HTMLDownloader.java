package cz.zcu.kiv.nlp.ir;

import edu.uci.ics.crawler4j.crawler.CrawlConfig;
import edu.uci.ics.crawler4j.crawler.Page;
import edu.uci.ics.crawler4j.crawler.exceptions.PageBiggerThanMaxSizeException;
import edu.uci.ics.crawler4j.fetcher.PageFetchResult;
import edu.uci.ics.crawler4j.fetcher.PageFetcher;
import edu.uci.ics.crawler4j.parser.HtmlParseData;
import edu.uci.ics.crawler4j.parser.ParseData;
import edu.uci.ics.crawler4j.parser.Parser;
import edu.uci.ics.crawler4j.url.WebURL;
import org.apache.http.HttpStatus;
import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;
import us.codecraft.xsoup.Xsoup;

import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * This class is a demonstration of how crawler4j can be used to download a website
 * Created by Tigi on 31.10.2014.
 */
public class HTMLDownloader extends AbstractHTMLDownloader {

    private final Parser parser;
    private final PageFetcher pageFetcher;

    /**
     * Constructor
     */
    public HTMLDownloader() {
        super();
        CrawlConfig config = new CrawlConfig();
        parser = new Parser(config);
        pageFetcher = new PageFetcher(config);

        config.setMaxDepthOfCrawling(0);
        config.setResumableCrawling(false);
    }

    /**
     * Downloads given url
     *
     * @param url page url
     * @return object representation of the html page on given url
     */
    private Page download(String url) throws InterruptedException, PageBiggerThanMaxSizeException, IOException {
        WebURL curURL = new WebURL();
        curURL.setURL(url);
        PageFetchResult fetchResult = null;
        Page page;
        try {
            fetchResult = pageFetcher.fetchPage(curURL);
            if (fetchResult.getStatusCode() == HttpStatus.SC_MOVED_PERMANENTLY) {
                curURL.setURL(fetchResult.getMovedToUrl());
                fetchResult = pageFetcher.fetchPage(curURL);
            }
            if (fetchResult.getStatusCode() == HttpStatus.SC_OK) {
                try {
                    page = new Page(curURL);
                    fetchResult.fetchContent(page);
                    parser.parse(page, curURL.getURL());
                    return page;
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        } finally {
            if (fetchResult != null) {
                fetchResult.discardContentIfNotConsumed();
            }
        }
        return null;
    }

    /**
     * Downloads given url page and extracts xpath expressions.
     *
     * @param url      page url
     * @param xpathMap pairs of description and xpath expression
     * @return pairs of descriptions and extracted values
     */
    public Map<String, List<String>> processUrl(String url, Map<String, String> xpathMap) {
        Map<String, List<String>> results = new HashMap<String, List<String>>();

        log.info("Processing: " + url);
        Page page = null;
        try {
            page = download(url);
        } catch (InterruptedException e) {
            e.printStackTrace();
        } catch (PageBiggerThanMaxSizeException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        if (page != null) {
            ParseData parseData = page.getParseData();
            if (parseData != null) {
                if (parseData instanceof HtmlParseData) {
                    Document document = Jsoup.parse(((HtmlParseData) parseData).getHtml());

                    for (String key : xpathMap.keySet()) {
                        ArrayList<String> list = new ArrayList<String>();
                        list.addAll(Xsoup.compile(xpathMap.get(key)).evaluate(document).list());
                        results.put(key, list);
                    }
                }
            } else {
                log.info("Couldn't parse the content of the page.");
            }
        } else {
            log.info("Couldn't fetch the content of the page.");
            failedLinks.add(url);
        }
        return results;
    }


    /**
     * Downloads given url page and extracts xpath expression.
     *
     * @param url   page url
     * @param xPath xpath expression
     * @return list of extracted values
     */
    public List<String> getLinks(String url, String xPath) {
        ArrayList<String> list = new ArrayList<String>();
        log.info("Processing: " + url);
        Page page = null;
        try {
            page = download(url);
        } catch (InterruptedException e) {
            e.printStackTrace();
        } catch (PageBiggerThanMaxSizeException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        if (page != null) {
            ParseData parseData = page.getParseData();
            if (parseData != null) {
                if (parseData instanceof HtmlParseData) {
                    Document document = Jsoup.parse(((HtmlParseData) parseData).getHtml());
                    List<String> xlist = Xsoup.compile(xPath).evaluate(document).list();
                    list.addAll(xlist);
                }
            } else {
                log.info("Couldn't parse the content of the page.");
            }
        } else {
            log.info("Couldn't fetch the content of the page.");
            failedLinks.add(url);
        }
        return list;
    }

    @Override
    public void quit() {
        //no need to clean up anything
    }
}



