<?php 

class Databaze{
    private $db; // objekt databaze
    
    public function __construct($host, $dbname, $usr, $pas){
        $this->db = new PDO("mysql:host=$host;dbname=$dbname",'root','');
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
    
    public function existujeUzivatel($log, $pas){
        // ziskat vysledek
        $vystup = $this->provedDotaz("SELECT jmeno FROM nyklm_uzivatele WHERE login='$log' AND heslo='$pas'");
        // získat po řádcích            
        /*while($row = $vystup->fetch(PDO::FETCH_ASSOC)){
            $pole[] = $row['login'].'<br>';
        }*/
        // získat všechny řádky do pole
        $pole = $vystup->fetchAll();
        return isset($pole[0]) ? $pole[0]["jmeno"] : null;
    }
    
}


?>