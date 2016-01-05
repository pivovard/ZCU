<?php

/**
*   Protoze nesla databaze, tak lze vyuzit tento soubor.
*   Do souboru nepotrebujete zasahovat.
*/
class Nahradni_DB{
    private $produkty;
    
    public function __construct(){
        session_start();
        // produkty
        $p[] = array( "Televizor", "5530", "");
        $p[] = array( "Lednička", "7800", "");
        $p[] = array( "Sporák", "8000", "");
        $p[] = array( "Pračka", "6500", "");
        $p[] = array( "Žehlička", "700", "");
        $p[] = array( "Vysavač", "3200", "");
        $p[] = array( "Holící strojek", "1200", "");
        $p[] = array( "Zastřihávač", "900", "");
        // vsechny maji jeden popis
        $popis = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aliquam ante. Integer vulputate sem a nibh rutrum consequat. Etiam dui sem, fermentum vitae, sagittis id, malesuada in, quam. Maecenas ipsum velit, consectetuer eu lobortis ut, dictum at dui. Etiam sapien elit, consequat eget, tristique non, venenatis quis, ante. Ut tempus purus at lorem. Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur? Nam sed tellus id magna elementum tincidunt. Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Suspendisse nisl. Duis risus. Morbi scelerisque luctus velit. Fusce nibh. Nullam justo enim, consectetuer nec, ullamcorper ac, vestibulum in, elit.";
        $i=0;
        foreach($p as $pp){
            $nacteno[] = array('id'=>$i++, 'nazev'=>$pp[0], 'cena'=>$pp[1], 'obrazek'=>$pp[2], 'popis'=>$popis);
        }
        $this->produkty = $nacteno;
    }
    
    public function nactiProdukty(){
        return $this->produkty;
    }
    
    public function doKosiku($idProduktu, $pocet, $uzivatel){
        $_SESSION["kosik"][$uzivatel][$idProduktu] = $pocet;
        
    }
    
    public function zKosiku($idProduktu, $uzivatel){
        if(isset($_SESSION["kosik"][$uzivatel][$idProduktu])){
            unset($_SESSION["kosik"][$uzivatel][$idProduktu]);
        }
    }
           
    public function obsahKosiku($uzivatel){
        if(isset($_SESSION["kosik"][$uzivatel])){
            foreach($_SESSION["kosik"][$uzivatel] as $produkt => $pocet){
                //echo "q:".$produkt."-".$pocet."-".$uzivatel;
                $this->produkty[$produkt]["ks"] = $pocet;
                $this->produkty[$produkt]["id"] = $produkt;
                $obsah[] = $this->produkty[$produkt];
            }            
        } 
        if(isset($obsah)){
            return $obsah;
        } else {
            return null;
        }
    }
    
    public function smazKosik($uzivatel){
        if(isset($_SESSION["kosik"][$uzivatel])){
            unset($_SESSION["kosik"][$uzivatel]);
        }
    }
    
    
}


?>