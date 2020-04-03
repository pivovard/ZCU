package cz.zcu.kiv.nlp.ir.trec.data;

import java.io.Serializable;
import java.util.Date;

/**
 * Created by Tigi on 8.1.2015.
 */
public class DocumentNew implements Document, Serializable {
    String text;
    String id;
    String title;
    Date date;
    int size = 0;
    final static long serialVersionUID = -5097715898427114007L;

    @Override
    public String toString() {
        return "DocumentNew{" +
                ", id='" + id + '\'' +
                ", title='" + title + '\'' +
                ", date=" + date +
                "text='" + text + '\'' +
                '}';
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }


    public Date getDate() {
        return date;
    }

    public int getSize() {
        return this.size;
    }

    public void setSize(int s) {
        this.size = s;
    }

    public void setDate(Date date) {
        this.date = date;
    }
}
