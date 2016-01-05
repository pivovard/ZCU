<?php

class Prihlaseni{
    
    public static $uzivatel;
    private $db;
    
    public function __construct(){
        // spusti session pro spravu prihlaseni uzivatele
        session_start();
        // importuje funkce pro práci s databází
        include("databaze.class.php");
        $this->db = new Databaze("localhost","db1_vyuka","root","");
    }
    
    public function kontrolaPrihlaseni(){
        return isset($_SESSION["prihlasen"]) ? $_SESSION["prihlasen"] : null;
    }
    
    public function prihlasUzivatele($log, $pas){
        // spravny login a heslo
        $jmeno = $this->db->existujeUzivatel($log, $pas);
        if(isset($jmeno)){
            $_SESSION["prihlasen"]=$jmeno;
            return "ANO";
        } else {
            return "NE";
        }   
    }
    
    public function odhlasUzivatele(){
        $_SESSION["prihlasen"]=null;
        return true;
    }
    
    public function registraceUzivatele(){
        
    }
}

?>