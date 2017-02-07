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
#include "hra.h"
#include "clovece.h"
#include "hrac.h"

//zkratky barev, ktere jsou prirazovany hracum
char *barvy[POCET_HRACU];

//Seznam vsech hracu
hrac_t *hraci = NULL;

//Mutex umoznujici pristup k seznamu hracu
pthread_mutex_t mutex_hraci;

/*
___________________________________________________________

	Vytvori barvy a priradi jim zkratky
___________________________________________________________
*/
void inicializuj_barvy()
{
	barvy[0] = malloc(sizeof(char)*4);
	strcpy(barvy[0], "zel");
	barvy[1] = malloc(sizeof(char)*4);
	strcpy(barvy[1], "cer");
	barvy[2] = malloc(sizeof(char)*4);
	strcpy(barvy[2], "zlu");
	barvy[3] = malloc(sizeof(char)*4);
	strcpy(barvy[3], "mod");
}

/*
___________________________________________________________

	Smaze seznam barev
___________________________________________________________
*/
void uvolni_barvy()
{
	int i;
	for(i = 0; i < POCET_HRACU; i++)
	{
		free(barvy[i]);
		barvy[i] = NULL;
	}
}

/*
___________________________________________________________

	Vyradi hrace ze seznamu hracu
___________________________________________________________
*/
void zrus_hrace(hrac_t *hrac)
{
	if(!hrac)
		return;

	char *id = malloc(20*sizeof(char));
	strcpy(id, hrac -> ID);
	int socket = hrac -> socket;
	int shut = hrac -> shut;
	
	char *log = malloc(100 * sizeof(char));
	sprintf(log, "Odpojil se hrac ID = %s\n", hrac -> ID);
	
	pthread_mutex_lock(&mutex_hraci);
	
	hrac_t *ptr = hraci;
	hrac_t *predchozi = NULL;
	
	do
	{
		if(strcmp(ptr -> ID, id)==0)
		{
			if(!predchozi)
			{
				if(ptr -> dalsi == NULL)
				{
					vymaz_hrace(ptr);
					hraci = NULL;
				}
				else
				{
					hraci = ptr -> dalsi;
					vymaz_hrace(ptr);
					ptr = NULL;
				}
			}
			else
			{
				predchozi -> dalsi = ptr -> dalsi;
				vymaz_hrace(ptr);
				ptr = NULL;
			}
			break;
				
		}
		
		predchozi = ptr;
		ptr = ptr -> dalsi;
		
	}while(ptr != NULL);
	
	pthread_mutex_unlock(&mutex_hraci);
	
	char *zprava = malloc(sizeof(char)*50);
	sprintf(zprava, "%s;konec\n", id);	
	
	if(shut != 1)
		odesli(socket, zprava);

	zapis_log(log);
	free(log);

	free(zprava);
	free(id);
}

/*
___________________________________________________________

	Vymaze hrace tj. uvolni pamet, kterou hrac zabiral
___________________________________________________________
*/
void vymaz_hrace(hrac_t *hrac)
{
	free(hrac-> ID);	
	free(hrac-> jmeno);
	
	int i;	
	for(i = 0; i < POCET_FIGUREK; i++)
	{
		free((hrac -> figurky)[i]);
	}
	free(hrac);
}

/*
___________________________________________________________

	Najde hrace v seznamu hracu podle ID,
	vraci NULL pri neuspechu.
___________________________________________________________
*/
hrac_t *najdi_hrace(char *id)
{
	if(!id)
		return NULL;

	pthread_mutex_lock(&mutex_hraci);
	if(hraci != NULL)
	{
		hrac_t *ptr = hraci;
		do
		{
			if(strcmp(ptr -> ID, id) == 0)
			{
				pthread_mutex_unlock(&mutex_hraci);
				return ptr;
			}
			
			ptr = ptr -> dalsi;
			
		}while(ptr != NULL);
	}
	pthread_mutex_unlock(&mutex_hraci);
	
	return NULL;
}


/*
___________________________________________________________

	Prida hrace do seznamu hracu
___________________________________________________________
*/
void pridej_hrace(hrac_t *hrac)
{
	
	if(!hrac)
		return;	
	
	pthread_mutex_lock(&mutex_hraci);
	if(hraci == NULL)
	{
		hraci = hrac;
	}
	else 
	{
		hrac_t *ptr = hraci;
		
		while(ptr -> dalsi != NULL)
		{
			ptr = ptr -> dalsi;
		}
		ptr -> dalsi = hrac;
				
	}
	pthread_mutex_unlock(&mutex_hraci);
}


/*
___________________________________________________________

	Vytvori zadanemu hraci figruky
___________________________________________________________
*/
void udelej_figurky(hrac_t *hrac)
{
	if(hrac != NULL)
	{
		int i;
		for(i = 0; i < POCET_FIGUREK; i++)
		{
			figurka_t *figurka = malloc(sizeof(figurka_t));
			figurka -> pocet_posunu = 0;
			figurka -> index = -1;
			(hrac -> figurky)[i] = figurka;	
		}	
	
	}
}

/*
___________________________________________________________

	Vytvori noveho hrace
___________________________________________________________
*/
hrac_t *vytvor_hrace(int socket, char *jmeno)
{
	hrac_t *hrac = malloc(sizeof(hrac_t));
	hrac -> jmeno = malloc(50*sizeof(char));
	sprintf(hrac -> jmeno,"%s",jmeno);
	
	hrac -> socket = socket;
	hrac -> shut = 0;
	hrac -> ID = generuj_ID();
	hrac -> dalsi = NULL;
	hrac -> hra = NULL;
	
	udelej_figurky(hrac);
	
	return hrac;
}


/*
___________________________________________________________

	Smaze cely seznam hracu
___________________________________________________________
*/
void uklid_hrace()
{
	while(hraci)
	{
		zrus_hrace(hraci);
	}
}

/*
___________________________________________________________

	Vypise seznam hracu
___________________________________________________________
*/
void vypis_hrace()
{
	hrac_t *ptr = hraci;
	printf("==================== Seznam hracu ======================\n");
	if(hraci != NULL)
	{
		do
		{
			printf("Jmeno %s ID = %s\n", ptr -> jmeno, ptr -> ID); 
			ptr = ptr -> dalsi;
		}while(ptr != NULL);
	}
	else printf("Seznam hracu je prazdny\n");

	printf("=========================================================\n");
}

