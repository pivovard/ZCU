package pivovar.meditabmobile;

/**
 * Created by pivov on 07-Jul-16.
 */
public class User {
    public String login;
    private String pass;

    public User(String l, String p){
        this.login = l;
        this.pass = p;
    }

    public boolean logIn(String l, String p){
        if(l.equals(login) && p.equals(pass)) return true;
        return false;
    }
}
