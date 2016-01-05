<!doctype html>
<?php 
    include("funkce_vysl.php"); // načte soubor s funkcemi
?>
<html>
    <head>
        <meta charset="utf-8">
        <title>Výstup</title>
    </head>
    <body>
        <h1>Výstup formuláře</h1>
    
        <div><strong>Post:</strong> <br>
<?php
    echo vypis($_POST); // vypíše pole do tabulky
?>
        </div>
        
        <div><strong>Get:</strong> <br>
<?php
    echo vypis($_GET);  // vypíše pole do tabulky
?>
        </div>
        
    </body>
</html>