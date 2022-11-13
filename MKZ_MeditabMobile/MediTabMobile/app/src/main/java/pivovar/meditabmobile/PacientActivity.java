package pivovar.meditabmobile;

import android.content.Intent;
import android.content.res.Resources;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;

import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.ListView;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.LinkedList;
import java.util.List;

public class PacientActivity extends AppCompatActivity {

    String pacientID;

    /**
     * The {@link android.support.v4.view.PagerAdapter} that will provide
     * fragments for each of the sections. We use a
     * {@link FragmentPagerAdapter} derivative, which will keep every
     * loaded fragment in memory. If this becomes too memory intensive, it
     * may be best to switch to a
     * {@link android.support.v4.app.FragmentStatePagerAdapter}.
     */
    private SectionsPagerAdapter mSectionsPagerAdapter;

    /**
     * The {@link ViewPager} that will host the section contents.
     */
    private ViewPager mViewPager;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_detail);

        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        // Create the adapter that will return a fragment for each of the three
        // primary sections of the activity.
        mSectionsPagerAdapter = new SectionsPagerAdapter(getSupportFragmentManager());

        // Set up the ViewPager with the sections adapter.
        mViewPager = (ViewPager) findViewById(R.id.container);
        mViewPager.setAdapter(mSectionsPagerAdapter);

        Intent i = getIntent();
        pacientID = (String) i.getExtras().get("ID");
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_detail, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    /**
     * A placeholder fragment containing a simple view.
     */
    public static class PlaceholderFragment extends Fragment {

        View rootView;
        String pacientID;

        /**
         * The fragment argument representing the section number for this
         * fragment.
         */
        private static final String ARG_SECTION_NUMBER = "section_number";

        public PlaceholderFragment() {
        }

        /**
         * Returns a new instance of this fragment for the given section
         * number.
         */
        public static PlaceholderFragment newInstance(int sectionNumber) {
            PlaceholderFragment fragment = new PlaceholderFragment();
            Bundle args = new Bundle();
            args.putInt(ARG_SECTION_NUMBER, sectionNumber);
            fragment.setArguments(args);
            return fragment;
        }

        @Override
        public View onCreateView(LayoutInflater inflater, ViewGroup container,
                                 Bundle savedInstanceState) {
            //View rootView; //= inflater.inflate(R.layout.fragment_medikace, container, false);
            pacientID = (String) getActivity().getIntent().getExtras().get("ID");

            if(getArguments().getInt(ARG_SECTION_NUMBER) == 1){
                rootView = inflater.inflate(R.layout.fragment_medikace, container, false);
                loadMedikace();
                return rootView;
            }
            else{
                rootView = inflater.inflate(R.layout.fragment_bilance, container, false);
                loadBilance();
                return rootView;
            }

            //TextView textView = (TextView) rootView.findViewById(R.id.section_label);
            //textView.setText(getString(R.string.section_format, getArguments().getInt(ARG_SECTION_NUMBER)));
            //return rootView;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        @Override
        public void onPause(){
            super.onPause();

            final EditText prijem = (EditText) rootView.findViewById(R.id.prijem);
            final EditText vydej = (EditText) rootView.findViewById(R.id.vydej);

            /*Resources raw = getResources();
            OutputStream out;
            OutputStreamWriter outw;
            BufferedWriter wr;
            String line;

            try {
                out = raw.openRawResource(R.raw.bilance);
                outw = new OutputStreamWriter(out);
                wr = new BufferedReader(outw);

                String lineOut = pacientID + ";" + prijem.getText() + ";" + vydej.getText();
                wr.append(line);
            }
            catch (Exception e){
            }*/
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        void loadBilance(){
            final EditText prijem = (EditText) rootView.findViewById(R.id.prijem);
            final EditText vydej = (EditText) rootView.findViewById(R.id.vydej);

            Resources raw = getResources();
            InputStream in;
            InputStreamReader inr;
            BufferedReader br;
            String line;

            try {
                in = raw.openRawResource(R.raw.bilance);
                inr = new InputStreamReader(in);
                br = new BufferedReader(inr);

                line = br.readLine();
                String[] split;

                while (line != null){
                    split = line.split(";");
                    if(split[0].equals(pacientID)){
                        prijem.setText(split[1]);
                        vydej.setText(split[2]);
                    }
                    line = br.readLine();
                }
            }
            catch (Exception e){
            }
        }

        List<String> medikace;
        String[] leky;
        ArrayAdapter<String> adapter;

        void loadMedikace(){
            medikace = new LinkedList<String>();

            Resources raw = getResources();
            InputStream in;
            InputStreamReader inr;
            BufferedReader br;
            String line;

            try {
                in = raw.openRawResource(R.raw.medikace);
                inr = new InputStreamReader(in);
                br = new BufferedReader(inr);

                line = br.readLine();
                String[] split;

                while (line != null){
                    split = line.split(";");
                    if(split[0].equals(pacientID) && !medikace.contains(split[1])){
                        medikace.add(split[1]);
                    }
                    line = br.readLine();
                }
            }
            catch (Exception e){
            }

            int size = medikace.size();
            leky = new String[size];

            for(int i = 0; i < size; i++){
                leky[i] = medikace.get(i);
            }

            adapter = new ArrayAdapter<String>(getActivity(), android.R.layout.simple_list_item_1, android.R.id.text1, leky);

            final ListView lw = (ListView) rootView.findViewById(R.id.listView2);
            lw.setAdapter(adapter);

            lw.setOnItemClickListener(new AdapterView.OnItemClickListener() {

                @Override
                public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                    Intent activity = new Intent(getActivity(), OrdinaceActivity.class);
                    activity.putExtra("ID", pacientID);
                    activity.putExtra("LEK", medikace.get(position));
                    startActivity(activity);
                }
            });
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }



    /**
     * A {@link FragmentPagerAdapter} that returns a fragment corresponding to
     * one of the sections/tabs/pages.
     */
    public class SectionsPagerAdapter extends FragmentPagerAdapter {

        public SectionsPagerAdapter(FragmentManager fm) {
            super(fm);
        }

        @Override
        public Fragment getItem(int position) {
            // getItem is called to instantiate the fragment for the given page.
            // Return a PlaceholderFragment (defined as a static inner class below).
            return PlaceholderFragment.newInstance(position + 1);
        }

        @Override
        public int getCount() {
            // Show 2 total pages.
            return 2;
        }

        @Override
        public CharSequence getPageTitle(int position) {
            switch (position) {
                case 0:
                    return "SECTION 1";
                case 1:
                    return "SECTION 2";
            }
            return null;
        }
    }
}
