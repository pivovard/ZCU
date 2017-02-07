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
#include "server.h"

extern long prijatatych_bajtu;
extern long odeslanych_bajtu;
extern long prijatatych_zprav;
extern long odeslanych_zprav;
extern long spatnych_zprav;

pthread_t thread_ids[80];
pthread_t thread_id;
int pocet_vlakenn = 0;
int odehrano = 0;
char *barvy[4];

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

void uvolni_barvy()
{
	int i;
	for(i = 0; i < POCET_HRACU; i++)
	{
		free(barvy[i]);
		barvy[i] = NULL;
	}
}

int hod_kostkou(hra_t *hra)
{
	if(!hra)
		return -1;
	
	if(hra -> kostka == 0)
	{	
		hra -> kostka = rand()%6+1;
		char *zprava = malloc(sizeof(char)*50);
		memset(zprava, 0, strlen(zprava));
		sprintf(zprava, "%s;kostka;%d\n", hra -> hrajici -> ID, hra -> kostka);
		
		multicast_hra(hra, zprava);
	}

	return hra -> kostka;
}


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
	//printf("zprava %d\n", strlen(zprava));

}

void multicast_hra(hra_t *hra, char *zprava)
{
	if(!hra || !zprava)
		return;	

	printf("%s\n",zprava);
	
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

int zkontroluj_tah(hra_t *hra, int figurka_index)
{
	if(!hra || figurka_index > POCET_FIGUREK -1)
		return -1;
	
	int kostka = hra -> kostka;
	if(figurka_index == -1)
		return 1;

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
			//printf("tri");
			return 1;
		} 		
	//printf("ctyri");			 	
	return 0;
}

void tahni(hra_t *hra, int figurka_index)
{
	if(!hra || figurka_index > POCET_FIGUREK -1)
		return;

	int res = zkontroluj_tah(hra, figurka_index);
	char *zprava = malloc(sizeof(char)*50);
	memset(zprava, 0, strlen(zprava));
	sprintf(zprava, "%s;figurka;%d;", hra -> hrajici -> ID, figurka_index);
	
	if(res)
		strcat(zprava, "ok");
	else
		strcat(zprava, "bad");
	
	strcat(zprava, "\n");
	multicast_hra(hra, zprava);
}

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
	char *zprava = malloc(sizeof(char)*50);
	memset(zprava, 0, strlen(zprava));
	sprintf(zprava, "%s;vyhra\n", hra -> hrajici -> ID);

	multicast_hra(hra, zprava);
	free(zprava);
}

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
	//printf("figurka %d posunu %d index %d\n", figurka_index,figurka -> pocet_posunu, figurka -> index);
	}	
	//odehrano = 1;
	sem_post(&(hra -> sem_odehrano));
	
}

void info_stav(hra_t *hra)
{
	if(!hra)
		return;

	char *zprava = malloc(sizeof(char)*50);
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
		}
			
	}
	
	if(hrac == hra -> hrajici)
	{
		hra -> kostka = 0;
		sem_post(&(hra -> sem_odehrano));
	}

	if(hra -> pocet_hracu == POCET_HRACU -1)
		info_hry();

	char *log = malloc(sizeof(char) * 50);
	sprintf(log, "Hrac ID = %s se odpoj od hry ID = %s\n",hrac -> ID, hra -> ID);
	zapis_log(log);
	free(log);

	char *zprava = malloc(50 * sizeof(char));
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
	//printf("barva\n");
	hrac -> barva = NULL;
	//printf("figurky\n");
	vynuluj_figurky(hrac);
	//printf("hotovo\n");
}

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

void spatna_zprava(char *zprava)
{
	printf("Ignorovana zprava %s", zprava);
	spatnych_zprav++;
}

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
				//printf("fig %d, pol %d\n", figurka_index, policko_index);			
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
			char *zprava = malloc(sizeof(char)*50);
			memset(zprava, 0, strlen(zprava));

			hra -> kostka = 0;	
			printf("Hraje %s\n", hra -> hrajici -> jmeno);
			sprintf(zprava, "%s;hraj\n", hra -> hrajici -> ID); 
			multicast_hra(hra, zprava);

			sem_wait(&(hra -> sem_odehrano));
		}while(hra -> kostka == 6);
		dalsi_hrac(hra);


	}
	return NULL;
}
