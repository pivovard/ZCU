package cz.zcu.kiv.nlp.ir.trec.preprocessing;


/**
 * @author Ljiljana Dolamic  University of Neuchatel
 * -removes case endings form nouns and adjectives, possesive adj. endings from names,
 *  diminutive, augmentative, comparative sufixes and derivational sufixes from nouns,
 *  takes care of palatalisation
 */
public class CzechStemmerAgressive implements Stemmer{
    /**
     * A buffer of the current word being stemmed
     */
    private StringBuffer sb=new StringBuffer();

    /**
     * Default constructor
     */
    public CzechStemmerAgressive(){} // constructor

    public String stem(String input){
        //
        input=input.toLowerCase();
        //reset string buffer
        sb.delete(0,sb.length());
        sb.insert(0,input);
        // stemming...
        //removes case endings from nouns and adjectives
        removeCase(sb);
        //removes possesive endings from names -ov- and -in-
        removePossessives(sb);
        //removes comparative endings
        removeComparative(sb);
        //removes diminutive endings
        removeDiminutive(sb);
        //removes augmentatives endings
        removeAugmentative(sb);
        //removes derivational sufixes from nouns
        removeDerivational(sb);

       String result = sb.toString();
        return result;
    }
    private void removeDerivational(StringBuffer buffer) {
        int len=buffer.length();
        //
        if( (len > 8 )&&
                buffer.substring( len-6 ,len).equals("obinec")){

            buffer.delete( len- 6 , len);
            return;
        }//len >8
        if(len > 7){
            if(buffer.substring( len-5 ,len).equals("ion\u00e1\u0159")){ // -ionĂˇĹ™

                buffer.delete( len- 4 , len);
                palatalise(buffer);
                return;
            }
            if(buffer.substring( len-5 ,len).equals("ovisk")||
                    buffer.substring( len-5 ,len).equals("ovstv")||
                    buffer.substring( len-5 ,len).equals("ovi\u0161t")||  //-oviĹˇt
                    buffer.substring( len-5 ,len).equals("ovn\u00edk")){ //-ovnĂ­k

                buffer.delete( len- 5 , len);
                return;
            }
        }//len>7
        if(len > 6){
            if(	buffer.substring( len-4 ,len).equals("\u00e1sek")|| // -Ăˇsek
                    buffer.substring( len-4 ,len).equals("loun")||
                    buffer.substring( len-4 ,len).equals("nost")||
                    buffer.substring( len-4 ,len).equals("teln")||
                    buffer.substring( len-4 ,len).equals("ovec")||
                    buffer.substring( len-5 ,len).equals("ov\u00edk")|| //-ovĂ­k
                    buffer.substring( len-4 ,len).equals("ovtv")||
                    buffer.substring( len-4 ,len).equals("ovin")||
                    buffer.substring( len-4 ,len).equals("\u0161tin")){ //-Ĺˇtin

                buffer.delete( len- 4 , len);
                return;
            }
            if(buffer.substring( len-4 ,len).equals("enic")||
                    buffer.substring( len-4 ,len).equals("inec")||
                    buffer.substring( len-4 ,len).equals("itel")){

                buffer.delete( len- 3 , len);
                palatalise(buffer);
                return;
            }
        }//len>6
        if(len > 5){
            if(buffer.substring( len-3 ,len).equals("\u00e1rn")){ //-Ăˇrn

                buffer.delete( len- 3 , len);
                return;
            }
            if(buffer.substring( len-3 ,len).equals("\u011bnk")){ //-Ä›nk

                buffer.delete( len- 2 , len);
                palatalise(buffer);
                return;
            }
            if(buffer.substring( len-3 ,len).equals("i\u00e1n")|| //-iĂˇn
                    buffer.substring( len-3 ,len).equals("ist")||
                    buffer.substring( len-3 ,len).equals("isk")||
                    buffer.substring( len-3 ,len).equals("i\u0161t")|| //-iĹˇt
                    buffer.substring( len-3 ,len).equals("itb")||
                    buffer.substring( len-3 ,len).equals("\u00edrn")){  //-Ă­rn

                buffer.delete( len- 2 , len);
                palatalise(buffer);
                return;
            }
            if(buffer.substring( len-3 ,len).equals("och")||
                    buffer.substring( len-3 ,len).equals("ost")||
                    buffer.substring( len-3 ,len).equals("ovn")||
                    buffer.substring( len-3 ,len).equals("oun")||
                    buffer.substring( len-3 ,len).equals("out")||
                    buffer.substring( len-3 ,len).equals("ou\u0161")){  //-ouĹˇ

                buffer.delete( len- 3 , len);
                return;
            }
            if(buffer.substring( len-3 ,len).equals("u\u0161k")){ //-uĹˇk

                buffer.delete( len- 3 , len);
                return;
            }
            if(buffer.substring( len-3 ,len).equals("kyn")||
                    buffer.substring( len-3 ,len).equals("\u010dan")||    //-ÄŤan
                    buffer.substring( len-3 ,len).equals("k\u00e1\u0159")|| //kĂˇĹ™
                    buffer.substring( len-3 ,len).equals("n\u00e9\u0159")|| //nĂ©Ĺ™
                    buffer.substring( len-3 ,len).equals("n\u00edk")||      //-nĂ­k
                    buffer.substring( len-3 ,len).equals("ctv")||
                    buffer.substring( len-3 ,len).equals("stv")){

                buffer.delete( len- 3 , len);
                return;
            }
        }//len>5
        if(len > 4){
            if(buffer.substring( len-2 ,len).equals("\u00e1\u010d")|| // -ĂˇÄŤ
                    buffer.substring( len-2 ,len).equals("a\u010d")||      //-aÄŤ
                    buffer.substring( len-2 ,len).equals("\u00e1n")||      //-Ăˇn
                    buffer.substring( len-2 ,len).equals("an")||
                    buffer.substring( len-2 ,len).equals("\u00e1\u0159")|| //-ĂˇĹ™
                    buffer.substring( len-2 ,len).equals("as")){

                buffer.delete( len- 2 , len);
                return;
            }
            if(buffer.substring( len-2 ,len).equals("ec")||
                    buffer.substring( len-2 ,len).equals("en")||
                    buffer.substring( len-2 ,len).equals("\u011bn")||   //-Ä›n
                    buffer.substring( len-2 ,len).equals("\u00e9\u0159")){  //-Ă©Ĺ™

                buffer.delete( len-1 , len);
                palatalise(buffer);
                return;
            }
            if(buffer.substring( len-2 ,len).equals("\u00ed\u0159")|| //-Ă­Ĺ™
                    buffer.substring( len-2 ,len).equals("ic")||
                    buffer.substring( len-2 ,len).equals("in")||
                    buffer.substring( len-2 ,len).equals("\u00edn")||  //-Ă­n
                    buffer.substring( len-2 ,len).equals("it")||
                    buffer.substring( len-2 ,len).equals("iv")){

                buffer.delete( len- 1 , len);
                palatalise(buffer);
                return;
            }

            if(buffer.substring( len-2 ,len).equals("ob")||
                    buffer.substring( len-2 ,len).equals("ot")||
                    buffer.substring( len-2 ,len).equals("ov")||
                    buffer.substring( len-2 ,len).equals("o\u0148")){ //-oĹ�

                buffer.delete( len- 2 , len);
                return;
            }
            if(buffer.substring( len-2 ,len).equals("ul")){

                buffer.delete( len- 2 , len);
                return;
            }
            if(buffer.substring( len-2 ,len).equals("yn")){

                buffer.delete( len- 2 , len);
                return;
            }
            if(buffer.substring( len-2 ,len).equals("\u010dk")||              //-ÄŤk
                    buffer.substring( len-2 ,len).equals("\u010dn")||  //-ÄŤn
                    buffer.substring( len-2 ,len).equals("dl")||
                    buffer.substring( len-2 ,len).equals("nk")||
                    buffer.substring( len-2 ,len).equals("tv")||
                    buffer.substring( len-2 ,len).equals("tk")||
                    buffer.substring( len-2 ,len).equals("vk")){

                buffer.delete( len-2 , len);
                return;
            }
        }//len>4
        if(len > 3){
            if(buffer.charAt(buffer.length()-1)=='c'||
                    buffer.charAt(buffer.length()-1)=='\u010d'|| //-ÄŤ
                    buffer.charAt(buffer.length()-1)=='k'||
                    buffer.charAt(buffer.length()-1)=='l'||
                    buffer.charAt(buffer.length()-1)=='n'||
                    buffer.charAt(buffer.length()-1)=='t'){

                buffer.delete( len-1 , len);
            }
        }//len>3

    }//removeDerivational

    private void removeAugmentative(StringBuffer buffer) {
        int len=buffer.length();
        //
        if( (len> 6 )&&
                buffer.substring( len- 4 ,len).equals("ajzn")){

            buffer.delete( len- 4 , len);
            return;
        }
        if( (len> 5 )&&
                (buffer.substring( len- 3 ,len).equals("izn")||
                        buffer.substring( len- 3 ,len).equals("isk"))){

            buffer.delete( len- 2 , len);
            palatalise(buffer);
            return;
        }
        if( (len> 4 )&&
                buffer.substring( len- 2 ,len).equals("\00e1k")){ //-Ăˇk

            buffer.delete( len- 2 , len);
            return;
        }

    }

    private void removeDiminutive(StringBuffer buffer) {
        int len=buffer.length();
        //
        if( (len> 7 )&&
                buffer.substring( len- 5 ,len).equals("ou\u0161ek")){  //-ouĹˇek

            buffer.delete( len- 5 , len);
            return;
        }
        if( len> 6){
            if(buffer.substring( len-4,len).equals("e\u010dek")||      //-eÄŤek
                    buffer.substring( len-4,len).equals("\u00e9\u010dek")||    //-Ă©ÄŤek
                    buffer.substring( len-4,len).equals("i\u010dek")||         //-iÄŤek
                    buffer.substring( len-4,len).equals("\u00ed\u010dek")||    //Ă­ÄŤek
                    buffer.substring( len-4,len).equals("enek")||
                    buffer.substring( len-4,len).equals("\u00e9nek")||      //-Ă©nek
                    buffer.substring( len-4,len).equals("inek")||
                    buffer.substring( len-4,len).equals("\u00ednek")){      //-Ă­nek

                buffer.delete( len- 3 , len);
                palatalise(buffer);
                return;
            }
            if( buffer.substring( len-4,len).equals("\u00e1\u010dek")|| //ĂˇÄŤek
                    buffer.substring( len-4,len).equals("a\u010dek")||   //aÄŤek
                    buffer.substring( len-4,len).equals("o\u010dek")||   //oÄŤek
                    buffer.substring( len-4,len).equals("u\u010dek")||   //uÄŤek
                    buffer.substring( len-4,len).equals("anek")||
                    buffer.substring( len-4,len).equals("onek")||
                    buffer.substring( len-4,len).equals("unek")||
                    buffer.substring( len-4,len).equals("\u00e1nek")){   //-Ăˇnek

                buffer.delete( len- 4 , len);
                return;
            }
        }//len>6
        if( len> 5){
            if(buffer.substring( len-3,len).equals("e\u010dk")||   //-eÄŤk
                    buffer.substring( len-3,len).equals("\u00e9\u010dk")||  //-Ă©ÄŤk
                    buffer.substring( len-3,len).equals("i\u010dk")||   //-iÄŤk
                    buffer.substring( len-3,len).equals("\u00ed\u010dk")||    //-Ă­ÄŤk
                    buffer.substring( len-3,len).equals("enk")||   //-enk
                    buffer.substring( len-3,len).equals("\u00e9nk")||  //-Ă©nk
                    buffer.substring( len-3,len).equals("ink")||   //-ink
                    buffer.substring( len-3,len).equals("\u00ednk")){   //-Ă­nk

                buffer.delete( len- 3 , len);
                palatalise(buffer);
                return;
            }
            if(buffer.substring( len-3,len).equals("\u00e1\u010dk")||  //-ĂˇÄŤk
                    buffer.substring( len-3,len).equals("au010dk")|| //-aÄŤk
                    buffer.substring( len-3,len).equals("o\u010dk")||  //-oÄŤk
                    buffer.substring( len-3,len).equals("u\u010dk")||   //-uÄŤk
                    buffer.substring( len-3,len).equals("ank")||
                    buffer.substring( len-3,len).equals("onk")||
                    buffer.substring( len-3,len).equals("unk")){

                buffer.delete( len- 3 , len);
                return;

            }
            if(buffer.substring( len-3,len).equals("\u00e1tk")|| //-Ăˇtk
                    buffer.substring( len-3,len).equals("\u00e1nk")||  //-Ăˇnk
                    buffer.substring( len-3,len).equals("u\u0161k")){   //-uĹˇk

                buffer.delete( len- 3 , len);
                return;
            }
        }//len>5
        if( len> 4){
            if(buffer.substring( len-2,len).equals("ek")||
                    buffer.substring( len-2,len).equals("\u00e9k")||  //-Ă©k
                    buffer.substring( len-2,len).equals("\u00edk")||  //-Ă­k
                    buffer.substring( len-2,len).equals("ik")){

                buffer.delete( len- 1 , len);
                palatalise(buffer);
                return;
            }
            if(buffer.substring( len-2,len).equals("\u00e1k")||  //-Ăˇk
                    buffer.substring( len-2,len).equals("ak")||
                    buffer.substring( len-2,len).equals("ok")||
                    buffer.substring( len-2,len).equals("uk")){

                buffer.delete( len- 1 , len);
                return;
            }
        }
        if( (len> 3 )&&
                buffer.substring( len- 1 ,len).equals("k")){

            buffer.delete( len- 1, len);
            return;
        }
    }//removeDiminutives

    private void removeComparative(StringBuffer buffer) {
        int len=buffer.length();
        //
        if( (len> 5)&&
                (buffer.substring( len-3,len).equals("ej\u0161")||  //-ejĹˇ
                        buffer.substring( len-3,len).equals("\u011bj\u0161"))){   //-Ä›jĹˇ

            buffer.delete( len- 2 , len);
            palatalise(buffer);
            return;
        }

    }

    private void palatalise(StringBuffer buffer){
        int len=buffer.length();

        if( buffer.substring( len- 2 ,len).equals("ci")||
                buffer.substring( len- 2 ,len).equals("ce")||
                buffer.substring( len- 2 ,len).equals("\u010di")||      //-ÄŤi
                buffer.substring( len- 2 ,len).equals("\u010de")){   //-ÄŤe

            buffer.replace(len- 2 ,len, "k");
            return;
        }
        if( buffer.substring( len- 2 ,len).equals("zi")||
                buffer.substring( len- 2 ,len).equals("ze")||
                buffer.substring( len- 2 ,len).equals("\u017ei")||    //-Ĺľi
                buffer.substring( len- 2 ,len).equals("\u017ee")){  //-Ĺľe

            buffer.replace(len- 2 ,len, "h");
            return;
        }
        if( buffer.substring( len- 3 ,len).equals("\u010dt\u011b")||     //-ÄŤtÄ›
                buffer.substring( len- 3 ,len).equals("\u010dti")||   //-ÄŤti
                buffer.substring( len- 3 ,len).equals("\u010dt\u00ed")){   //-ÄŤtĂ­

            buffer.replace(len- 3 ,len, "ck");
            return;
        }
        if( buffer.substring( len- 2 ,len).equals("\u0161t\u011b")||   //-ĹˇtÄ›
                buffer.substring( len- 2 ,len).equals("\u0161ti")||   //-Ĺˇti
                buffer.substring( len- 2 ,len).equals("\u0161t\u00ed")){  //-ĹˇtĂ­

            buffer.replace(len- 2 ,len, "sk");
            return;
        }
        buffer.delete( len- 1 , len);
        return;
    }//palatalise

    private void removePossessives(StringBuffer buffer) {
        int len=buffer.length();

        if( len> 5 ){
            if( buffer.substring( len- 2 ,len).equals("ov")){

                buffer.delete( len- 2 , len);
                return;
            }
            if(buffer.substring( len-2,len).equals("\u016fv")){ //-ĹŻv

                buffer.delete( len- 2 , len);
                return;
            }
            if( buffer.substring( len- 2 ,len).equals("in")){

                buffer.delete( len- 1 , len);
                palatalise(buffer);
                return;
            }
        }
    }//removePossessives

    private void removeCase(StringBuffer buffer) {
        int len=buffer.length();
        //
        if( (len> 7 )&&
                buffer.substring( len- 5 ,len).equals("atech")){

            buffer.delete( len- 5 , len);
            return;
        }//len>7
        if( len> 6 ){
            if(buffer.substring( len- 4 ,len).equals("\u011btem")){   //-Ä›tem

                buffer.delete( len- 3 , len);
                palatalise(buffer);
                return;
            }
            if(buffer.substring( len- 4 ,len).equals("at\u016fm")){  //-atĹŻm
                buffer.delete( len- 4 , len);
                return;
            }

        }
        if( len> 5 ){
            if(buffer.substring( len-3,len).equals("ech")||
                    buffer.substring( len-3,len).equals("ich")||
                    buffer.substring( len-3,len).equals("\u00edch")){ //-Ă­ch

                buffer.delete( len-2 , len);
                palatalise(buffer);
                return;
            }
            if(buffer.substring( len-3,len).equals("\u00e9ho")|| //-Ă©ho
                    buffer.substring( len-3,len).equals("\u011bmi")||  //-Ä›mu
                    buffer.substring( len-3,len).equals("emi")||
                    buffer.substring( len-3,len).equals("\u00e9mu")||  // -Ă©mu				                                                                buffer.substring( len-3,len).equals("ete")||
                    buffer.substring( len-3,len).equals("eti")||
                    buffer.substring( len-3,len).equals("iho")||
                    buffer.substring( len-3,len).equals("\u00edho")||  //-Ă­ho
                    buffer.substring( len-3,len).equals("\u00edmi")||  //-Ă­mi
                    buffer.substring( len-3,len).equals("imu")){

                buffer.delete( len- 2 , len);
                palatalise(buffer);
                return;
            }
            if( buffer.substring( len-3,len).equals("\u00e1ch")|| //-Ăˇch
                    buffer.substring( len-3,len).equals("ata")||
                    buffer.substring( len-3,len).equals("aty")||
                    buffer.substring( len-3,len).equals("\u00fdch")||   //-Ă˝ch
                    buffer.substring( len-3,len).equals("ama")||
                    buffer.substring( len-3,len).equals("ami")||
                    buffer.substring( len-3,len).equals("ov\u00e9")||   //-ovĂ©
                    buffer.substring( len-3,len).equals("ovi")||
                    buffer.substring( len-3,len).equals("\u00fdmi")){  //-Ă˝mi

                buffer.delete( len- 3 , len);
                return;
            }
        }
        if( len> 4){
            if(buffer.substring( len-2,len).equals("em")){

                buffer.delete( len- 1 , len);
                palatalise(buffer);
                return;

            }
            if( buffer.substring( len-2,len).equals("es")||
                    buffer.substring( len-2,len).equals("\u00e9m")||    //-Ă©m
                    buffer.substring( len-2,len).equals("\u00edm")){   //-Ă­m

                buffer.delete( len- 2 , len);
                palatalise(buffer);
                return;
            }
            if( buffer.substring( len-2,len).equals("\u016fm")){

                buffer.delete( len- 2 , len);
                return;
            }
            if( buffer.substring( len-2,len).equals("at")||
                    buffer.substring( len-2,len).equals("\u00e1m")||    //-Ăˇm
                    buffer.substring( len-2,len).equals("os")||
                    buffer.substring( len-2,len).equals("us")||
                    buffer.substring( len-2,len).equals("\u00fdm")||     //-Ă˝m
                    buffer.substring( len-2,len).equals("mi")||
                    buffer.substring( len-2,len).equals("ou")){

                buffer.delete( len- 2 , len);
                return;
            }
        }//len>4
        if( len> 3){
            if(buffer.substring( len-1,len).equals("e")||
                    buffer.substring( len-1,len).equals("i")){

                palatalise(buffer);
                return;
            }
            if(buffer.substring( len-1,len).equals("\u00ed")||    //-Ă©
                    buffer.substring( len-1,len).equals("\u011b")){   //-Ä›

                palatalise(buffer);
                return;
            }
            if( buffer.substring( len-1,len).equals("u")||
                    buffer.substring( len-1,len).equals("y")||
                    buffer.substring( len-1,len).equals("\u016f")){   //-ĹŻ

                buffer.delete( len- 1 , len);
                return;
            }
            if( buffer.substring( len-1,len).equals("a")||
                    buffer.substring( len-1,len).equals("o")||
                    buffer.substring( len-1,len).equals("\u00e1")||  // -Ăˇ
                    buffer.substring( len-1,len).equals("\u00e9")||  //-Ă©
                    buffer.substring( len-1,len).equals("\u00fd")){   //-Ă˝

                buffer.delete( len- 1 , len);
                return;
            }
        }//len>3
    }


	public String getRevision() {
		// TODO Auto-generated method stub
		return null;
	}




}


