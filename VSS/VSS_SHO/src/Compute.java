
public class Compute {
	
	static double lambda = VSS.lambda;
	
	static double mu1 = VSS.mu0;
	static double mu2 = VSS.mu1;
	static double mu3 = VSS.mu2;
	static double mu4 = VSS.mu3;
	static double mu5 = VSS.mu4;
	
	static double p1 = 0.99;
	static double p2 = 0.333;
	static double p3 = 0.4;
			

	public static void compute() {
		
		double A1 = lambda / p1;
		
		//Am = p2*(p1*A1 + A5)  + p2*(1-2*p3)*3Am
		double Am = p2 * p1 * A1 / (1 - 3 * p2 * p3 - 3 * p2 * (1-2*p3));
		//Am = p2*(p1*A1 + A5)  + p2*(1-2*p3)*Am
		//double Am = p2 * p1 * A1 / (1 - 3 * p2 * p3 - p2 * (1-2*p3));
		
		double A2 = Am, A3 = Am, A4 = Am;
		double A5 = p3 * 3 * Am;
		
		System.out.println("A1 " + A1);
		System.out.println("Am " + Am);
		System.out.println("A5 " + A5);
		System.out.println();
		
		//Støední doba obsluhy
		double Ts1 = 1/mu1;
		double Ts2 = 1/mu2;
		double Ts3 = 1/mu3;
		double Ts4 = 1/mu4;
		double Ts5 = 1/mu5;
		
		System.out.println("Ts1 " + Ts1);
		System.out.println("Ts2 " + Ts2);
		System.out.println("Ts3 " + Ts3);
		System.out.println("Ts4 " + Ts4);
		System.out.println("Ts5 " + Ts5);
		System.out.println();

		
		//Zatížení uzlù
		double ro1 = A1 * Ts1;
		double ro2 = A2 * Ts2;
		double ro3 = A3 * Ts3;
		double ro4 = A4 * Ts4;
		double ro5 = A5 * Ts5;
		
		System.out.println("ro1 " + ro1);
		System.out.println("ro2 " + ro2);
		System.out.println("ro3 " + ro3);
		System.out.println("ro4 " + ro4);
		System.out.println("ro5 " + ro5);
		System.out.println();
				
		//Støední poèet požadavkù v uzlech
		double Lq1 = ro1 / (1-ro1);
		double Lq2 = ro2 / (1-ro2);
		double Lq3 = ro3 / (1-ro3);
		double Lq4 = ro4 / (1-ro4);
		double Lq5 = ro5 / (1-ro5);
		
		System.out.println("Lq1 " + Lq1);
		System.out.println("Lq2 " + Lq2);
		System.out.println("Lq3 " + Lq3);
		System.out.println("Lq4 " + Lq4);
		System.out.println("Lq5 " + Lq5);
		System.out.println();
				
		//Støední délka fronty
		double Lw1 = Lq1 - ro1;
		double Lw2 = Lq2 - ro2;
		double Lw3 = Lq3 - ro3;
		double Lw4 = Lq4 - ro4;
		double Lw5 = Lq5 - ro5;
		
		System.out.println("Lw1 " + Lw1);
		System.out.println("Lw2 " + Lw2);
		System.out.println("Lw3 " + Lw2);
		System.out.println("Lw4 " + Lw4);
		System.out.println("Lw5 " + Lw5);
		System.out.println();
				
		//Prùmìrná doba prùchodu požadavku uzly
		double Tq1 = Lq1 / A1;
		double Tq2 = Lq2 / A2;
		double Tq3 = Lq3 / A3;
		double Tq4 = Lq4 / A4;
		double Tq5 = Lq5 / A5;
		
		System.out.println("Tq1 " + Tq1);
		System.out.println("Tq2 " + Tq2);
		System.out.println("Tq3 " + Tq3);
		System.out.println("Tq4 " + Tq4);
		System.out.println("Tq5 " + Tq5);
		System.out.println();
				
		//Prùmìrná doba prùchodu požadavku frontou
		double Tw1 = Lw1 / A1;
		double Tw2 = Lw2 / A2;
		double Tw3 = Lw3 / A3;
		double Tw4 = Lw4 / A4;
		double Tw5 = Lw5 / A5;
		
		System.out.println("Tw1 " + Tw1);
		System.out.println("Tw2 " + Tw2);
		System.out.println("Tw3 " + Tw3);
		System.out.println("Tw4 " + Tw4);
		System.out.println("Tw5 " + Tw5);
		System.out.println();
				
		//Støední poèet požadavkù v síti
		double Lq = Lq1 + Lq2 + Lq3 +Lq4 + Lq5;
		
		System.out.println("Lq " + Lq);
		System.out.println();
				
		//Støední doba prùchodu požadavku sítí
		double Tq = Lq / lambda;

		System.out.println("Tq " + Tq);
		System.out.println();
	}
	
	
	public static void main(String[] args) { 
		compute();
	}
}
