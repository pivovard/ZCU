#include <stdio.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/un.h>
#include <unistd.h>
#include <netinet/in.h>
#include <stdlib.h>
#include <string.h>
#include <pthread.h>
#include <semaphore.h>
#include <time.h>
#include "struktury.h"
#include "server.h"
#include "stats.h"
#include "hra.h"
#include "hrac.h"
#include "clovece.h"

extern long prijatych_bajtu;
extern long odeslanych_bajtu;
extern long prijatych_zprav;
extern long odeslanych_zprav;
extern long spatnych_zprav;
extern time_t poc_cas, akt_cas;
extern pthread_mutex_t mutex_hry;
extern hra_t *hry;
extern hrac_t *hraci;
extern pthread_mutex_t mutex_hraci;
extern char *barvy[4];
pthread_t thread_id;

//Struktura pro nastaveni timeoutu socketu
struct timeval timeout;      

/*
___________________________________________________________

	Odpoji hrace ze zadane hry.
	Hrac obdrzi zpravu a vrati se do vypisu her
___________________________________________________________
*/
void odpoj(hrac_t *hrac, hra_t *hra)
{
	if(!hra || !hrac)
		return;
		
	int i;	
	for(i = 0; i < POCET_HRACU; i++)
	{
		if((hra -> hraci)[i] == hrac)
		{
			(hra -> hraci)[i] = NULL;
			(hra -> pocet_hracu)--;
			hrac -> hra = NULL;
			break;
		}
			
	}
	
	if(hrac == hra -> hrajici)
	{
		hra -> kostka = 0;
		sem_post(&(hra -> sem_odehrano));
	}

	if(hra -> pocet_hracu == POCET_HRACU -1)
		info_hry();

	char *log = malloc(sizeof(char) * 200);
	sprintf(log, "Hrac ID = %s se odpojil od hry ID = %s\n",hrac -> ID, hra -> ID);
	zapis_log(log);
	free(log);

	char *zprava = malloc(100 * sizeof(char));
	sprintf(zprava, "%s;odpoj\n", hrac -> ID);
	if(hrac -> shut != 1)
		odesli(hrac -> socket, zprava);
		
	if(hra -> pocet_hracu == 0)
	{
		zrus_hru(hra);
		free(zprava);
	}
	else
	{	
		multicast_hra(hra, zprava);
		posli_info_hraci(hra);
		info_stav(hra);
	}
	
	hrac -> barva = NULL;
	vynuluj_figurky(hrac);
}


/*
___________________________________________________________

	Rozdeli prijatou zpravu do tokenu
___________________________________________________________
*/
char **rozdel(char *zprava)
{
	if(!zprava)
		return NULL;

	char **rozdelena_zprava = malloc(MAX_TOKENS * sizeof(char*));
	int i = 0;
	
	rozdelena_zprava[i] = strtok(zprava, ";,.");
	
	if(rozdelena_zprava[i] == NULL)
		return rozdelena_zprava;
		
	i++;
	while(i < MAX_TOKENS)
	{
		rozdelena_zprava[i] = strtok(NULL, ";,.");
		
		if(rozdelena_zprava[i] == NULL)
			return rozdelena_zprava;
			
		i++;
	}
	return rozdelena_zprava;
}

/*
___________________________________________________________

	Vypise hlasku pri prijeti nekorektni zpravy
___________________________________________________________
*/
void spatna_zprava(char *zprava)
{
	printf("Ignorovana zprava %s", zprava);
	spatnych_zprav++;
}


/*
___________________________________________________________

	Zpracuje zpravu a provede potrebne ukony
___________________________________________________________
*/
void zpracuj_zadost(char *zprava)
{
	if(!zprava)
		return;

	char ** priznaky = rozdel(zprava);
	char *id;
	
	hrac_t *hrac = NULL;
	hra_t *hra = NULL;

	if(priznaky[0])
	{
		id = priznaky[0];
		hrac = najdi_hrace(id);
		
		if(!hrac)
		{
			spatna_zprava(zprava);
			return;
		}
		hra = hrac -> hra;
	}
	else spatna_zprava(zprava);
	
	if(priznaky[1])
	{
		
		if(strcmp(priznaky[1], "odehrano") == 0 && priznaky[2] && priznaky[3] && priznaky[4])
		{			

			int figurka_index = atoi(priznaky[2]);
			int policko_index = atoi(priznaky[3]);
			
			if(hra && hra -> hrajici)
				if(strcmp(hra -> hrajici -> ID, id) == 0)
				{		
					proved_posun(hra,figurka_index, policko_index);
				}

		}
		else if(strcmp(priznaky[1], "kostka") == 0 && priznaky[2])
		{
			if(hra && hra -> hrajici)
				if(strcmp(hra -> hrajici -> ID, id) == 0)
				{
					hod_kostkou(hra);
					
					if(!muze_hrat(hra))
						tahni(hra,-1);
				}
		}
		
		else if(strcmp(priznaky[1], "hraci") == 0 && priznaky[2])
		{
			posli_info_hraci(hra);
		}
		
		else if(strcmp(priznaky[1], "figurka") == 0 && priznaky[2] && priznaky[3])
		{
			int tah = atoi(priznaky[2]);			
			
			if(hra && hra -> hrajici)
				if(strcmp(hra -> hrajici -> ID, id) == 0)
					tahni(hra, tah);
		}
		
		else if(strcmp(priznaky[1], "hry") == 0)
			info_hry();
			
		else if(strcmp(priznaky[1], "nova_hra") == 0)
		{			
			vytvor_hru(hrac);
		}
		
		else if(strcmp(priznaky[1], "pripoj") == 0 && priznaky[2])
		{
			pripoj(hrac, najdi_hru(priznaky[2]));	
		}
		
		else if(strcmp(priznaky[1], "zacni_hru") == 0 && priznaky[2])
		{
			thread_id = 0;
			pthread_create(&thread_id, NULL,serve_hra, (void*)hra);
			hra -> vlakno = thread_id;
		}
			
		else if(strcmp(priznaky[1], "stav") == 0 && priznaky[2])
		{
			info_stav(hra);
		}
		
		else if(strcmp(priznaky[1], "odpoj") == 0 && priznaky[2])
		{
			odpoj(hrac, hra);
		}
		
		else if(strcmp(priznaky[1], "konec") == 0 && priznaky[2])
		{
			odpoj(hrac, hra);
			zrus_hrace(hrac);
		}
		
		else 
		{
			spatna_zprava(zprava);
		}
			
	}
	else spatna_zprava(zprava);
	
	free(priznaky);
}


/*
___________________________________________________________

	Pripoji zadaneho hrace do zadane hry
___________________________________________________________
*/
void pripoj(hrac_t *hrac, hra_t *hra)
{
	if(!hrac || !hra)
		return;	
	
	
	if(hra -> pocet_hracu >= 4)
		return;

	int i;
	for(i = 0; i < POCET_HRACU; i++)
	{
		if((hra -> hraci)[i] == NULL)
		{
			(hra -> hraci)[i] = hrac;
			hrac -> barva = barvy[i];
			//hrac -> poc_index = POSUN * i;
			hra -> pocet_hracu++;
			hrac -> hra = hra;	
			break;
		}
	}


	char *zprava = malloc(sizeof(char)*200);
	memset(zprava, 0, strlen(zprava));
	sprintf(zprava, "%s;ty;%s;%s\n", hrac -> ID, hrac -> barva, hra -> ID);
		
	odesli(hrac -> socket, zprava);
	free(zprava);

	char *log = malloc(200 * sizeof(char));
	sprintf(log, "Hrac ID = %s se pripojil do hry ID = %s\n", hrac -> ID, hra -> ID);
	zapis_log(log);
	free(log);

	posli_info_hraci(hra);
	if(hra -> pocet_hracu == POCET_HRACU)
		info_hry();



}

/*
___________________________________________________________

	Generuje unikatni cislo pro hry a hrace
___________________________________________________________
*/
char* generuj_ID()
{
	char *id = malloc(20*sizeof(char));

	do{
		sprintf(id,"%d",rand());
	}while(najdi_ID(id) != 0);

	
	return id;

}

/*
___________________________________________________________

	Hleda, jestli se zadane ID nachazi v seznamu hracu nebo her
___________________________________________________________
*/
int najdi_ID(char *id)
{
	if(!id)
		return -1;
	
	pthread_mutex_lock(&mutex_hraci);	
	if(hraci != NULL)
	{	
		hrac_t *ptr = hraci;
		
		do
		{
			if(strcmp(ptr -> ID, id) == 0)
			{
				pthread_mutex_unlock(&mutex_hraci);
				return 1;
			}

			ptr = ptr -> dalsi;
		}while(ptr != NULL);
		
	}
	pthread_mutex_unlock(&mutex_hraci);
	
	pthread_mutex_lock(&mutex_hry);
	if(hry != NULL)
	{
		hra_t *ptr = hry;
		
		do
		{
			if(strcmp(ptr -> ID, id) == 0)
			{
				pthread_mutex_unlock(&mutex_hry);
				return 1;
			}

			ptr = ptr -> dalsi;
		}while(ptr != NULL);
	}
	pthread_mutex_unlock(&mutex_hry);
	
	return 0;
}



/*
___________________________________________________________

	Prijima zpravy z daneho socketu pri padu klienta
	osetri vymazani jeho zaznamu
___________________________________________________________
*/
void *recieving(void *arg)
{	
	hrac_t *hrac = (hrac_t*) arg;
	int client_sock = hrac -> socket;
	char *id = hrac -> ID;
	
	int prijato;
	int timeout = 0;

	char cbuf[1024];
	while((prijato = recv(client_sock, cbuf, 1024 * sizeof(char), 0)) != 0)
	{
		if(prijato == -1)
		{
			if(hrac -> hra && hrac -> hra -> hrajici == hrac)
			{	
				if(timeout > 0)
				{
					char *zprava = malloc(sizeof(char)*100);
					sprintf(zprava, "%s;kicked\n", hrac -> ID);
					odesli(hrac -> socket, zprava);
					free(zprava);
					
					odpoj(hrac, hrac -> hra);
				}
				
				timeout ++;
			}
			else timeout = 0;

		}
		else 
		{
			prijatych_bajtu += prijato;
			prijatych_zprav++;

			printf("<-- %s\n",cbuf);       		
			zpracuj_zadost(cbuf);
			memset(cbuf, 0, 1024*sizeof(char));		
			
			timeout = 0;
		}


	}
	
	hrac = najdi_hrace(id);
	if(hrac)
	{
		hrac -> shut = 1;

		if(hrac -> hra)
			odpoj(hrac, hrac -> hra);
			
		zrus_hrace(hrac);
	}
	return 0;
}


/*
___________________________________________________________

	Provede pripojeni noveho klienta,
	vytvoreni zaznamu v seznamu hracu a vytvoreni prijimaciho vlakna
___________________________________________________________
*/
void *serve_request(void *arg)
{
	int client_sock = *(int *) arg;;
	char msg[64];
	memset(msg,0, strlen(msg));

	prijatych_bajtu += recv(client_sock, msg, 64 * sizeof(char), 0);
	prijatych_zprav++;
	
	//nastaveni timeoutu recv
	setsockopt (client_sock, SOL_SOCKET, SO_RCVTIMEO, (char *)&timeout,sizeof(timeout));

   	char *id = strtok(msg, ";,.");
	char *priznaky = NULL;

	if(id)
	{
		priznaky = strtok(NULL,";,.");
	}
	else spatnych_zprav++;

	if(priznaky != NULL && strcmp(priznaky, "jmeno") == 0)
	{

		char *jmeno = strtok(NULL,";,.");
		
	   if(!jmeno)
	   {
	     jmeno = malloc(sizeof(char) * 10);
	     sprintf(jmeno, "Hrac");
      }
      
		hrac_t *hrac = vytvor_hrace(client_sock, jmeno);
		
		char *zprava = malloc(sizeof(char)*100);
		sprintf(zprava, "%s;ID\n", hrac -> ID);
		odesli(hrac -> socket, zprava);
		free(zprava);
		
		pridej_hrace(hrac);
		thread_id = 0;
		pthread_create(&thread_id, NULL,recieving, (void *) hrac);
		
		char *log = malloc(100 * sizeof(char));
		sprintf(log, "Pripojil se hrac %s, ID = %s\n", hrac -> jmeno, hrac -> ID);
		zapis_log(log);
		free(log);
	}
	else 
	{
		//printf("Chyba pri pridavani hrace!!\n");
		
		char *log = malloc(100 * sizeof(char));
		sprintf(log, "Chyba pri pridavani hrace\n");
		zapis_log(log);
		free(log);
		
		spatnych_zprav++;
	}
	
	free(arg);
	
	return NULL;
}


/*
___________________________________________________________

	Odesle zpravu na dany socket a zapise do statistiky
___________________________________________________________
*/
void odesli(int socket, char *zprava)
{
	odeslanych_bajtu += send(socket, zprava, strlen(zprava)*sizeof(char), 0);
	odeslanych_zprav++;
}

/*
___________________________________________________________

	Rozesle zpravu vsem pripojenym hracum
___________________________________________________________
*/
void broadcast(char *zprava)
{	
	if(!zprava)
		return;	
		
	printf("--> %s\n",zprava);
	
	pthread_mutex_lock(&mutex_hraci);	
	if(hraci != NULL)
	{	
		hrac_t *ptr = hraci;
		
		do
		{
			if(ptr -> shut != 1)
				odesli(ptr -> socket, zprava);
				
			ptr = ptr -> dalsi;
		}while(ptr != NULL);
		
	}
	pthread_mutex_unlock(&mutex_hraci);
	
	free(zprava);
}


/*
___________________________________________________________

	Vytvori server_socket a prijima klientske spojeni
	a predava na zpracovani funkci serve_request
___________________________________________________________
*/
void * serve_accepts(void *arg)
{
	int server_sock;
	int client_sock;
	int return_value;
	int *th_socket;
	int port = *(int *)arg;
	
	struct sockaddr_in local_addr;
	struct sockaddr_in remote_addr;
	
	socklen_t remote_addr_len;
	
	server_sock = socket(AF_INET, SOCK_STREAM, 0);
	
	int flag = 1;
	setsockopt(server_sock, SOL_SOCKET, SO_REUSEADDR, &flag, sizeof(int));
   	
   	//Nastaveni timeoutu na 60 sekund
	timeout.tv_sec = 60;
    	timeout.tv_usec = 0;

	if (server_sock<=0) 
		return NULL;
	
	memset(&local_addr, 0, sizeof(struct sockaddr_in));

	local_addr.sin_family = AF_INET;
	local_addr.sin_port = htons(port);
	local_addr.sin_addr.s_addr = INADDR_ANY;

	return_value = bind(server_sock, (struct sockaddr *)&local_addr, sizeof(struct sockaddr_in));

	if (return_value == 0)
		printf("Bind OK\n");
	else
	{
		printf("Bind ERR\n");
		exit(1);
		return NULL;
	}

	return_value = listen(server_sock, 5);
	
	if (return_value == 0)
		printf("Listen OK\n");
	else
	{
		printf("Listen ERR\n");
		exit(1);
		return NULL;
	}


	while(1)
	{
		client_sock = accept(server_sock,(struct sockaddr *)&remote_addr, &remote_addr_len);
		
		if (client_sock > 0 ) 
		{
			th_socket=malloc(sizeof(int));
			*th_socket=client_sock;
			
			thread_id = 0;
			pthread_create(&thread_id, NULL,(void *)&serve_request, (void *)th_socket);
		} 
		else 
		{
			//printf("Fatalni chyba pri zpracovani socketu\n");
			
			char *log = malloc(sizeof(char) * 50);
			sprintf(log, "Fatalni chyba pri zpracovani socketu\n");
			zapis_log(log);
			
			exit(1);
			return NULL;
		}
	}
	
	return NULL;
}


/*
___________________________________________________________

	Hlavni metoda aplikace, vytvori server na zadanem portu,
	jestlize port nebyl zadan jako parametr, vytvori server
	s portem 10000.
	Vytvori ostatni vlakna nutna pro beh aplikace.
___________________________________________________________
*/
int main (int argv, char *args[])
{
	time(&poc_cas);
	
	int port;
	srand(time(NULL));
	inicializuj_barvy();
	
	FILE *logs = fopen("logs.log", "w");
	fclose(logs);
	
	char *log = malloc(sizeof(char) * 100);
	sprintf(log, "Spusteni serveru %s\n", asctime(localtime(&poc_cas)));	
	zapis_log(log);
	free(log);

	if(argv > 1)
	{
		port = atoi(args[1]);
		
		if(port > 1025 && port < 65536)
			printf("Server bezi na portu %d\n", port);
		
		else
		{	
			printf("Nastavuji defaultni port 10000\n");
			port = 10000;
		}
	}
	else
	{
		printf("Nastavuji defaultni port 10000\n");
		port = 10000;
	}

	log = malloc(sizeof(char) * 100);
	sprintf(log, "Server bezi na portu %d\n", port);	
	zapis_log(log);
	
	thread_id = 0;
	pthread_create(&thread_id, NULL, serve_accepts, (void *)&port);
	
	char vstup[1024];
	while(scanf("%s", vstup) != -1)
	{
		if(strcmp(vstup, "quit") == 0)
			break;
		if(strcmp(vstup, "info") == 0)
			vypis_info(stdout);
		if(strcmp(vstup, "hraci") == 0)
			vypis_hrace();
		if(strcmp(vstup, "hry") == 0)
			vypis_hry();
	}
	
	//printf("Ukoncuji server\n");
	pthread_cancel(thread_id);
	
	char *zprava = malloc(sizeof(char) * 50);
	sprintf(zprava, "1;konec_server\n");
	broadcast(zprava);
	
	uvolni_barvy();
	uklid_hrace();
	uklid_hry();
	
	log = malloc(sizeof(char) * 50);
	sprintf(log, "Vypnuti serveru\n");
	zapis_log(log);
	free(log);
	zapis_stats();

	vypis_info(stdout);

	return 0;
}
