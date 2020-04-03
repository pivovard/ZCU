package cz.zcu.kiv.nlp.ir.trec.data;

import java.io.Serializable;

/**
 * Created by Tigi on 8.1.2015.
 */
public class Topic implements Serializable {
    String narrative;
    String description;
    String id;
    String title;
    String lang;

    public String getNarrative() {
        return narrative;
    }

    public void setNarrative(String narrative) {
        this.narrative = narrative;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getLang() {
        return lang;
    }

    public void setLang(String lang) {
        this.lang = lang;
    }

    @Override
    public String toString() {
        return "Topic{" +
                "narrative='" + narrative + '\'' +
                ", description='" + description + '\'' +
                ", id='" + id + '\'' +
                ", title='" + title + '\'' +
                ", lang='" + lang + '\'' +
                '}';
    }
}
