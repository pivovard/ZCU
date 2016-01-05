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
#include "hrac.h"
#include "clovece.h"

extern long prijatych_bajtu;
extern long odeslanych_bajtu;
extern long prijatych_zprav;
extern long odeslanych_zprav;
extern long spatnych_zprav;

pthread_t thread_id;

/*
___________________________________________________________

	Vygeneruje novou hodnotu pro kostku v zadane hre.
	Jestlize kostka jiz byla hozena vraci hozenou hodnotu
___________________________________________________________
*/
int hod_kostkou(hra_t *hra)
{
	if(!hra)
		return -1;
	
	if(hra -> kostka == 0)
	{	
		hra -> kostka = rand()%6+1;
		
		char *zprava = malloc(sizeof(char)*100);
		memset(zprava, 0, strlen(zprava));
		sprintf(zprava, "%s;kostka;%d\n", hra -> hrajici -> ID, hra -> kostka);		
		multicast_hra(hra, zprava);
	}

	return hra -> kostka;
}

/*
___________________________________________________________

	Odesle vesem hracum ve hre informace,
	o vsech hracich hrajich tuto hru
___________________________________________________________
*/
void posli_info_hraci(hra_t *hra)
{
	if(!hra)
		return;
	
	char *zprava = malloc(sizeof(char)*1024);
	memset(zprava, 0, strlen(zprava));
	sprintf(zprava, "1;hraci");		

	int i;
	for(i = 0; i < POCET_HRACU; i++)
	{	
		hrac_t *hrac = (hra -> hraci)[i];
		
		if(hrac)
		{
			sprintf(zprava, "%s;%s;%s;%s", zprava, hrac -> ID, hrac -> jmeno, hrac -> barva);
		}	
	}
	strcat(zprava, "\n");	
	multicast_hra(hra, zprava);

}


/*
___________________________________________________________

	Zkontrulje zda hrajici hrac muze tahnout s figurkou
	zadaneho indexu. 
	Vraci 1 jestli je posun mozny, 0 kdyz ne 
	a -1 pri chybe.
___________________________________________________________
*/
int zkontroluj_tah(hra_t *hra, int figurka_index)
{
	if(!hra || figurka_index > POCET_FIGUREK -1)
		return -1;
		
	if(figurka_index == -1)
		return 1;
	
	int kostka = hra -> kostka;
	figurka_t *figurka = (hra -> hrajici -> figurky)[figurka_index];
	
	if(figurka -> pocet_posunu == 0)
	{
		if(kostka == 6)
		{
			int i;			
			for(i = 0; i < POCET_FIGUREK; i++)
			{
				if(i != figurka_index)				
					if((hra -> hrajici -> figurky)[i] -> pocet_posunu == 1)
						return 0;
			}
			return 1;
		}
		return 0;
	}
			
	if((figurka -> pocet_posunu + kostka) <= POCET_POLICEK)
	{
		int i;			
		for(i = 0; i < POCET_FIGUREK; i++)
		{
			if(i != figurka_index)				
				if((hra -> hrajici -> figurky)[i] -> pocet_posunu == figurka -> pocet_posunu + kostka)
					return 0;
		}
		return 1;
	}
	else if((figurka -> pocet_posunu + kostka) <= (POCET_POLICEK + POCET_FIGUREK))
		{
			int i = 0;			
			for(; i < POCET_FIGUREK; i++)
			{
				if(i != figurka_index)				
					if((hra -> hrajici -> figurky)[i] -> pocet_posunu == figurka -> pocet_posunu + kostka)
						return 0;
			}
			return 1;
		} 				 	
	return 0;
}


/*
___________________________________________________________

	Zkontroluje, jestli hrajici hrac muze hrat
	zadanou figurkou a odesle zpravu
___________________________________________________________
*/
void tahni(hra_t *hra, int figurka_index)
{
	if(!hra || figurka_index > POCET_FIGUREK -1)
		return;
	
	char *zprava = malloc(sizeof(char)*100);
	memset(zprava, 0, strlen(zprava));
	sprintf(zprava, "%s;figurka;%d;", hra -> hrajici -> ID, figurka_index);
	
	int res = zkontroluj_tah(hra, figurka_index);
	
	if(res)
		strcat(zprava, "ok");
	else
		strcat(zprava, "bad");
	
	strcat(zprava, "\n");
	multicast_hra(hra, zprava);
}


/*
___________________________________________________________

	Zkontroluje, zda se na jednom policku nenachazi vice
	figurek. Jestlize ano, vyhodi jednu figurku podle pravidel
___________________________________________________________
*/
void vyhod(hra_t *hra, figurka_t *figurka)
{
	if(!hra || !figurka)
		return;

	int index = figurka -> index;
	int i;
	for(i = 0; i < POCET_HRACU; i++)
	{
		hrac_t *hrac = (hra -> hraci)[i];
		if(hrac)
		{
			figurka_t **figurky = hrac -> figurky;
			
			int j;
			for(j = 0; j < POCET_FIGUREK; j++)
			{
				if(figurky[j] != figurka && figurka -> pocet_posunu <= POCET_POLICEK && figurky[j] -> pocet_posunu <= POCET_POLICEK)					
					if(figurky[j] -> index == index)
					{
						figurky[j] -> pocet_posunu = 0;
						figurky[j] -> index = -1;
						return;
					}
			}
		}
	}
	
}


/*
___________________________________________________________

	Zkontrulje zda hrajici hrac muze tahnout alespon
	jednou figurkou.
	Vraci 1 jestlize ano a 0 kdyz ne.
___________________________________________________________
*/
int muze_hrat(hra_t *hra)
{
	if(!hra)
		return -1;

	int i;
	for(i = 0; i < POCET_FIGUREK; i++)
	{
		if(zkontroluj_tah(hra, i))
			return 1;
	}
	return 0;
}


/*
___________________________________________________________

	Zkontrulje zda hrajici hrac dostal vsechny
	figurky do domecku, ze tedy vyhral.
___________________________________________________________
*/
void zkontroluj_konec(hra_t *hra)
{
	if(!hra)
		return;

	int i;
	for(i = 0; i < POCET_FIGUREK; i++)
			{			
				if((hra -> hrajici -> figurky)[i] -> pocet_posunu <= POCET_POLICEK)
					return;
			}
	
	char *zprava = malloc(sizeof(char)*100);
	memset(zprava, 0, strlen(zprava));
	sprintf(zprava, "%s;vyhra\n", hra -> hrajici -> ID);

	multicast_hra(hra, zprava);
}


/*
___________________________________________________________

	Presune zadanou figurku na nove policko
___________________________________________________________
*/
void proved_posun(hra_t *hra, int figurka_index, int policko_index)
{
	if(!hra || figurka_index > POCET_FIGUREK -1 || policko_index > POCET_POLICEK)
		return;	

	if(figurka_index >= 0)
	{	
		figurka_t *figurka = (hra -> hrajici -> figurky)[figurka_index];
	
		if(figurka -> pocet_posunu == 0)
			figurka -> pocet_posunu++;	
		else figurka -> pocet_posunu += hra -> kostka;	
	
		figurka -> index = policko_index;
		
		vyhod(hra, figurka);
		zkontroluj_konec(hra);
	}	
	sem_post(&(hra -> sem_odehrano));	
}


/*
___________________________________________________________

	Posle informaci o aktualnim stavu hry
___________________________________________________________
*/
void info_stav(hra_t *hra)
{
	if(!hra)
		return;

	char *zprava = malloc(sizeof(char)*100);
	memset(zprava, 0, strlen(zprava));
	
	hrac_t *admin = NULL;
	int stav = 1;

	int i;	
	for(i = 0; i < POCET_HRACU; i++)
	{
		if((hra -> hraci)[i])
		{
			admin = (hra -> hraci)[i];
			break;
		}
			
	}
	
	if(hra -> pocet_hracu == 1)
	{
		stav = 2;
		if(hra -> hrajici != NULL)
		{
			sprintf(zprava, "%s;vyhra\n", admin -> ID);
			multicast_hra(hra, zprava);
			return;
		}
	}
	if(admin != NULL && hra -> hrajici == NULL)
	{
		sprintf(zprava, "%s;stav;%d\n", admin -> ID, stav);
		multicast_hra(hra, zprava);
	}
}


/*
___________________________________________________________

	Nastavi hraci figurky na pocatecni pozice
___________________________________________________________
*/
void vynuluj_figurky(hrac_t *hrac)
{
	if(!hrac)
		return;

	int i;
	for(i = 0; i < POCET_FIGUREK; i++)
	{
		figurka_t *figurka = (hrac -> figurky)[i];
		
		if(figurka)
		{
			figurka -> pocet_posunu = 0;
			figurka -> index = -1;
		}
	}	
}


/*
___________________________________________________________

	Zvoli dalsiho hrace, ktery je na tahu
	Jestlize nelze nikoho zvolit hrajici je NULL
___________________________________________________________
*/
void dalsi_hrac(hra_t *hra)
{
	if(!hra)
		return;
		
	int i;
	for(i = 0; i < POCET_HRACU; i++)
	{
		hra -> hrajici_index = (hra -> hrajici_index + 1) % POCET_HRACU;					
		hra -> hrajici = hra -> hraci[hra -> hrajici_index];
		
		if(hra -> hrajici != NULL)
			break;
	}
	hra -> kostka = 0;
}

/*
___________________________________________________________

	Zprostredkovava celou hru, 
	- ceka na odehrani hrajiciho
	- strida hrace
	- pri hozeni sestky opakuje tah
___________________________________________________________
*/
void* serve_hra(void *arg)
{
	if(!arg)
		return NULL;

	hra_t *hra = (hra_t*) arg;	
	hra -> hrajici_index = rand()% POCET_HRACU;
	
	dalsi_hrac(hra);	
	info_hry();

	while(hra -> hrajici != NULL)
	{
		do{
			hra -> kostka = 0;
			
			char *zprava = malloc(sizeof(char)*100);
			memset(zprava, 0, strlen(zprava));	
			sprintf(zprava, "%s;hraj\n", hra -> hrajici -> ID); 
			multicast_hra(hra, zprava);

			sem_wait(&(hra -> sem_odehrano));
		}while(hra -> kostka == 6);
		
		dalsi_hrac(hra);
	}
	return NULL;
}
