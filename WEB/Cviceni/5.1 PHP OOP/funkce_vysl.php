<?php
/**
*     Načte volby pro SELECT se zvířaty.
*     
*     @return String Volby do SELECT.       
*/
function nactiVolby(){
    $soubor = "zvirata.txt";
    $vystup = ""; // vystupni string
    // existuje a je souborem?
    if(file_exists($soubor) && is_file($soubor)){
        $fr = @fopen($soubor, "r"); // otevři soubor pro čtení
        while(!feof($fr)) { // je konec souboru
            $radka = trim(fgets($fr)); // načti řádek a odstraň bílé znaky
            $vystup .= "<option value='$radka'>$radka</option>"; // vypiš option
        }
        fclose($fr); // zavři soubor
    }
    return $vystup;
}

/**
    Vypíše obsah pole do tabulky a uloží ji do souboru. 
*  @param Array $vstup Vstupni pole.
*  @return HTML pro vytvoreni tabulky.
*/
function vypis($vstup){
    if(count($vstup)==0){// pole je prázdné
        echo "prázdné pole";
        return ""; 
    }
    
    $tab = vytvorTabulku($vstup); // vytvoří z pole tabulku  
    
    ulozDoSouboru($tab); // uloží tabulku do souboru
    if($vstup["pozdrav"]){ // je zvolen pozdrav?
        echo "Pozdrav: ";
        $vstup["pozdrav"](); // zavolá funkci uloženou v proměnné   
    } 

    return $tab;  // vrací tabulku
}

/**
  *  Vypíše obsah pole do tabulky. Používá rekurzi na vložená pole a pokud není u prvku vyplněna hodnota, tak vypíše "nezadáno".
  *  @param Array $vstup Vstupni pole.
  *  @return HTML pro vytvoreni tabulky.
 */  
function vytvorTabulku($vstup){
    $textTabulky = "<table border><tr><td>key</td><td>value</td></tr>";
    foreach($vstup as $key => $value){ // procházím pole
        // programátorský způsob
        $textTabulky.= "<tr><td>".$key."</td><td>".((is_array($value)) ? vytvorTabulku($value) : (((trim($value)=="")?"nezadáno":$value)."</td></tr>")); // vypise nebo rekurze
        
        // polo-amatérský způsob
        /*$textTabulky.= "<tr><td>".$key."</td><td>";
        if(is_array($value)){
            $textTabulky.= vytvorTabulku($value); // rekurze
        } else {
            $textTabulky.= (trim($value)=="") ? "nezadáno":$value; // jen hodnota
        } 
        $textTabulky.= '</td></tr>'; */
            
    }
    $textTabulky.= "</table>";
    return $textTabulky;
} 


/**
*   Uloží výpis do souboru.
*    
*   @param String $text Text pro uložení.
*   @return boolean Výsledek.
*/
function ulozDoSouboru($vstup){
    // kontrola existence adresáře
    $adr = "vystup";
    if(file_exists($adr) && is_file($adr)){ // existuje soubor se jménem adresáře?
        echo "adresář nelze uložit";    
    } elseif(!file_exists($adr)) { // neexistuje adresář ani soubor
        mkdir($adr); // vytvoří adresář
    }
    
    $soubor = $adr."/".date("Y-m-d_H-i").".txt";
    echo "Vytvořen soubor: ".$soubor."<br/>";
    $fw = @fopen($soubor, "w") or die("Soubor nelze otevřít"); // otevři soubor pro zápis
    fwrite($fw, $vstup);
    fclose($fw);
}

function a(){
  echo "ahoj";
}

function c(){
  echo "čau";
}

function n(){
  echo "nazdar";
}

?>