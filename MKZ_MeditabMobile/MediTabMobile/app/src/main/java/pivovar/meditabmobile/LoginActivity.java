package pivovar.meditabmobile;

import android.content.Intent;
import android.content.res.Resources;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.LinkedList;
import java.util.List;

public class LoginActivity extends AppCompatActivity {

    List<User> users;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        load();

        final Button button = (Button) findViewById(R.id.button);
        final EditText login = (EditText) findViewById(R.id.login);
        final EditText pass = (EditText) findViewById(R.id.pass);

        button.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                boolean success = false;
                for (User u : users) {
                    success = u.logIn(login.getText().toString(), pass.getText().toString());
                    if(success) break;
                }

                Intent activity = new Intent(LoginActivity.this, PacientListActivity.class);
                if(success){
                    startActivity(activity);
                }
                else{
                    Toast.makeText(getApplicationContext(), "Chybne prihlaseni", Toast.LENGTH_LONG).show();
                }
            }
        });
    }

    void load(){
        users = new LinkedList<>();

        Resources raw = getResources();
        InputStream in;
        InputStreamReader inr;
        BufferedReader br;
        String line;

        try {
            in = raw.openRawResource(R.raw.users);
            inr = new InputStreamReader(in);
            br = new BufferedReader(inr);
            //File sdcard = Environment.getExternalStorageDirectory();
            //File file = new File("Download\\users.txt");
            //br = new BufferedReader(new FileReader(file));
            //FileInputStream fis = getApplicationContext().openFileInput("Download/users.txt");
            //InputStreamReader isr = new InputStreamReader(fis);
            //br = new BufferedReader(isr);

            line = br.readLine();
            String[] split;

            while (line != null){
                split = line.split(";");
                users.add(new User(split[0],split[1]));
                line = br.readLine();
            }
        }
        catch (Exception e){
        }
    }
}
