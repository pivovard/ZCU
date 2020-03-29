#include "load.h"
#include "struct.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

/*Nacteni trenovacich souboru
-ulozi slova do slovniku
-vraci pocet nactenych slov*/
int load_train_data(char *file_name, int file_count, Dictionary **dict, char type){
	
	/*Pomocne promenne pro praci se soubory*/
	char *path, *tmp_digit, *string, *token;
	FILE *file;
	int i, size, count = 0;

	for (i = 1; i <= file_count; i++){
		path = (char *)malloc(1024);
		tmp_digit = (char *)malloc(32);

		/*Cesta k souboru*/
		/*path = strcpy(path, "data/");
		path = strcat(path, file_name);
		sprintf(tmp_digit, "%d", i);
		path = strcat(path, tmp_digit);
		path = strcat(path, ".txt");*/

		sprintf(path, "data/%s%d.txt", file_name, i);

		/*printf("Nacitam %s\n", path);*/

		/*Otevreni souboru*/
		file = fopen(path, "r");
		if (file == NULL){
			printf("Nepodarilo se nacist soubor!\n");
		}

		free(path);
		free(tmp_digit);

		/*Zjisteni velikosti souboru*/
		size = get_size(file);

		/*Alokovani mista pro vstupni data (+1 pro ukoncovaci znak '\0'*/
		string = (char *)malloc(size + 1);
		/*Precteni dat ze souboru*/
		fgets(string, size, file);

		/*Zavre trenovaci soubor*/
		fclose(file);

		/*Rozdeleni retezce na slova*/
		token = strtok(string, " ");

		while (token != NULL){
			count++;

			/*Pridani do slovniku*/
			addToDict(dict, token, type);

			token = strtok(NULL, " ");
		}

		free(string);
		free(token);
	}

	return count;
}

/*Nacte 1 testovany soubor - vrati string obsahu souboru*/
char * load_test_data(char *file_name, int count){

	/*Pomocne promenne pro praci se soubory*/
	char *path, *tmp_digit, *string;
	FILE *file;
	int size;
	
	path = (char *)malloc(1024);
	tmp_digit = (char *)malloc(32);

	/*Cesta k souboru*/
	/*path = strcpy(path, "data/");
	path = strcat(path, file_name);
	sprintf(tmp_digit, "%d", count);
	path = strcat(path, tmp_digit);
	path = strcat(path, ".txt");*/

	sprintf(path, "data/%s%d.txt", file_name, count);

	/*printf("Nacitam %s\n", path);*/

	/*Otevreni souboru*/
	file = fopen(path, "r");
	if (file == NULL){
		printf("Nepodarilo se nacist soubor!\n");
		return NULL;
	}
	free(path);
	free(tmp_digit);

	/*Zjisteni velikosti souboru*/
	size = get_size(file);

	/*Alokovani mista pro vstupni data (+1 pro ukoncovaci znak '\0'*/
	string = (char *)malloc(size + 1);
	/*Precteni dat ze souboru*/
	fgets(string, size, file);

	/*Zavre testovaci soubor*/
	fclose(file);

	return string;
}



/*Zjisteni velikosti souboru*/
int get_size(FILE *file){
	int size;

	/*Skok na konec souboru*/
	fseek(file, 0, SEEK_END);
	/*Velikost souboru*/
	size = ftell(file);
	/*Skok na zacatek souboru*/
	fseek(file, 0, SEEK_SET);

	return size;
}