package pivovar.meditabmobile;

import android.content.Intent;
import android.content.res.Resources;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.ListView;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.util.LinkedList;
import java.util.List;

public class OrdinaceActivity extends AppCompatActivity {

    List<Ordinace> ordinace;
    String[] ord = new String[24];
    ArrayAdapter<String> adapter;

    String pacientID;
    String lekID;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_medikace);

        Intent i = getIntent();
        pacientID = (String) i.getExtras().get("ID");
        lekID = (String) i.getExtras().get("LEK");

        load();

        adapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, android.R.id.text1, ord);

        final ListView lw = (ListView) findViewById(R.id.listView3);
        lw.setAdapter(adapter);

        lw.setOnItemClickListener(new AdapterView.OnItemClickListener() {

            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {

                if(ordinace.get(position).stav.equals("O")) {
                    ordinace.get(position).stav = "P";
                    ord[position] = ordinace.get(position).time + " " + ordinace.get(position).davka + " " + ordinace.get(position).stav;
                    adapter.notifyDataSetChanged();
                }
            }
        });
    }

    @Override
    public void onPause(){
        super.onPause();

            /*Resources raw = getResources();
            OutputStream out;
            OutputStreamWriter outw;
            BufferedWriter wr;
            String line;

            try {
                out = raw.openRawResource(R.raw.bilance);
                outw = new OutputStreamWriter(out);
                wr = new BufferedReader(outw);

                String lineOut = pacientID + ";" + lekID;

                for(int i = 0; i < 24; i++){
                    lineOut += ";" + ordinace.get(i).time + "," + ordinace.get(i).davka + "," + ordinace.get(i).stav + ",";
                }

                wr.append(line);
            }
            catch (Exception e){
            }*/
    }

    void load(){
        ordinace = new LinkedList<Ordinace>();

        Resources raw = getResources();
        InputStream in;
        InputStreamReader inr;
        BufferedReader br;
        String line;
        String[] lineFinal = new String[26];

        try {
            in = raw.openRawResource(R.raw.medikace);
            inr = new InputStreamReader(in);
            br = new BufferedReader(inr);

            line = br.readLine();
            String[] split;


            while (line != null){
                split = line.split(";");
                if(split[0].equals(pacientID) && split[1].equals(lekID)){
                    lineFinal = split;
                }
                line = br.readLine();
            }

            String[] splitFinal;
            for(int i = 2; i < 26; i++){
                splitFinal = lineFinal[i].split(",");
                ordinace.add(new Ordinace(splitFinal[0], splitFinal[1], splitFinal[2]));
                ord[i - 2] = splitFinal[0] + " " + splitFinal[1] + " " + splitFinal[2];
            }
        }
        catch (Exception e){
        }
    }
}
