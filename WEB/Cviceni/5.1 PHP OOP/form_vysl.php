<!doctype html>
<?php 
    include("funkce_vysl.php"); // načte soubor s funkcemi
?>
<html>
    <head>
        <meta charset="utf-8">
        <title>Formulář</title>
    </head>
    <body>
        <h1>Formulář</h1>
        
        <form action="vystup_vysl.php" method="post">
            <fieldset>
                <legend>Registrační formulář</legend>

                <fieldset>
                    <legend>Osobní informace</legend>
                    <br>
                    Jméno:
                    <input type="text" name="jmeno"><br>
                    <br>
                    Příjmení:
                    <input type="text" name="prijmeni" ><br>
                    <br>
                    Email:
                    <input type="text" name="email" ><br>
                    <br>
                   
                    <br>
                    
                    Nahrát soubor:<br>
                    <input type="file" name="soubor[]" multiple><br>
                    
                    Oblíbené zvíře (s CTRL zvolte více):<br>
                    <select name="zvire[]" size="5" multiple>
<?php
    echo nactiVolby();   // načte volby do SELECT
?>
                    </select><br>
                    <br>
                    
                    Pozdrav: 
                    <select name="pozdrav">
                      <option value="a">ah..</option>
                      <option value="c">ca..</option>
                      <option value="n">na..</option>
                    </select>
                    
                    
                </fieldset>
                <br>
                <input type="submit" value="Odeslat">                
                <input type="reset" value="Smazat">
            </fieldset>  
            
        </form>

        
    </body>
</html>