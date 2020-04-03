package cz.zcu.kiv.nlp.ir.trec.data;

/**
 * Created by Tigi on 6.1.2015.
 */
public interface Result {

    String getDocumentID();

    double getScore();

    int getRank();

    void setRank(int rank);

    String toString(String topic);
}
