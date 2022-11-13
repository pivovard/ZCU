import cz.zcu.fav.kiv.jsim.*;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

/**
 * Created by pivov on 20-Jan-19.
 */
public abstract class Server extends JSimProcess {

    protected double mu;
    protected double probability;
    protected double probabilityMed;
    protected Queue queue;
    protected List<Queue> output = new ArrayList<Queue>();

    protected int counter;
    protected double transTq;
    protected double transTs;

    protected double var;

    public Server(String name, JSimSimulation jSimSimulation) throws JSimSimulationAlreadyTerminatedException, JSimInvalidParametersException, JSimTooManyProcessesException {
        super(name, jSimSimulation);
    }

    public Server(String name, JSimSimulation simulation, double mu, double var, Queue queue, List<Queue> output) throws JSimSimulationAlreadyTerminatedException, JSimInvalidParametersException, JSimTooManyProcessesException {
        super(name, simulation);

        this.mu = mu;
        this.queue = queue;
        this.output = output;
        this.var = var;

        counter = 0;
        transTq = 0.0;
    }

    protected double getHoldTime(){
        if(var > 0) {
            Random r = new Random();
            double rnd;
            double sum = 0;

            do {
                for (int j = 0; j < 12; j++) {
                    sum = r.nextFloat();
                }
                rnd =  var/mu*(sum -0.6) + 1/mu;
            } while(rnd < 0);

            return rnd;
        } else {
            return JSimSystem.negExp(mu);
        }
    }

    public int getCounter()
    {
        return counter;
    }

    public double getTransTq()
    {
        return transTq;
    }

    public double getTransTs()
    {
        return transTs;
    }

}
