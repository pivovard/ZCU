<!doctype html>
<html>
    <head>
        <meta charset="utf-8">
        <title>{{nadpis}}</title>
        <style>
            .suda {background-color: darksalmon;}
            .licha {background-color: burlywood;}
        </style>
        <script>
            function najeti(elem){
                inputy = elem.getElementsByTagName("input");
                posledni = inputy[inputy.length-1]; // jen posledni
                posledni.style.backgroundColor = "green";
            }
            function odjeti(elem){
                inputy = elem.getElementsByTagName("input");
                posledni = inputy[inputy.length-1]; // jen posledni
                posledni.style.backgroundColor = "";
                elem.getElementsByClassName
            }
        </script>
    </head>
    <body style="background-color:blue;">
        <div style="background-color:white;width:500px;margin:0 auto;">
            <h1>{{nadpis}}</h1>
            
            {% if uzivatel %}
                    {{uzivatel}}<br>
                    <form action="#" method="post">
                        <fieldset>
                            <legend>Odhlášení uživatele</legend>
                            <input type="submit" name="odhlaseni" value="Odhlášení">
                        </fieldset>
                    </form>
            {% else %}
                    <form action="#" method="post">
                        <fieldset>
                            <legend>Přihlášení uživatele</legend>
                            Login: <input type="text" name="login" maxlength="30"><br>
                            <input type="submit" name="prihlaseni" value="Přihlásit">
                        </fieldset>
                    </form>                                          
            {% endif %}
            
            {% if kos %}
                    <h2>Nákupní košík</h2>
                	<table>
                        {% for k in kos %}
                        <tr onmouseover="najeti(this)" onmouseout="odjeti(this)"><td class="{{ cycle(['suda', 'licha'], loop.index0) }}">
                        <form action="index.php?web=kosik" method="post">
                            {{ k.nazev }}, Cena:{{ k.cena }}, Kusů:{{ k.ks }}
                        <input type="hidden" name="produkt" value="{{k.id}}">
                        <input type="submit" name="odebrat" value="Odebrat">
                        </form>
                        </td></tr>   
                        {% endfor %}
                    </table>            
            {% endif %}        
            
            {% if produkty %}
                    <h2>Produkty v obchodě</h2>
                    <table>
                    {% for k in produkty %}
                        <tr onmouseover="najeti(this)" onmouseout="odjeti(this)"><td class="{{ cycle(['suda', 'licha'], loop.index0) }}">
                        <form action="index.php?web=kosik" method="post">
                            {{k.nazev}}, cena:{{k.cena}}, 
                            <input type="hidden" name="produkt" value="{{k.id}}">
                            <input type="number" min="0" max="10" value="1" name="mnozstvi">
                            <input type="submit" name="pridat" value="Přidat do košíku">
                        </form>
                        </td></tr>
                    {% endfor %}
                    </table>
            {% endif %}
                        
            {{text|raw}}
        </div>
    </body>
</html>
