#define POCET_POLICEK 32
#define POCET_FIGUREK 4
#define POCET_HRACU 4
#define MAX_TOKENS 5

#ifndef STRUKTURY_H
#define STRUKTURY_H

typedef struct figurka
{
	int pocet_posunu;
	int index;
}figurka_t;

typedef struct hrac
{
	int socket;
	int shut;
	char *jmeno;
	char *barva;
	char *ID;
	//int poc_index;
	figurka_t *figurky[POCET_FIGUREK];
	struct hrac *dalsi;
	struct hra *hra;
	
}hrac_t;

typedef struct hra
{
	char *ID;
	char *jmeno;
	hrac_t *hraci[4];
	int pocet_hracu;
	hrac_t *hrajici;
	int kostka;
	sem_t sem_odehrano;
	int hrajici_index;
	pthread_t vlakno;
	struct hra *dalsi;
}hra_t;

#endif
