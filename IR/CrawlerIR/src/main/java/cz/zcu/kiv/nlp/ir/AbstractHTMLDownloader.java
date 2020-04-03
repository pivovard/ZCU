package cz.zcu.kiv.nlp.ir;

import org.apache.log4j.Logger;

import java.util.HashSet;
import java.util.Set;

/**
 * This class is a demonstration of how crawler can be used to download a website
 * Created by Tigi on 31.10.2014.
 */
public abstract class AbstractHTMLDownloader implements HTMLDownloaderInterface {

    static final Logger log = Logger.getLogger(AbstractHTMLDownloader.class);
    Set<String> failedLinks = new HashSet<String>();

    /**
     * Get failed links.
     *
     * @return failed links
     */
    public Set<String> getFailedLinks() {
        return failedLinks;
    }

    /**
     * Empty the empty links set
     */
    public void emptyFailedLinks() {
        failedLinks = new HashSet<String>();
    }
}



