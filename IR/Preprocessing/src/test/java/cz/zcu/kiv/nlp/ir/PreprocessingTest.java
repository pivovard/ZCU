package cz.zcu.kiv.nlp.ir;

import org.apache.log4j.BasicConfigurator;
import org.apache.log4j.Level;
import org.apache.log4j.Logger;
import org.junit.BeforeClass;
import org.junit.Test;

import java.util.Arrays;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

import java.io.*;

import static org.junit.Assert.*;

/**
 * User: tigi
 * Date: 6.12.12
 * Time: 9:06
 */
public class PreprocessingTest {

    static Preprocessing preprocessing;
    private static final Logger log = Logger.getLogger(PreprocessingTest.class);

    @BeforeClass
    public static void setUpBeforeClass() {

        BasicConfigurator.configure();
        Logger.getRootLogger().setLevel(Level.INFO);
        createNewInstance();
    }

    private static void createNewInstance() {
//        preprocessing = new BasicPreprocessing(
//                new CzechStemmerLight(), new BasicTokenizer(" "), null, false, true, true
//        );

//        preprocessing = new BasicPreprocessing(
//                new CzechStemmerLight(), new BasicTokenizer("\\s+"), null, false, true, true
//        );

        preprocessing = new BasicPreprocessing(
                new CzechStemmerAgressive(), new AdvancedTokenizer(), StopwordsReader.Read(), false, true, true
        );
    }

    @Test
    public void testContainsKey() throws Exception {
        String text = "Ćauík";
        preprocessing.index(text);
        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);

        log.info(Arrays.toString(wordFrequencies.keySet().toArray()));
        assertTrue(wordFrequencies.containsKey("cau"));
    }

    private void printWordFrequencies(Map<String, Integer> wordFrequencies) {
        for (String key : wordFrequencies.keySet()) {
            log.info(key + ": " + wordFrequencies.get(key));
        }
        log.info("");
        printSortedDictionary(wordFrequencies);
    }

    private void printSortedDictionary(Map<String, Integer> wordFrequencies) {
        Object[] a = wordFrequencies.keySet().toArray();
        Arrays.sort(a);
        System.out.println(Arrays.toString(a));
    }

    @Test
    public void testHTML() throws Exception {
        createNewInstance();
        String text = "<li>";
        preprocessing.index(text);
        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);
        assertTrue(wordFrequencies.containsKey(text));
    }

    @Test
    public void testLink() throws Exception {
        createNewInstance();
        String text = " http://www.csfd.cz/film/261379-deadpool/komentare/?comment=10355101 link";
        preprocessing.index(text);
        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);
        assertFalse(wordFrequencies.containsKey("http"));
        assertTrue(wordFrequencies.containsKey("http://www.csfd.cz/film/261379-deadpool/komentare/?comment=10355101"));
    }

    @Test
    public void testTokenize() throws Exception {
        createNewInstance();
        preprocessing.index("(pěstí).");
        preprocessing.index("1280x800");
        preprocessing.index("pr*sata");
        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);
        assertTrue(wordFrequencies.containsKey(preprocessing.getProcessedForm("pěstí")));
        assertTrue(wordFrequencies.containsKey(preprocessing.getProcessedForm("1280x800")));
        assertTrue(wordFrequencies.containsKey(preprocessing.getProcessedForm("pr*sata")));
    }

    @Test
    public void testStopWords() throws Exception {
        createNewInstance();
        preprocessing.index("Tímto textem britský The Guardian Jana Čulíka nepotěšil");
        preprocessing.index("Jestliže nepřijde, měl by se omluvit.");
        preprocessing.index("Šedesát procent naší elektřiny vyrábíme z dováženého plynu, přičemž průměr EU je okolo 40 %.");
        preprocessing.index("Tento seznam může být revidován po dvou letech, přičemž samotné programy mohou být předloženy jednou za rok.");
        preprocessing.index("Tímto poznáním překročuje ženské hnutí hranice feminismu a stává se hnutím pokrokovým, demokratickým. Doznávám, že jsem byl značně překvapen tímto novým důkazem praktické stránky soudruhových theorií. Čechové byli stíženi tímtéž osudem po staletích hrdinského odboje proti německému útlaku. Tímto způsobem také, když opět chcete začít pracovat, aktivní spořič obrazovky ukončujete. Tím jste určili, že program vyhledá všechny soubory, jejichž název tímto slovem začíná. Patnáct procent s tímto názorem nesouhlasilo a pětina mužů nevěděla, jak odpovědět. Celou tu pohádku o třech Garridebech si patrně vymyslel právě za tímto účelem. Vložíte znaménko rovná se každý vzorec musí tímto znaménkem začínat. Zpředu kráčel Stivín, za nim jel rytíř mladý a za tímto klusal starec. Před tímto gentlemanem můžete říci vše, co byste hodlal svěřit mně. Všeobecně dlužno uznati, že tímto nejvýhodněším místem je škola. Chtěl bych Vás proto tímto dopisem poprosit o odpověď na tyto otázky. Vím to a ráda bych, abyste i vy začal svou práci s tímto vědomím. Tehdejší prezident Charles King musí pod tímto tlakem odstoupit. Pocítila jsem ostré bodnutí žárlivosti nad tímto cizincem. Páže, pohledem tímto překvapen, po celém těle se třásl. Tímto emailem bych se chtěla informovat o příznacích AIDS. Hlava mi téměř vybuchla snahou uvažovat tímto způsobem. Nechala jsem tímto temným zjištěním naplnit své oči. Čtvrtina respondentů naopak s tímto názorem souhlasila. Rance zdál se býti poněkud pozloben tímto odbočením. Tímto strašlivým způsobem jsem nabyl svoje dědictví. Věčná škoda, že tímto uměním plýtvala na Dolpha. Nepřipouštěl jsem si tímto směrem jedinou myšlenku. Dodavatel tímto informuje odběratele v souladu s zák. Víš, že mi nikdo neposlal květiny tímto způsobem. Ne, rozhodně se nesnese s tímto morbidním panákem. Tímto večerem se počalo podivínství kapitána J. Mohla být rána zasazena tímto předmětem? Mohla. Jasnovidec byl zřejmě polichocen tímto uznáním. ");
        preprocessing.index("Ačkoli se celý rok učil, známky na vysvědčení má podprůměrné.");
        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);
        assertFalse(wordFrequencies.containsKey(preprocessing.getProcessedForm("tímto")));
        assertFalse(wordFrequencies.containsKey(preprocessing.getProcessedForm("ačkoli")));
        assertFalse(wordFrequencies.containsKey(preprocessing.getProcessedForm("jestliže")));
        assertFalse(wordFrequencies.containsKey(preprocessing.getProcessedForm("přičemž")));
    }

    @Test
    public void testDate() throws Exception {
        createNewInstance();
        String date = "11.2. 2015";
        preprocessing.index(date);
        date = "15.5.2010";
        preprocessing.index(date);
        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);
        assertTrue(wordFrequencies.containsKey(preprocessing.getProcessedForm("11.2.")));
        assertTrue(wordFrequencies.containsKey(preprocessing.getProcessedForm("15.5.2010")));
    }

    @Test
    public void testDiacritics() throws Exception {
        createNewInstance();
        String text = "ćau";
        preprocessing.index(text);
        preprocessing.index("cau");
        preprocessing.index("caú");
        preprocessing.index("cáu");
        preprocessing.index("čau");
        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);
        text = preprocessing.getProcessedForm(text);
        assertEquals(5, wordFrequencies.get(text).intValue());
    }

    @Test
    public void testLowercase() throws Exception {
        createNewInstance();
        preprocessing.index("BOMB");
        preprocessing.index("Bomba");
        preprocessing.index("bomba");
        preprocessing.index("BOMBY");
        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);
        assertEquals(4, wordFrequencies.get(preprocessing.getProcessedForm("bomb")).intValue());
    }


    @Test
    public void testStemming() throws Exception {
        createNewInstance();
        preprocessing.index("smějí");
        preprocessing.index("směju");
        preprocessing.index("směješ");
        preprocessing.index("smějeme");
        preprocessing.index("smějí");
        preprocessing.index("smějou");
        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);
        assertEquals(4, wordFrequencies.get(preprocessing.getProcessedForm("smějí")).intValue());
        assertEquals(1, wordFrequencies.get(preprocessing.getProcessedForm("směješ")).intValue());
        assertEquals(1, wordFrequencies.get(preprocessing.getProcessedForm("smějeme")).intValue());
    }

    @Test
    public void testLowercaseAndStemming() throws Exception {
        createNewInstance();
        preprocessing.index("BOMB");
        preprocessing.index("Bomba");
        preprocessing.index("bomba");
        preprocessing.index("bomby");
        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);
        assertEquals(4, wordFrequencies.get(preprocessing.getProcessedForm("bomb")).intValue());
    }

    @Test
    public void testLong() throws Exception {
        createNewInstance();
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

        for (String document : text.split("\n")) {
            preprocessing.index(document);
        }

        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);
        assertEquals(2, wordFrequencies.get(preprocessing.getProcessedForm("bomb")).intValue());
        assertEquals(2, wordFrequencies.get(preprocessing.getProcessedForm("tržby")).intValue());
        assertEquals(1, wordFrequencies.get(preprocessing.getProcessedForm("z3735f")).intValue());
        assertEquals(4, wordFrequencies.get("</li>").intValue());
    }


    @Test
    public void testYourData() throws Exception {
        createNewInstance();


        //todo zaindexujte vaše data a vytvořte testy na přítomnost nesmyslných slov, která se vyskytla ve vašem slovníku (a opravte)

        final Map<String, Integer> wordFrequencies = preprocessing.getWordFrequencies();
        printWordFrequencies(wordFrequencies);

    }
}
