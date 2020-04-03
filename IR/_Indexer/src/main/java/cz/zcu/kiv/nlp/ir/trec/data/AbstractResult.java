package cz.zcu.kiv.nlp.ir.trec.data;

/**
 * Created by Tigi on 6.1.2015.
 */
public abstract class AbstractResult implements Result, Comparable<Result> {
    String documentID;
    int rank = -1;
    double score = -1;

    public String getDocumentID() {
        return documentID;
    }

    public double getScore() {
        return score;
    }

    public void setDocumentID(String documentID) {
        this.documentID = documentID;
    }

    public void setRank(int rank) {
        this.rank = rank;
    }

    public int getRank() {
        return rank;
    }

    public void setScore(double score) {
        this.score = score;
    }

    @Override
    public String toString() {
        return "Result{" +
                "documentID='" + documentID + '\'' +
                ", rank=" + rank +
                ", score=" + score +
                '}';
    }

    public String toString(String topic) {
        return topic + " Q0 " + documentID + " " + rank + " " + score + " runindex1";
    }
}
