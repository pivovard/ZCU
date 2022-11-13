<?php

$page = @$_REQUEST["page"]."";
$subpage = @$_REQUEST["subpage"]."";
if($page == ""){
    $page = "uvod";
}   

$filename = $page.".php";

//$obsah = file_get_contents($filename);
$obsah = phpWrapperFromFile($filename);








// Twig stahnout z githubu - klidne staci zip a dat do slozky twig-master
		// kontrolu provedete dle umisteni souboru Autoloader.php, ktery prikladam pro kontrolu
	
	// nacist twig - kopie z dokumentace
	require_once 'twig-master/lib/Twig/Autoloader.php';
	Twig_Autoloader::register();
	// cesta k adresari se sablonama - od index.php
	$loader = new Twig_Loader_Filesystem('sablony');
	$twig = new Twig_Environment($loader); // takhle je to bez cache
	// nacist danou sablonu z adresare
	$template = $twig->loadTemplate('sablona1.htm');
	
	// render vrati data pro vypis nebo display je vypise
	// v poli jsou data pro vlozeni do sablony
	$template_params = array();
	$template_params["obsah"] = $obsah;
	$template_params["nadpis1"] = "Nadpis 1";
	echo $template->render($template_params);










function phpWrapperFromFile($filename)
	{
		ob_start();
		
		if (file_exists($filename) && !is_dir($filename))
		{
			include($filename);
		}
		
		// nacte to z outputu
		$obsah = ob_get_clean();
		return $obsah;
	}


?>