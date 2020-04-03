package cz.zcu.kiv.nlp.ir;


import java.util.*;

/**
 * Created by Tigi on 29.2.2016.
 */
public class BasicPreprocessingExample {

    static Stemmer stemmer = new CzechStemmerAgressive();

    public static void main(String[] args) {
        String text =
                "čáú jak se máš?" +
                        "<li> o co jsti se snažil a jak,</li>\n" +
                        "<li> jakým výsledkem skončila vaše akce,</li>\n" +
                        "<li> v kolik hodin jste akci prováděli,</li>\n" +
                        "studentský průkaz je jiska\n" +
                        " Sooperméďa. A přesně v souladu s názvem. Poolka filmu je opravdu mrtvá. To jsou ty momenty, kdy se metrosexooální narcis Gájen Gejnolds prochází po přehlídkovém moloo a snaží se vám oorpootně a za každou cenu narvat nagelovanou rookojeť katany mezi poolky. Tehdy dostáváte kýčovitou, místy až debilně felální trapárnoo pro retardy, kombinovanou s dost soochým až noodným dějem a zbytečně sympatickým záporákem. Ale pokood jste třeba nesmírně seriózně beroucí se, sebestředný, slizký, plešatějící emoteplouš jménem Robert, určitě si i tak SMÍCHY naprskáte flitry do popkornoo a samým vzroošením rozmažete řasenkoo. Naštěstí je tu ale ta druhá poolka, a ta je opravdoo výživná! To jsou ty momenty, kdy se Gájen nasouká do pro něj příhodného červeného latexoo a stane se z něj cynický hláškoojící zmrd, který to všem těm otylým čtenářoom omalovánek napálí bez servítkoo přímo do xichtoo. Tehdy dostáváte koolantní a krvavou akcičkoo, napěchovanou brootálními bonbonmoty, trefným popkooltoorním laškováním, ponižováním Xmenů a děláním kokota z Volverína. Škoda jen, že toohle ooroveň neoodrželi celých sto minut, ale jinak po Kikásu a Strážcích galaxie třetí vlaštovka, která ukazooje, že když se leporela pro dyslektiky přestanou brát mentorsky a vážně a necílí na dvanáctileté mentální mootanty či morálně neposqrněné šosácké zroody, moože z toho být solidní taškařice. A moc rád bych v boodoucnu viděl homixový krosouvr Deadpool vs. Kapitán Homokokot: Oosvit oplzlosti.(20.2.2016)\n" +
                        "všechny komentáře uživatele / [Trvalý odkaz: http://www.csfd.cz/film/261379-deadpool/komentare/?comment=10355101]\n" +
                        "koukáte na to? ****\n" +
                        "Deadpool mě příjemně překvapil. Ne post/pubertálním humorem, ten jsem spíš přežil. Ani ne roztomilou sebeuvědomělostí procesu vyprávění, neb ta patří k základní narativní výbavě současné zábavné fikce. S čím však Deadpool pracuje vskutku důmyslně, je výstavba osnovy jeho vyprávění. Časová pásma, odbočky, situace - staví na nenápadném variování účelně omezeného množství vzorců, takže i přes snahu diváka neustále překvapovat nakonec nepůsobí nestřídmě. Jo, na povrchu jde o romantický příběh na pozadí sebestředné komiksové parodie, ale výstavbou je to vlastně stará dobrá detektivka drsné školy. V lecčem klasičtější než mnohé z těch, které se k této tradici v posledních dekádách hlásí.(11.2.2016)\n" +
                        "všechny komentáře uživatele / [Trvalý odkaz: http://www.csfd.cz/film/261379-deadpool/komentare/?comment=10358805]\n" +
                        "sákrýš ty jsou tak nekorektní až to bolí... ****\n" +
                        "Koukat se na Deadpoola ,to je trochu jako jít po Václaváku s ožralým kámošem, který si stoupne uprostřed chodníku a začne chcát. Každých deset vteřin zařve: BACHA MÁM V RUCE PTÁKA!, což je nejdřív docela funny, ale pak se to stane krapet předvídatelné a únavné. Deadpool je odlišný ne tím, že by se tolik od ostatních superheroes lišil, ale tím, že odlišnost neustále tematizuje a sype divákům do ksichtu. Jinak je úplně stejně průhledný jako CapAm, jen tam, kde se kapitán chová jako Dušín, se Deadpool nutně zachová vždy jako kokot. Je to prostě modelový návrat potlačeného. Marvel tak dlouho odsouval násilí, vulgaritu a sex ze svých filmů, až vzniklo dost materiálu na Deadpoola, který ukázkově zaplňuje všechny díry (pěstí). Funguje to jako objednávkový fan service a lubrikant pro další X-Meny, kde se určitě nebude klít a masturbovat. A stejně tak pro celé marvelovské univerzum, ať už za ním stojí kdokoli. Neberte to špatně - hlášky jsou bezva, akce fajn. Ale pod pozlátkem frků vo korektnosti a honění péra je v jádru úplně stejně jalová romantická story se špatným padouchem (Ed Skrein = lame), jako v případě mnoha dalších komiksáků. Deadpool vydělává na tom, že na svoje slabiny poukáže, ale výsledkem není tak zábavná a soudržná podívaná jako Strážci galaxie, ale spíš zmateně kličkující přešívka, která svoje slabiny maskuje pubertálními výstřelky. Jen to z mého pohledu nefunguje jako film, ale spíš jako fanboyovská směsice gagů s proměnlivou úrovní. S rostoucí stopáží roste i pocit, že film jede na automat a na jeden dobrý gag vychrlí tří průměrné. Takže OK, beru, ale okouzlení Kick-Assem se neopakuje, Vaughn trotiž dovede chcát proti větru i bez toho, aby vám stokrát zdůraznil, že při tom drží v ruce ptáka a to se nedělá. Škoda, že mi není o 20 let míň. Jak správně poznamenal kolega Samohan Řepák: byl by to nejlepší film, co jsem kdy viděl. Tohle musím v tradici českého filmu překřtít na SUPERHRDINSKI FILM. [60%]\n" +
                        "Nerikam, ze mi hlaskovaci postava Deadpoola nebo ksicht Ryana Reynoldse sedl uplne dokonale, ani ze mi vsech 1000 vtipku prislo vtipnych, ale tohle je v dobe kdy se masy hrnou do kin na plochy, nenapadity, okoukany, najisto vydelecny akcni mrdky, nenatocis pomalu nic drzyho a nekrestanskyho za poradny prachy, komiksovej boom kolikrat uz sakra nudi... tak tohle je proste skvela kapka zivy vody. A ja mam velkou radost, ze to zvalcuje pokladny kin po cely planete (s vyjimkou tam kde to neprojde pres cenzuru), protoze to je jednoznacnej signal, ze chceme tocit i filmy pro dospely, jazykem co nas mluvi vetsina a s ujetym humorem, co ma kurva beztak tolik z vas. Jo, chceme i NEKOREKTNI filmy pro lidi co nejsou nudny upjaty pici.\n" +
                        "Z malého/bezvýznamného štěku ve Wolverinovi rovnou do první ligy? Tohle srazilo všechny odborníky (přes kvalitu - kritiky, ano :) - i tržby) do kolen. Nevěřil jsem, že ten film může fungovat. Obavy z roztříštěnosti filmu do jednotlivých vtipných scén se ale nenaplnily a to halvně díky skvěle zvolené struktuře vyprávění. Deadpool ale hlavně láká diváky na specifický humor (kdo sledoval internet, sociální sítě a plakáty okolo Deadpoola, ten ví...) a tady nabírá film opravdu na obrátkách. Je tu hromada narážek na jiné filmy, na Wolverina, na Xmeny, na casting filmů... a pak je tu tuna sexistických vtipů, které pobaví většinou největší pr*sata v sále (a jejich partnerky, co se smějí také všemu:)). Když nad tím zpětně přemýšlím, většina těchto \"Im touching myself tonight\" vtipů je vlatně zbytečná a film by fungoval i bez nich (a jejich kvalita je neuvěřitelně různorodá). Sečteno podtrženo, tohle je jiná komiksárna, sprostá, odvážná a procenty hodně nadhodnocená, ale i přesto zábavná! PPS: je to vlastně správně, aby si film uvědomoval sám sebe a kritizoval okolní universa nebo obsazení rolí?:) To už je na jinou diskuzi... PS: jsem sám zvědav, co ještě se z prvního Wolverina \"odřízne\" vzhledem k tomu, že Deadpool má svůj restart, Gambita čeká to samé...(15.2.2016)\n" +
                        "všechny komentáře uživatele / [Trvalý odkaz: http://www.csfd.cz/film/261379-deadpool/komentare/?comment=10360720]\n" +
                        "to je BOMBA ****\n" +
                        "Po prvním shlédnutí traileru jsem Deadpoolovi moc nevěřil, ale nakonec musím říct, že je to fakticky bomba. Nicméně zdejší hodnocení mě vysloveně vytáhlo do kina, abych se nakonec sám přesvědčil. Výsledek je takovej, že Deadpool přesně splnil, pro co byl určenej. První chvíle sice byly dost rozpačitý. Po půl hodině jsem nic moc nevěděl, co si o filmu myslet, ale jakmile Deadpool rozjel trojboj v hláškované, neměl konkurenci a sypal to ze sebe jak bábovičky písek. V tu chvíli jsem si užíval naprosto boží narážky na všechny možný a nemožný postavy superhrdinského univerza a přemýšlel nad tím, jestli takovej film někdo za dvacet, třicet let ocení. Ve výsledku je to stejně ale jedno, protože tržby se tvoří teď a tady a ty v tuhle chvíli mluví za vše." +
                        "<li> jméno, příjmení, orion login, studentské číslo.</li>" +
                        "Tablet PC - Intel Atom Quad Core Z3735F, kapacitní multidotykový IPS 10.1\" LED 1280x800, Intel HD Graphics, RAM 2GB, 64GB eMMC, WiFi, Bluetooth 4.0, webkamera 2 Mpx + 5 Mpx, 2článková baterie, Windows 10 Home 32bit + MS Office Mobile";

        final String[] tokenize = split(text, " ");
        Arrays.sort(tokenize);
        System.out.println(Arrays.toString(tokenize));

        System.out.println();

        final ArrayList<String> documents = new ArrayList<String>();
        for (String document : split(text, "\n")) {
            documents.add(document);
        }

//        wordsStatistics(documents, " ", false, null, false, false);

        System.out.println();
        System.out.println("-------------------------------");
        System.out.println();

        wordsStatistics(documents, "\\S+", false, null, false, false, false);

        System.out.println();
        System.out.println("------------LOWERCASE---------------");
        System.out.println();
        wordsStatistics(documents, "\\S+", false, null, false, false, true);

        System.out.println();
        System.out.println("----------STEM Light------------------");
        System.out.println();
        stemmer = new CzechStemmerLight();
        wordsStatistics(documents, "\\S+", true, null, false, false, true);

        System.out.println();
        System.out.println("----------STEM Agressive------------------");
        System.out.println();

        wordsStatistics(documents, "\\S+", true, null, false, false, true);

        System.out.println();
        System.out.println("-----------ACCENTS---------------");
        System.out.println();

        wordsStatistics(documents, "\\S+", true, null, false, true, true);


        System.out.println();
        System.out.println("-----------REGEX---------------");
        System.out.println();
        wordsStatistics(documents, AdvancedTokenizer.defaultRegex, true, null, false, true, true);

        System.out.println();
        System.out.println(stemmer.stem("vize"));
        System.out.println(stemmer.stem("vizionar"));
        System.out.println(stemmer.stem("vizionář"));
        System.out.println(stemmer.stem("vidím"));
        System.out.println(stemmer.stem("vidíš"));
        System.out.println(stemmer.stem("vidim"));
        System.out.println(stemmer.stem("vidis"));

        System.out.println(stemmer.stem("nejneobhospodařovávatelnějšími"));
        stemmer = new CzechStemmerLight();
        System.out.println(stemmer.stem("nejneobhospodařovávatelnějšími"));
    }



    public static void wordsStatistics(List<String> lines, String tokenizeRegex, boolean stemm, Set<String> stopwords, boolean removeAccentsBeforeStemming, boolean removeAccentsAfterStemming, boolean toLowercase) {
        Set<String> words = new HashSet<String>();
        long numberOfWords = 0;
        long numberOfChars = 0;
        long numberOfDocuments = 0;
        for (String line : lines) {
            numberOfDocuments++;

            if (toLowercase) {
                line = line.toLowerCase();
            }
            if (removeAccentsBeforeStemming) {
                line = AdvancedTokenizer.removeAccents(line);
            }
            for (String token : tokenize(line, tokenizeRegex)) {
//            for (String token : BasicTokenizer.tokenize(line, BasicTokenizer.defaultRegex)) {
                if (stemm) {
                    token = stemmer.stem(token);
                }
                if (removeAccentsAfterStemming) {
                    token = AdvancedTokenizer.removeAccents(token);
                }
                numberOfWords++;
                numberOfChars += token.length();
                words.add(token);
            }
        }
        System.out.println("numberOfWords: " + numberOfWords);
        System.out.println("numberOfUniqueWords: " + words.size());
        System.out.println("numberOfDocuments: " + numberOfDocuments);
        System.out.println("average document char length: " + numberOfChars / (0.0 + numberOfDocuments));
        System.out.println("average word char length: " + numberOfChars / (0.0 + numberOfWords));


        Object[] a = words.toArray();
        Arrays.sort(a);
        System.out.println(Arrays.toString(a));
    }

    private static String[] split(String line, String regex) {
        return line.split(regex);
    }

    private static String[] tokenize(String line, String regex) {
        return AdvancedTokenizer.tokenize(line, regex);
    }
}
