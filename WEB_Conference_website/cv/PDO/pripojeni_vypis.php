<?php
    session_start();

    include_once("db_pdo.class.php");
    include_once("predmety.class.php");
    include_once("settings.inc.php");

    function printr($val)
    {
        echo "<hr><pre>";
        print_r($val);
        echo "</pre><hr>";
    }

    
    $key_my_user = "predmety_user";
    if(isset($_SESSION[$key_my_user]))
    {
        
    }
    else $_SESSION[$key_my_user] = array();

    $prihlasen = false;
    if(isset($_SESSION[$key_my_user]["login"]))
    {
        $prihlasen = true;
    }

    //$_SESSION[$key_my_user] = array();

    printr($_POST);
    $action = @$_POST["action"]."";
    $user = @$_POST["user"];
    if($action == "login_go")
    {
        printr($user);
        
        if(trim($user["login"]) == "admin" && $user["pass"] == "admin")
        {
            $_SESSION[$key_my_user]["login"] = $user["login"];
            $prihlasen = true;
        }
    }
        
    if($prihlasen)
    {
        echo "Prihlaseny";
    }
    else
    {
        echo "Neprihlaseny";
        
        echo "<form method=\"post\">
            <input type='hidden' name='action' value='login_go'/>
            Login: <input type='text' name ='user[login]'>
            Pass: <input type='text' name ='user[pass]'>
            <input type='submit' value='Login'>";
    }
    

    $predmety = new predmety();
    $predmety->Connect();

    $seznam_predmetu=$predmety->LoadAll();
    printr($seznam_predmetu);


    $seznam_predmetu=$predmety->LoadOne();
    if($seznam_predmetu != null)
    {
        foreach ($seznam_predmetu as $predmet){
            echo "Zkratka: $predmet[zkratka], nazev: $predmet[nazev]<br>";
        }
    }
?>