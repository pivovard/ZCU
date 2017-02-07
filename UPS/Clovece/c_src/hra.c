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
#include "struktury.h"
#include "server.h"
#include "stats.h"
#include "hrac.h"
#include "clovece.h"
#include "hra.h"

//Mutex chranici pristup ke spojovemu seznamu hry
pthread_mutex_t mutex_hry;

//Spojovy seznam vsech existujicich her
hra_t *hry = NULL;

/*
___________________________________________________________

	Najde hru v seznamu podle ID
	Pri neuspechu vraci NULL
___________________________________________________________
*/
hra_t *najdi_hru(char *id)
{	
	pthread_mutex_lock(&mutex_hry);	
	if(hry != NULL)
	{
		hra_t *ptr = hry;
		do
		{
			if(strcmp(ptr -> ID, id) == 0)
			{
				pthread_mutex_unlock(&mutex_hry);
				return ptr;
			}
			ptr = ptr -> dalsi;
		}while(ptr != NULL);
	}
	pthread_mutex_unlock(&mutex_hry);
	return NULL;
}

/*
___________________________________________________________

	Rozesle vsem uzivatelum informace o hrach, 
	ke kterym se lze pripojit.
___________________________________________________________
*/
void info_hry()
{
	hra_t *ptr = hry;
	
	char *zprava = malloc(sizeof(char)*1024);
	memset(zprava, 0, strlen(zprava));
	sprintf(zprava, "1;hry");

	pthread_mutex_lock(&mutex_hry);
	if(hry != NULL)
	{
		do
		{
			if(!ptr -> hrajici && ptr -> pocet_hracu < POCET_HRACU)
				sprintf(zprava, "%s;%s;%s", zprava, ptr -> jmeno, ptr -> ID);
				
			ptr = ptr -> dalsi;
		}while(ptr != NULL);
	}
	pthread_mutex_unlock(&mutex_hry);
	
	strcat(zprava, "\n");	
	broadcast(zprava);

}


/*
___________________________________________________________

	Vytvori novou hru a pripoji do ni vytvarejiciho hrace
___________________________________________________________
*/
void vytvor_hru(hrac_t *hrac)
{
	if(!hrac)
		return;

	hra_t *hra = malloc(sizeof(hra_t));
	hra -> ID = generuj_ID();
	
	hra -> jmeno = malloc(sizeof(char)*30);
	strcpy(hra -> jmeno, "hra-");
	
	strcat(hra -> jmeno, hra -> ID);
	hra -> dalsi = NULL;
	hra -> pocet_hracu = 0;
	
	sem_init(&(hra -> sem_odehrano),0,0);
	hra -> hrajici = NULL;
	
	int i;
	for(i = 0; i < POCET_HRACU; i++)
	{
		(hra -> hraci)[i] = NULL;
	}
	
	char *log = malloc(sizeof(char) * 50);
	sprintf(log, "Vytvorena hra ID = %s\n", hra -> ID);
	zapis_log(log);
	free(log);
	
	zarad_hru(hra);
	pripoj(hrac, hra);
	info_hry();
}


/*
___________________________________________________________

	Zaradi novou hru do seznamu her
___________________________________________________________
*/
void zarad_hru(hra_t *hra)
{	
	if(!hra)
		return;	

	pthread_mutex_lock(&mutex_hry);	
	if(hry == NULL)
	{
		hry = hra;
	}
	else 
	{
		hra_t *ptr = hry;
		
		while(ptr -> dalsi != NULL)
		{
			ptr = ptr -> dalsi;
		}
		
		ptr -> dalsi = hra;
				
	}
	pthread_mutex_unlock(&mutex_hry);
}


/*
___________________________________________________________

	Vyradi hru ze seznamu her
___________________________________________________________
*/
void zrus_hru(hra_t *hra)
{	
	if(!hra)
		return;
	
	char *id = hra -> ID;
	hra_t *ptr = hry;
	hra_t *predchozi = NULL;
	
	char *log = malloc(100 * sizeof(char));
	sprintf(log, "Zrusena hra ID = %s\n", hra -> ID);

	pthread_mutex_lock(&mutex_hry);
	do
	{
		if(strcmp(ptr -> ID, id)==0)
		{
			if(!predchozi)
			{
				if(ptr -> dalsi == NULL)
				{
					vymaz_hru(ptr);
					hry = NULL;
				}
				else
				{
					hry = ptr -> dalsi;
					vymaz_hru(ptr);
				}
			}
			else
			{
				predchozi -> dalsi = ptr -> dalsi;
				vymaz_hru(ptr);
				ptr = NULL;
			}
			break;
				
		}
		
		predchozi = ptr;
		ptr = ptr -> dalsi;
		
	}while(ptr != NULL);
	pthread_mutex_unlock(&mutex_hry);

	zapis_log(log);
	free(log);
	info_hry();
}


/*
___________________________________________________________

	Vymaze hru tj. uvolni pamet, kterou hra zabrala
___________________________________________________________
*/
void vymaz_hru(hra_t *hra)
{
	if(!hra)
		return;

	free(hra -> ID);	
	free(hra -> jmeno);
	sem_destroy(&(hra -> sem_odehrano));
	free(hra);	
}


/*
___________________________________________________________

	Rozesle zpravu vsem hracum zadane hry
___________________________________________________________
*/
void multicast_hra(hra_t *hra, char *zprava)
{
	if(!hra || !zprava)
		return;	

	printf("--> %s\n",zprava);
	
	int i;
	for(i = 0; i < POCET_HRACU; i++)
	{
		if((hra -> hraci)[i])
		{
			if((hra -> hraci)[i] -> shut != 1)
				odesli((hra -> hraci)[i] -> socket, zprava);
		}
	}
	free(zprava);
}


/*
___________________________________________________________

	Smaze vsechny hry
___________________________________________________________
*/
void uklid_hry()
{
	while(hry)
	{
		zrus_hru(hry);
	}
}

/*
___________________________________________________________

	Vypise seznam hrer
___________________________________________________________
*/
void vypis_hry()
{
	hra_t *ptr = hry;
	printf("==================== Seznam her ========================\n");
	if(hry != NULL)
	{
		do
		{
			printf("Jmeno %s ID = %s\n", ptr -> jmeno, ptr -> ID); 
			ptr = ptr -> dalsi;
		}while(ptr != NULL);
	}
	else printf("Seznam her je prazdny\n");

	printf("=========================================================\n");
}


