<?php 

class Databaze{
    private $db; // objekt databaze
    
    public function __construct($host, $dbname, $usr, $pas){
        $this->db = new PDO("mysql:host=$host;dbname=$dbname", $usr, $pas);
    }
    
    private function provedDotaz($dotaz){
        $res = $this->db->query($dotaz);
        if (!$res) {
            $error = $this->db->errorInfo();
            echo $error[2];
            return null;
        } else {
            return $res;            
        }
    }
    
    
    public function vratUzivatele($log, $pas){
        /////// START: klasický dotaz bez ošetření vstupů //////////
        // ziskat vysledek dotazu klasicky
        $vystup = $this->provedDotaz("SELECT * FROM nyklm_uzivatele WHERE login='$log' AND heslo='$pas';");
        /////// KONEC: klasický dotaz /////////////
        /////// START: osetreni SQL Injection ////////////
        // pripravi dotaz
    /*    $vystup = $this->db->prepare("SELECT * FROM nyklm_uzivatele WHERE login=:log AND heslo=:pas;");
        $params = array(':log' => $log, ':pas' => $pas);
        // provede dotaz
        if(!$vystup->execute($params)){ 
            return null; 
        }*/
        /// KONEC: osetreni SQL Injection ///
        // získat po řádcích            
        /*while($row = $vystup->fetch(PDO::FETCH_ASSOC)){
            $pole[] = $row['login'].'<br>';
        }*/
        // získat všechny řádky do pole
        $pole = $vystup->fetchAll();
        print_r($pole);
        // vratim jen prvni radek pole, tj. 1 uzivatele
        return isset($pole) ? $pole : null;
    }
    
    public function registrujUzivatele($jm, $log, $pas, $mail){
        /// klasicky
        //$dotaz = "INSERT INTO nyklm_uzivatele (jmeno, login, heslo, email) VALUES ('$jm', '$log', '$pas', '$mail');";
        //$vystup = $this->provedDotaz($dotaz);
        /// predpripraveny dotaz
        $dotaz = "INSERT INTO nyklm_uzivatele (jmeno, login, heslo, email) VALUES (?,?,?,?);";
        $vystup = $this->db->prepare($dotaz);
        $jm = htmlspecialchars($jm);
        $vystup->execute(array($jm, $log, $pas, $mail));
        return $vystup->fetchAll();
    }
    
}


?>