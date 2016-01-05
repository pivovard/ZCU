// funkce jQuery pri nacteni stranky
$(document).ready( function(){
  // obarvim radky tabulek
  $("#kos tr:odd").css("background-color", "lightblue");
  $("#kos tr:even").css("background-color", "lightgreen");
  $("#produkty tr:odd").css("background-color", "lightblue");
  $("#produkty tr:even").css("background-color", "lightgreen");
  
  // skryti vsech detailu produktu  
  $("#produkty .popis").hide();
  // zobrazeni detailu u prvniho produktu
  $("#produkty .popis:first").show();
  
  // při kliku na řádek produktu skryje detail
  $("#produkty tr").click(function(){
      $("#produkty tr").not(this).find(".popis").slideUp(1500); // krome aktualniho
      $(this).find(".popis").slideDown(1500);
  });
  
  // spocita polozky nakupniho kosiku
  $("#koupeno").text($("#kos tr").size());
  
  // tlacitko pro objednani obsahu kosiku
  $('#tl_objednani').click(function(){
      // ziskani obsahu kosiku
      var prom = "";
      $("#kos tr").find("form").each(function( index, element ){
          prom += (index+1)+": "+$(element).text().trim()+"\n";
      });
      // potvrzeni objednavky
      if(confirm("Vážně chcete objednat toto zboží:\n\n"+prom+"\n")){      
          //$('#ajax').load("objednavka.php", { uzivatel: $('#uzivatel').text()});
          // AJAXem volam stranku na serveru
          $('#ajax').load("objednavka.php",                 // URL stranky
                      { uzivatel: $('#uzivatel').text() },  // data ve formatu JSON
                      function(response, status, request) { // funkce pro zpracovani vysledku
                        if(status == "success"){            // v poradku
                            $("#cely_kos").hide();
                            //alert($(response).text()); 
                        }
                        if(status == "error"){              // error
                            alert("Error: " + request.status + ": " + request.statusText);
                        }  
                      });
      } 
  });  
});