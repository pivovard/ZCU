import cz.zcu.fav.kiv.jsim.*;

/**
 * Created by pivov on 20-Jan-19.
 */
public class Queue extends JSimHead {

    private JSimProcess server;
    private int counter;
    private double transTa;
    private double time;

    public Queue(String name, JSimSimulation jSimSimulation, JSimProcess s) throws JSimInvalidParametersException, JSimTooManyHeadsException {
        super(name, jSimSimulation);
    }

    public Queue(String name, JSimSimulation simulation) throws JSimInvalidParametersException, JSimTooManyHeadsException {
        this(name, simulation, null);
    }

    public JSimProcess getServer() {
        return server;
    }

    public void setServer(JSimProcess server) {
        this.server = server;
    }

    public void trans(double t) {
        transTa += (t - time);
        time = t;
        counter++;
    }

    public int getCounter() {
        return counter;
    }

    public double getTransTa() {
        return transTa;
    }
}
