package cz.zcu.kiv.nlp.ir;

import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.firefox.FirefoxDriver;
import us.codecraft.xsoup.Xsoup;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * This class is a demonstration of how crawler4j can be used to download a website
 * Created by Tigi on 31.10.2014.
 */
public class HTMLDownloaderSelenium extends AbstractHTMLDownloader {

    WebDriver driver;

    /**
     * Constructor
     */
    public HTMLDownloaderSelenium() {
        super();
        System.setProperty("webdriver.chrome.driver", "./chromedriver.exe");
        driver = new ChromeDriver();
    }

    /**
     * Quit driver/browser
     */
    public void quit() {
        driver.quit();
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
        driver.get(url);
        String dom = driver.getPageSource();
        if (dom != null) {
            Document document = Jsoup.parse(dom);

            for (String key : xpathMap.keySet()) {
                ArrayList<String> list = new ArrayList<String>();
                list.addAll(Xsoup.compile(xpathMap.get(key)).evaluate(document).list());
                results.put(key, list);
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
        driver.get(url);
        String dom = driver.getPageSource();
        if (dom != null) {
            Document document = Jsoup.parse(dom);
            List<String> xlist = Xsoup.compile(xPath).evaluate(document).list();
            list.addAll(xlist);
        } else {
            log.info("Couldn't fetch the content of the page.");
            failedLinks.add(url);
        }
        return list;
    }
}



