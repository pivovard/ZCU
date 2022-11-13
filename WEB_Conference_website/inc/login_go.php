<?php
session_start();

$key = "conference_system";

if(!isset($_SESSION[$key]))
{
    $_SESSION[$key] = array();
}


$action = @$_REQUEST["action"]."";
$user = @$_POST["user"];

$db = new db_login();
$db->Connect();
 

if($action == "login")
{
    $result = $db->Login($user);
    
    if(trim($user["login"]) == $result["login"] && $user["pass"] == $result["pass"])
    {
        $_SESSION[$key]["login"] = $user["login"];
        $_SESSION[$key]["ID"] = $result["ID"];
        
        $result = $db->GetRight($result["right"]);
        $_SESSION[$key]["right"] = $result["right"];
        
        header("Location:index.php");
    }
}

if($action == "register")
{
    $result = $db->Register($user);
    
    if(is_numeric($result)){
        $_SESSION[$key]["login"] = $user["login"];
        $_SESSION[$key]["ID"] = $result;
        
        $result = $db->GetRight($user["right"]);
        $_SESSION[$key]["right"] = $result["right"];
        
        header("Location:index.php");
    }
}

if($action == "logout")
{
    session_unset();
    header("Location:index.php");
}


function isLogged()
{
    $key = $GLOBALS["key"];
    
    if(isset($_SESSION[$key]["login"]))
    {
        return true;
    }
    else
    {
        return false;
    }
}



?>

