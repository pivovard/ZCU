#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

#include "struct.h"
#include "load.h"

/*Vstupni argumenty*/
char *spam_file, *ham_file, *test_file, *output_file;
int spam_cnt, ham_cnt, test_cnt;

/*Pravdepodobnost vyskytu klasifikacni tridy*/
double P_spam;
double P_ham;

/*Slovnik*/
Dictionary *dictionary;
int dict_size;

/*Pocty slov*/
int spam_words_cnt, ham_words_cnt;

/*Nacte a klasifikuje testovaci tridy a vysledek ulozi do souboru*/
void classify(){
	Dictionary *temp;
	char *string, *token;
	double spam = 0;
	double ham = 0;
	FILE *file;
	int i;

	/*Otevreni souboru pro zapis*/
	file = fopen(output_file, "w");
	if (file == NULL){
		printf("Nepodarilo se otevrit soubor pro zapis!\n");
		return;
	}

	/*Pro vsechny testovaci soubory*/
	for (i = 1; i <= test_cnt; i++){
		/*Nacte soubor*/
		string = load_test_data(test_file, i);

		/*Pro vsechna slova - vypocet sumy*/
		token = strtok(string, " ");
		while (token != NULL){
			temp = dictionary;

			/*Prochazeni slovniku*/
			while (temp != NULL){
				if (strcmp(temp->word, token) == 0){
					spam += temp->spam;
					ham += temp->ham;
					break;
				}

				temp = temp->next;
			}

			token = strtok(NULL, " ");
		}

		free(string);
		free(token);

		/*Pricte P(c)*/
		spam += log(P_spam);
		ham += log(P_ham);

		/*Zapise vysledek do souboru*/
		if (spam > ham){
			printf("test%d.txt\tS\n", i);
			fprintf(file, "test%d.txt\tS", i);
		}
		else
		{
			printf("test%d.txt\tH\n", i);
			fprintf(file, "test%d.txt\tH", i);
		}

		/*Odradkovani*/
		if (i < test_cnt){
			fprintf(file, "\n");
		}

		/*Vynulovani pro dalsi soubor*/
		spam = 0;
		ham = 0;
	}

	/*Zavreni souboru pro zapis*/
	fclose(file);
}

/*Vypise napovedu pri zadani spatnych argumentu*/
int help(){
	printf("Wrong arguments\n"
		"Set arguments: <spam> <spam - cnt> <ham> <ham - cnt> <test> <test - cnt> <out - file> where\n"
		"<spam>, <ham>, <test> are patterns of names of entry files\n"
		"<spam - cnt>, <ham - cnt>, <test - cnt> are number of entry files\n"
		"and <out - file> is name of output file\n\n");

	system("pause");

	return 1;
}

int main(int argc, char *argv[]){
	
	/*Test spravnosti argumentu*/
	if (argc != 8 || isdigit(*argv[3]) || isdigit(*argv[5]) || isdigit(*argv[7])){
		return help();
	}

	/*Ulozeni argumentu z prikazove radky do promennych*/
	spam_file = argv[1];
	spam_cnt = strtol(argv[2], NULL, 10);
	ham_file = argv[3];
	ham_cnt = strtol(argv[4], NULL, 10);
	test_file = argv[5];
	test_cnt = strtol(argv[6], NULL, 10);
	output_file = argv[7];

	/*Pravdepodobnost vyskytu klasifikacni tridy*/
	P_spam = spam_cnt / (double)(spam_cnt + ham_cnt);
	P_ham = ham_cnt / (double)(spam_cnt + ham_cnt);

	/*Nacteni trenovacich souboru*/
	spam_words_cnt = load_train_data(spam_file, spam_cnt, &dictionary, 'S');
	ham_words_cnt = load_train_data(ham_file, ham_cnt, &dictionary, 'H');

	dict_size = sizeDict(dictionary);

	/*Vypocet pravdepodobnosti vyskytu ve spamu nebo hamu pro kazde slovo*/
	pravdepodobnost(dictionary, dict_size, spam_words_cnt, ham_words_cnt);
	/*printDictionary(dictionary);*/

	/*Klasifikuje testovaci tridy a vysledek ulozi do souboru*/
	classify();

	printf("Klasifikace dokoncena.\n");

	/*Uvolneni pameti*/
	freeDict(dictionary);

	/*system("pause");*/

	return 0;
}