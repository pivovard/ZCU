package cz.zcu.kiv.nlp.ir.trec.data;

/**
 * Created by Tigi on 8.1.2015.
 */
public class ResultImpl extends AbstractResult {

    public ResultImpl(String id, double score){
        this.documentID = id;
        this.score = score;
    }

    public int compareTo(Result o) {
        return ((Double)o.getScore()).compareTo(this.score);
    }
}
