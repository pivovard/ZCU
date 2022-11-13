import cz.zcu.fav.kiv.jsim.*;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by pivov on 19-Jan-19.
 */
public class VSS {

    private static int steps = 10000;

    static double lambda = 1;

    static double mu0 = 3.0;
    static double mu1 = 2.3;
    static double mu2 = 1.8;
    static double mu3 = 2;
    static double mu4 = 1.5;

    private static double VAR1 = 0.05;
    private static double VAR2 = 0.2;
    private static double VAR3 = 0.7;

    public static void main(String[] args) {

        switch (args.length) {
            case 0:     //default
                RunSim(0);
                RunSim(VAR1);
                RunSim(VAR2);
                RunSim(VAR3);
                break;
            case 1:
                steps = Integer.parseInt(args[0]);
                RunSim(0);
                RunSim(VAR1);
                RunSim(VAR2);
                RunSim(VAR3);
                break;
            case 2:
                steps = Integer.parseInt(args[0]);
                if(args[1].equalsIgnoreCase("exp")) RunSim(0);
                if(args[1].equalsIgnoreCase("gauss")){
                    RunSim(VAR1);
                    RunSim(VAR2);
                    RunSim(VAR3);
                }
                break;

            default:
                System.out.println("Wrong arguments! Run as: VSS (steps count) (EXP / GAUSS)");
                break;
        }
    }


    public static void RunSim(double var){
    	
        JSimSimulation simulation = null;
        Queue queue0 = null, queue1 = null, queue2 = null, queue3 = null, queue4 = null;
        Server server0 = null, server1 = null, server2 = null, server3 = null, server4 = null;
        Generator generator = null;
        List<Queue> queues = new ArrayList<Queue>();

        try{
            System.out.println("Initializing.");
            if (var == 0) {
                System.out.println("Exponencial distribution");
                
                System.out.println("Computed values:");
                Compute.compute();
            } else {
                System.out.println("Gaussian distribution: " + var);
            }

            simulation = new JSimSimulation("Simulation");

            queue0 = new Queue("Queue 0", simulation, null);
            queue1 = new Queue("Queue 1", simulation, null);
            queue2 = new Queue("Queue 2", simulation, null);
            queue3 = new Queue("Queue 3", simulation, null);
            queue4 = new Queue("Queue 4", simulation, null);

            queues.add(queue0);
            queues.add(queue1);
            queues.add(queue2);
            queues.add(queue3);
            queues.add(queue4);

            server0 = new ServerReception("Server 0", simulation, mu0, var, queue0, queues);
            server1 = new ServerMedic("Server 1", simulation, mu1, var, queue1, queues);
            server2 = new ServerMedic("Server 2", simulation, mu2, var, queue2, queues);
            server3 = new ServerMedic("Server 3", simulation, mu3, var, queue3, queues);
            server4 = new ServerRTG("Server 4", simulation, mu4, var, queue4, queues);

            queue0.setServer(server0);
            queue1.setServer(server1);
            queue2.setServer(server2);
            queue3.setServer(server3);
            queue4.setServer(server4);

            generator = new Generator("Generator", simulation, lambda, queue0, var);

            simulation.message("Running the simulation.");

            generator.activate(0.0);

            int i = 0;
            while ((i < steps) && (simulation.step() == true)) {
                i++;
            }

            simulation.message("Simulation interrupted at time " + simulation.getCurrentTime());

            simulation.message("Generated " + generator.counter);

            double Ts0 = server0.getTransTs()/server0.getCounter();
            double Ts1 = server1.getTransTs()/server1.getCounter();
            double Ts2 = server2.getTransTs()/server2.getCounter();
            double Ts3 = server3.getTransTs()/server3.getCounter();
            double Ts4 = server4.getTransTs()/server4.getCounter();

            simulation.message(" ");
            simulation.message("Ts1 "+Ts0);
			simulation.message("Ts2 "+Ts1);
			simulation.message("Ts3 "+Ts2);
			simulation.message("Ts4 "+Ts3);
			simulation.message("Ts5 "+Ts4);

            simulation.message(" ");
            simulation.message("Tq1 "+(Ts0+queue0.getTw()));
            simulation.message("Tq2 "+(Ts1+queue1.getTw()));
            simulation.message("Tq3 "+(Ts2+queue2.getTw()));
            simulation.message("Tq4 "+(Ts3+queue3.getTw()));
            simulation.message("Tq5 "+(Ts4+queue4.getTw()));

            double ro0 = Ts0 / (queue0.getTransTa()/queue0.getCounter());
            double ro1 = Ts1 / (queue1.getTransTa()/queue1.getCounter());
            double ro2 = Ts2 / (queue2.getTransTa()/queue2.getCounter());
            double ro3 = Ts3 / (queue3.getTransTa()/queue3.getCounter());
            double ro4 = Ts4 / (queue4.getTransTa()/queue4.getCounter());

            simulation.message(" ");
            simulation.message("ro1 "+(ro0));
            simulation.message("ro2 "+(ro1));
            simulation.message("ro3 "+(ro2));
            simulation.message("ro4 "+(ro3));
            simulation.message("ro5 "+(ro4));

            double lq0 = ro0+queue0.getLw();
            double lq1 = ro1+queue1.getLw();
            double lq2 = ro2+queue2.getLw();
            double lq3 = ro3+queue3.getLw();
            double lq4 = ro4+queue4.getLw();

            simulation.message(" ");
            simulation.message("Lq1 "+lq0);
            simulation.message("Lq2 "+lq1);
            simulation.message("Lq3 "+lq2);
            simulation.message("Lq4 "+lq3);
            simulation.message("Lq5 "+lq4);

            simulation.message(" ");
            simulation.message("Lw1 "+queue0.getLw());
            simulation.message("Lw2 "+queue1.getLw());
            simulation.message("Lw3 "+queue2.getLw());
            simulation.message("Lw4 "+queue3.getLw());
            simulation.message("Lw5 "+queue4.getLw());

            double Lq = lq0+lq1+lq2+lq3+lq4;
            
            simulation.message(" ");
            simulation.message("Lq "+ Lq);

            simulation.message(" ");
            simulation.message("Tq " + Lq/lambda);


        } catch (JSimInvalidParametersException e) {
            e.printStackTrace();
        } catch (JSimTooManyProcessesException e) {
            e.printStackTrace();
        } catch (JSimTooManyHeadsException e) {
            e.printStackTrace();
        } catch (JSimSimulationAlreadyTerminatedException e) {
            e.printStackTrace();
        } catch (JSimSecurityException e) {
            e.printStackTrace();
        } catch (JSimMethodNotSupportedException e) {
            e.printStackTrace();
        }
        finally
        {
            simulation.shutdown();
            System.out.println("End of simulation.");
            System.out.println("");
        }
    }


}
