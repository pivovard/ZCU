<?php
$page = @$_REQUEST["page"]."";
if($page == ""){
    $page = "index";
}

include_once("inc/settings.php");
include_once("inc/functions.php");
include_once("inc/db_pdo.class.php");
include_once("inc/db_login.class.php");
include_once("inc/db_articles.class.php");
include_once("inc/login_go.php");
?>

<!DOCTYPE html>

<html>
    <head>
        <?php include "nav/head.php";?>
    </head>
    <body>
        
        <?php
        
        include "nav/header.php";
        include "nav/navbar.php";
        
        if(in_array($page, array("index", "about", "contact", "login", "register", "articles")))
        {
            include "cont/".$page.".inc.php";
        }
        else if(in_array($page, array("admin", "author", "reviewer")))
        {
            $user_right = $_SESSION["conference_system"]["right"];
            if(isLogged() && $page == $user_right){
                include "cont/".$page.".inc.php";
            }
            else{
                header("Location:index.php?page=login");
            }
        }
        else {
            echo "<h1>404: Page not found!</h1>";
        }
        
        include "nav/footer.php";
        ?>
        
    </body>
</html>