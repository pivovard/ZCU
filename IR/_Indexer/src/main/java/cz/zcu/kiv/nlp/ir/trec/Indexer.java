package cz.zcu.kiv.nlp.ir.trec;

import cz.zcu.kiv.nlp.ir.trec.data.Document;

import java.util.List;

/**
 * Created by Tigi on 6.1.2015.
 */
public interface Indexer {
    void index(List<Document> documents);
}
