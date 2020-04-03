package cz.zcu.kiv.nlp.vs;

import java.io.*;
import java.util.ArrayList;
import java.util.List;

/**
 * Created with IntelliJ IDEA.
 * User: tigi
 * Date: 13.11.12
 * Time: 10:25
 * To change this template use File | Settings | File Templates.
 */
public class Record implements Serializable {
    String school;
    String typStudia;
    String studijniObor;
    String studijniProgram;
    String zamereni;
    String url;
    List<String> texts = new ArrayList<>();

    public Record() {
    }

    public String getSchool() {
        return school;
    }

    public void setSchool(String school) {
        this.school = school;
    }

    public String getTypStudia() {
        return typStudia;
    }

    public void setTypStudia(String typStudia) {
        this.typStudia = typStudia;
    }

    public String getStudijniObor() {
        return studijniObor;
    }

    public void setStudijniObor(String studijniObor) {
        this.studijniObor = studijniObor;
    }

    public String getStudijniProgram() {
        return studijniProgram;
    }

    public void setStudijniProgram(String studijniProgram) {
        this.studijniProgram = studijniProgram;
    }

    public String getZamereni() {
        return zamereni;
    }

    public void setZamereni(String zamereni) {
        this.zamereni = zamereni;
    }

    public String getUrl() {
        return url;
    }

    public void setUrl(String url) {
        this.url = url;
    }

    public List<String> getTexts() {
        return texts;
    }

    public void setTexts(List<String> texts) {
        this.texts = texts;
    }

    @Override
    public String toString() {
        return "Record{" +
                "school='" + school + '\'' +
                ", typStudia='" + typStudia + '\'' +
                ", studijniObor='" + studijniObor + '\'' +
                ", studijniProgram='" + studijniProgram + '\'' +
                ", zamereni='" + zamereni + '\'' +
                ", url='" + url + '\'' +
                ", texts=" + texts +
                '}';
    }

    public static void save(String path, List<Record> list) {
        try {
            FileOutputStream fileOut = new FileOutputStream(path);
            ObjectOutputStream out = new ObjectOutputStream(fileOut);
            out.writeObject(list);
            out.close();
            fileOut.close();
            System.out.printf("Serialized data is saved in /tmp/employee.ser");
        } catch (IOException i) {
            i.printStackTrace();
        }
    }

    public static List<Record> load(File serializedFile) {
        final Object object;
        try {
            final ObjectInputStream objectInputStream = new ObjectInputStream(new FileInputStream(serializedFile));
            object = objectInputStream.readObject();
            objectInputStream.close();
            return (List<Record>) object;
        } catch (Exception ex) {
            throw new RuntimeException(ex);
        }
    }
}
