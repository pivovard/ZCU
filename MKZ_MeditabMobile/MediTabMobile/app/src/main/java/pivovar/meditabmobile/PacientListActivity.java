package pivovar.meditabmobile;

import android.content.Intent;
import android.content.res.Resources;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.LinkedList;
import java.util.List;

public class PacientListActivity extends AppCompatActivity {

    List<Pacient> pacients;
    String[] name;
    ArrayAdapter<String> adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_pacient);

        load();
        transfer();

        adapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_2, android.R.id.text1, name);

        final ListView lw = (ListView) findViewById(R.id.listView);
        lw.setAdapter(adapter);

        lw.setOnItemClickListener(new AdapterView.OnItemClickListener() {

            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {

                // ListView Clicked item index
                int itemPosition     = position;
                /*
                // ListView Clicked item value
                String  itemValue    = (String) lw.getItemAtPosition(position);
                // Show Alert
                Toast.makeText(getApplicationContext(),
                        "Position :"+itemPosition+"  ListItem : " +itemValue , Toast.LENGTH_LONG)
                        .show();
                */
                Intent activity = new Intent(PacientListActivity.this, PacientActivity.class);
                activity.putExtra("ID", pacients.get(position).RC);
                startActivity(activity);
            }
        });
    }

    void load(){
        pacients = new LinkedList<Pacient>();

        Resources raw = getResources();
        InputStream in;
        InputStreamReader inr;
        BufferedReader br;
        String line;

        try {
            in = raw.openRawResource(R.raw.pacients);
            inr = new InputStreamReader(in);
            br = new BufferedReader(inr);

            line = br.readLine();
            String[] split;

            while (line != null){
                split = line.split(";");
                pacients.add(new Pacient(split[0],split[1], split[2]));
                line = br.readLine();
            }
        }
        catch (Exception e){
        }
    }

    void transfer(){
        int size = pacients.size();
        name = new String[size];

        for(int i = 0; i < size; i++){
            name[i] = pacients.get(i).prijmeni + " " + pacients.get(i).jmeno + ", " + pacients.get(i).RC;
        }
    }
}
