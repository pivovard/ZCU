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


if(in_array($page, array("index", "about", "contact", "login", "register", "articles")))
        {
            $filename =  "cont/".$page.".inc.php";
        }
        else if(in_array($page, array("admin", "author", "reviewer")))
        {
            $user_right = $_SESSION["conference_system"]["right"];
            if(isLogged() && $page == $user_right){
                $filename =  "cont/".$page.".inc.php";
            }
            else{
                header("Location:index.php?page=login");
            }
        }
        else {
            echo "<h1>404: Page not found!</h1>";
        }

$nav = phpWrapperFromFile("nav/navbar.php");
$sidenav = phpWrapperFromFile("nav/sidenav.php");
$obsah = phpWrapperFromFile($filename);
        

	// nacist twig - kopie z dokumentace
	require_once 'twig-master/lib/Twig/Autoloader.php';
	Twig_Autoloader::register();
	// cesta k adresari se sablonama - od index.php
	$loader = new Twig_Loader_Filesystem('sablony');
	$twig = new Twig_Environment($loader); // takhle je to bez cache
	// nacist danou sablonu z adresare
	$template = $twig->loadTemplate('sablona.htm');

	// render vrati data pro vypis nebo display je vypise
	// v poli jsou data pro vlozeni do sablony
	$template_params = array();
	$template_params["nav"] = $nav;
	$template_params["obsah"] = $obsah;
    $template_params["sidenav"] = $sidenav;
	echo $template->render($template_params);




?>