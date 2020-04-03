package cz.zcu.kiv.nlp.ir;

import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * This class is a demonstration of how crawler can be used to download a website
 * Created by Tigi on 31.10.2014.
 */
public interface HTMLDownloaderInterface {

    /**
     * Get failed links.
     *
     * @return failed links
     */
    public Set<String> getFailedLinks();

    /**
     * Empty the empty links set
     */
    public void emptyFailedLinks();


    /**
     * Downloads given url page and extracts xpath expressions.
     *
     * @param url      page url
     * @param xpathMap pairs of description and xpath expression
     * @return pairs of descriptions and extracted values
     */
    public Map<String, List<String>> processUrl(String url, Map<String, String> xpathMap);


    /**
     * Downloads given url page and extracts xpath expression.
     *
     * @param url   page url
     * @param xPath xpath expression
     * @return list of extracted values
     */
    public List<String> getLinks(String url, String xPath);

    /**
     * Quit driver/browser
     */
    public void quit();
}



