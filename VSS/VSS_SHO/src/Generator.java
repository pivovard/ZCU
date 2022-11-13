import cz.zcu.fav.kiv.jsim.*;

import java.util.Random;

/**
 * Created by pivov on 20-Jan-19.
 */
public class Generator extends JSimProcess {

    private double lambda;
    private Queue queue;
    public int counter;
    private double var;

    public Generator(String name, JSimSimulation jSimSimulation, double l, Queue q,
                     double v) throws JSimSimulationAlreadyTerminatedException, JSimInvalidParametersException, JSimTooManyProcessesException {
        super(name, jSimSimulation);
        lambda = l;
        queue = q;
        counter = 0;
        var = v;
    }

    protected void life()
    {
        try
        {
            while (true)
            {
                JSimLink link = new JSimLink(new Transaction(myParent.getCurrentTime()));

                link.into(queue);
                queue.trans(myParent.getCurrentTime());
                counter++;

                if (queue.getServer().isIdle())
                    queue.getServer().activate(myParent.getCurrentTime());

                double holdTime ;
                if(var > 0) { //gauss
                    Random r = new Random();
                    double rnd;
                    double sum = 0;

                    do {
                        for (int j = 0; j < 12; j++) {
                            sum = r.nextFloat();
                        }
                        rnd =  var/lambda*(sum -0.6) + 1/lambda;
                    } while(rnd < 0);

                    holdTime = rnd;
                } else { //exp
                    holdTime = JSimSystem.negExp(lambda);
                }

                hold(holdTime);
            }
        }
        catch (JSimException e)
        {
            e.printStackTrace();
            e.printComment(System.err);
        }
    }

}
