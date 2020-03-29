package cz.zcu.kiv.nlp.ir.trec.data;

/**
 * Created by pivov on 23-May-19.
 */
public class Posting {

    String documentID;
    int frequency = -1;
    double tfidf = -1;


    public Posting(String id){
        this.documentID = id;
        this.frequency = 1;
    }

    public String getDocumentID() { return documentID; }

    public void setDocumentID(String documentID) {
        this.documentID = documentID;
    }

    public int getFrequency() {
        return this.frequency;
    }

    public void setFrequency(int f) {
        this.frequency = f;
    }

    public void increaseFrequency() {
        this.frequency +=1;
    }

    public double getTfIdf() {
        return this.tfidf;
    }

    public void setTfIdf(double tfidf) {
        this.tfidf =tfidf;
    }

    @Override
    public String toString() {
        return "{documentID='" + documentID + '\'' +
                ", frequency=" + frequency +
                ", tf-idf=" + tfidf +
                '}';
    }
}
