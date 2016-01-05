#include <stdio.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/un.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>

#define PORT = 10000;			//Cislo portu
#define BUFF_SIZE = 1024;		//Velikost bufferu
#define MAX = 10;				//Maximalni pocet clientu

int main(int argc, char *argv[])
{

	int server_socket;			//Socket serveru
	int client_sock[MAX];		//Socket clienta
	struct sockaddr_in localAddr
	struct sockaddr_in remoteAddr
	socklen_t addrlen;            // Velikost adresy vzdáleného pocítace
	
	int count = -1;              // Pocet pripojení
	int playing;				//id hrajiciho hrace
	int value;					//navratova hodnota
	int size;                  	// Pocet prijatých a odeslaných bytu
	
	char buffer[BUFF_SIZE];         // Prijímací buffer
   

   // Vytvoríme soket
   if ((server_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP)) == -1)
   {
     printf("Nepodarilo se vytvorit socket");
     return -1;
   }
   
   //sockaddr_in
   //Alokovani pameti
   memset(&localAddr, 0, sizeof(struct sockaddr_in));
   // 1) Rodina protokolu
   localAddr.sin_family = AF_INET;
   // 2) Císlo portu
   localAddr.sin_port = htons(PORT);
   // 3) Nastavení IP adresy lokální sítové karty, pres kterou je možno se pripojit - odkudkoliv. 
   localAddr.sin_addr.s_addr = INADDR_ANY;
   
   // bind - priradíme soketu jméno
   if (bind(server_socket, (sockaddr *)&localAddr, sizeof(localAddr)) == 0)
   {
     printf("Bind OK");
   }
   else
   {
   		printf("Bind ERROR");
   		return -1
   }
   
   // Vytvoríme frontu požadavku na spojení. 
   // Vytvoríme frontu maximální velikosti 10 požadavku.
   if (listen(server_socket, 10) == 0)
   {
     printf("Listen OK");
   }
   else
   {
   		printf("Listen ERROR");
   		return -1
   }
   
   while(true)
   {
   		// Poznacím si velikost struktury remoteAddr.
     	// Predám to funkci accept. 
     	addrlen = sizeof(remoteAddr);
   		// Vyberu z fronty požadavek na spojení.
     	// "client" je nový soket spojující klienta se serverem.
   		client_sock[count] = accept(server_socket, (sockaddr*)&remoteAddr, &addrlen);
   		
   		if (client_sock == -1)
     	{
       		printf("Problem s prijetim pripojeni.");
       		continue;
     	}
     	
     	count ++;
     	
     	printf("connection from %s:%i\n", inet_ntoa(incoming_addr.sin_addr), ntohs(incoming_addr.sin_port));
   		
   		size = sendmsg(client_socket[count], "Navazano spojeni. Pocet hracu: " + count, 0);
   		//size = send(client_socket[count], "Navazano spojeni. Pocet hracu: " + count, len, 0);
		   
		test(size);   		
   }
   
   //zavreni spojeni klientu
   for(int i = 0; i < count; i++)
   {
   	close(client_socket[count]);
   }
   //zavreni spojeni serveru
   close(server_socket);
   return 0;
}

void game()
{
	//cekani na pripojeni alespon dvou hracu
	while(count < 1)
	{
		wait();
	}
	
	playing = 0;
	//hraje se dokud hraji alespon dva hraci
	while(count > 0)
	{
		sendMessage("Zprava");
		
		size = recv(client_socket[playing], buffer, BUFF_SIZE - 1, 0);
		test(size);
	
		char msg[] = parse(buff);
		
		switch(msg[0])
		{
			
		}
	}
}

void sendMessage(char msg[])
{
	//
	//
	// sestaveni zpravy
	//
	//
	size = sendmsg(client_socket[count], msg, 0);
	test(size);
}

void parse(char msg[])
{
	//rozparsovani prijate zpravy
}

/*Test uspesneho prenosu dat*/
int test(int size)
{
	if(size == -1)
	{
		printf("Nebyla prenesena zadna data!");
		return -1;
	}
	return 0;
}


