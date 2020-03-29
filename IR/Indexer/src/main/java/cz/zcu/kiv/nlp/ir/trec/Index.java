package cz.zcu.kiv.nlp.ir.trec;

import cz.zcu.kiv.nlp.ir.trec.data.*;
import cz.zcu.kiv.nlp.ir.trec.preprocessing.*;

import java.lang.reflect.Array;
import java.util.*;

/**
 * @author tigi
 */

public class Index implements Indexer, Searcher {

    private boolean toLowercase;
    private boolean removeAccentsBeforeStemming;
    private boolean removeAccentsAfterStemming;

    private Stemmer stemmer;
    private Tokenizer tokenizer;
    private Set<String> stopwords;

    public Map<String, Map<String, Posting>> invertedIndex = new HashMap<String, Map<String, Posting>>();

    Map<String, Document> documents = new HashMap<String, Document>();

    /**
     * Creates default indexer
     */
    Index() {
        this(new AdvancedTokenizer(), new CzechStemmerAgressive(), false, true, true);
    }

    /**
     * Creates custom indexer
     * @param tokenizer
     * @param stemmer
     * @param removeAccentsBeforeStemming
     * @param removeAccentsAfterStemming
     * @param toLowercase
     */
    Index(Tokenizer tokenizer, Stemmer stemmer, boolean removeAccentsBeforeStemming, boolean removeAccentsAfterStemming, boolean toLowercase) {
        this.tokenizer = tokenizer;
        this.stemmer = stemmer;

        this.removeAccentsAfterStemming = removeAccentsAfterStemming;
        this.removeAccentsBeforeStemming = removeAccentsBeforeStemming;
        this.toLowercase = toLowercase;

        stopwords = StopwordsReader.Read();
    }


//region index

    /**
     * Index all documents
     * @param documents
     */
    public void index(List<Document> documents) {
        for (Document document : documents) {
            processDocument(document);
            this.documents.put(document.getId(), document);
        }
        computeTfIdf();
    }

    /**
     * Index document
     * @param document
     */
    public void index(Document document){
        processDocument(document);
        this.documents.put(document.getId(), document);

        computeTfIdf();
    }

    /**
     * Update document in index
     * @param documents
     */
    public void update(List<Document> documents) {
        for (Document document : documents) {
            delete(document);
            index(document);
        }
        computeTfIdf();
    }

    /**
     * Update document in index
     * @param document
     */
    public void update(Document document){
        delete(document);
        index(document);

        computeTfIdf();
    }

    /**
     * Removes document from index
     * @param document
     */
    public void delete(Document document){
        for(Map<String, Posting> res : invertedIndex.values()){
            res.remove(document.getId());
        }
        this.documents.remove(document);

        computeTfIdf();
    }

    /**
     * Removes document from index
     * @param docId
     */
    public void delete(String docId){
        for(Map<String, Posting> res : invertedIndex.values()){
            res.remove(docId);
        }
        this.documents.remove(docId);

        computeTfIdf();
    }

    /**
     * Preprocess document and create inverted index
     * @param document
     */
    private void processDocument(Document document) {
        List<String> tokens = new ArrayList<String>();

        tokens.addAll(processString(document.getTitle()));
        tokens.addAll(processString(document.getText()));
        tokens.addAll(processString(document.getDate().toString()));

        document.setSize(tokens.size());

        for (String token : tokens) {
            if(invertedIndex.containsKey(token)){
                if(invertedIndex.get(token).containsKey(document.getId())){
                    invertedIndex.get(token).get(document.getId()).increaseFrequency();
                }
                else{
                    invertedIndex.get(token).put(document.getId(), new Posting(document.getId()));
                }
            }
            else{
                invertedIndex.put(token, new HashMap<String, Posting>());
                invertedIndex.get(token).put(document.getId(), new Posting(document.getId()));
            }
        }
    }

    /**
     * Preprocess text:
     * lower case, tokenize, stem, remove accents, remove stopwords
     * @param text
     * @return
     */
    private List<String> processString(String text) {
        List<String> tokens = new ArrayList<String>();

        if (toLowercase) {
            text = text.toLowerCase();
        }
        if (removeAccentsBeforeStemming) {
            text = AdvancedTokenizer.removeAccents(text);
        }
        for (String token : tokenizer.tokenize(text)) {

            token = stemmer.stem(token);

            if (removeAccentsAfterStemming) {
                token = AdvancedTokenizer.removeAccents(token);
            }
            if (stopwords.contains(token)) {
                continue;
            }
            tokens.add(token);
        }
        return tokens;
    }

    /**
     * Compute tf-idf for all terms in documents
     */
    private void computeTfIdf(){
        for(String term : invertedIndex.keySet()){
            double idf = Math.log10((double)documents.size()/invertedIndex.get(term).size());

            for(Posting res : invertedIndex.get(term).values()){
                double tf = (double)res.getFrequency() / documents.get(res.getDocumentID()).getSize();

                res.setTfIdf(tf*idf);
                //res.setTfIdf((1+Math.log10(tf))*idf);
            }
        }
    }

//endregion

//region search

    /**
     * Returns list of results of documents that contain query
     * @param query_in
     * @return
     */
    public List<Result> search(String query_in) {
        List<Result> results = new ArrayList<Result>();
        int k = 1000;

        List<String> query = processString(query_in);
        //<operand, terms>
        Map<String, List<String>> logic = getLogic(query);
        query.removeAll(logic.keySet());    //remove AND/OR/NOT

        double[] tfidf = computeTfIdfQuery(query);
        double wq = normalizeVector(tfidf);

        Map<String, double[]> weights = getWeights(query, logic);
        Map<String, Double> wd = getNormalizedWeights(weights);

        System.gc(); //clear memory to prevent OutOfMemoryException

        //count score
        for(String docId : wd.keySet()){
            double score = scalar(tfidf, weights.get(docId)) / (wq*wd.get(docId));
            results.add(new ResultImpl(docId, score));
        }

        Collections.sort(results, (r1,r2)->((Double)r2.getScore()).compareTo(r1.getScore())  );
        //results = results.subList(0, k);

        int i = 0;
        for(Result res : results){
            res.setRank(i);
            i++;
        }

        return results;
    }

    /**
     * Returns map of terms for logic operations
     * @param query
     * @return
     */
    private Map<String, List<String>> getLogic(List<String> query){
        Map<String, List<String>> logic = new HashMap<String, List<String>>();
        logic.put("AND", new ArrayList<String>());
        logic.put("OR", new ArrayList<String>());
        logic.put("NOT", new ArrayList<String>());

        for (int i = 1; i < query.size() -1; i++){
            if(query.get(i).equals("and")){
                logic.get("AND").add(query.get(i-1));
                logic.get("AND").add(query.get(i+1));
            }
            if(query.get(i).equals("or")){
                logic.get("OR").add(query.get(i-1));
                logic.get("OR").add(query.get(i+1));
            }
            if(query.get(i).equals("not")){
                logic.get("NOT").add(query.get(i+1));
            }
        }
        return logic;
    }

    /**
     * Evaluation of logic operations
     * @param weights
     * @return
     */
    private Map<String, double[]> applyLogic(List<String> query, Map<String, List<String>> logic, Map<String, double[]> weights){
        //apply logic
        List<String> removeList = new ArrayList<String>();

        for (String docId : weights.keySet()) {
            double[] vector = weights.get(docId);

            List<String> exp = logic.get("AND");
            for (int i = 0; i < exp.size(); i += 2) {
                int pos1 = query.indexOf(exp.get(i));
                int pos2 = query.indexOf(exp.get(i+1));
                if (vector[pos1] == 0 || vector[pos2] == 0){
                    //weights.remove(docId);
                    removeList.add(docId);
                    continue;
                }
            }
            exp = logic.get("OR");
            for (int i = 0; i < exp.size(); i += 2) {
                int pos1 = query.indexOf(exp.get(i));
                int pos2 = query.indexOf(exp.get(i+1));
                if (vector[pos1] == 0 && vector[pos2] == 0){
                    //weights.remove(docId);
                    removeList.add(docId);
                    continue;
                }
            }
            exp = logic.get("NOT");
            for (int i = 0; i < exp.size(); i++) {
                int pos = query.indexOf(exp.get(i));
                if (vector[pos] > 0){
                    //weights.remove(docId);
                    removeList.add(docId);
                    continue;
                }
            }
        }
        //remove only if something left
        if(removeList.size() < weights.size()){
            //weights.remove(removeList);
            for(String docId : removeList){
                weights.remove(docId);
            }
        }
        return weights;
    }

    /**
     * Compute tf-idf of query
     * @param query
     * @return
     */
    private double[] computeTfIdfQuery(List<String> query){
        double[] tfidf = new double[query.size()];

        for(int i = 0; i < query.size(); i++){
            //for(String term : query){
            if(!invertedIndex.containsKey(query.get(i))) continue;

            double idf = Math.log10((double)documents.size()/invertedIndex.get(query.get(i)).size());
            double tf = 1.0 / query.size();

            tfidf[i] = tf*idf;
        }
        return tfidf;
    }

    /**
     * Returns weights of documents that contain query terms
     * @param query
     * @param logic
     * @return
     */
    private Map<String, double[]> getWeights(List<String> query, Map<String, List<String>> logic) {
        //get weight vector
        //<docID, vector>
        Map<String, double[]> weights = new HashMap<String, double[]>();

        for (int i = 0; i < query.size(); i++) {
            if (!invertedIndex.containsKey(query.get(i))) continue;

            for (Posting post : invertedIndex.get(query.get(i)).values()) {
                if (!weights.containsKey(post.getDocumentID())) {
                    weights.put(post.getDocumentID(), new double[query.size()]);
                }
                weights.get(post.getDocumentID())[i] = post.getTfIdf();
            }
        }

        //apply logic
        weights = applyLogic(query, logic, weights);

        return weights;
    }

    /**
     * Returns normalized weights of documents that contain query terms
     */
    private Map<String, Double> getNormalizedWeights(Map<String, double[]> weights){
        //normalize vectors
        //<docId, score>
        Map<String, Double> wd = new HashMap<String, Double>();

        for(String docId : weights.keySet()){
            double w = normalizeVector(weights.get(docId));
            wd.put(docId, w);
        }

        return wd;
    }

    /**
     * Normalize weight vector
     * @param vector
     * @return
     */
    private double normalizeVector(double[] vector) {
        double sum = 0.0;
        for (double x : vector) {
            sum += x * x;
        }
        return Math.sqrt(sum);
    }

    /**
     * Scalar product of 2 vectors
     * @param a
     * @param b
     * @return
     */
    private double scalar(double[] a, double[] b){
        double sum = 0;
        for(int i = 0; i < a.length; i++){
            sum += a[i]*b[i];
        }
        return sum;
    }

    //endregion
}
