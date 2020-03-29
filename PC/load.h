#ifndef _LOAD_H
#define _LOAD_H

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "struct.h"

/*Nacteni trenovacich souboru
-ulozi slova do slovniku
-vraci pocet nactenych slov*/
int load_train_data(char *file_name, int count, Dictionary **dict, char type);

/*Nacte 1 testovany soubor - vrati string obsahu souboru*/
char * load_test_data(char *file_name, int count);

/*Zjisteni velikosti souboru*/
int get_size(FILE *file);




#endif