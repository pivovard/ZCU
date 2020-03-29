#include "struct.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

/*Pridani slova do slovniku*/
void addToDict(Dictionary **head, char *word, char type){
	Dictionary *temp = *head;

	/*Prochazeni vsech slov*/
	while (temp != NULL){
		/*Test shodnosti - pokud shodne, zvysi se pocet a funkce skonci*/
		if (strcmp(temp->word, word) == 0){
			temp->count++;
			/*Pro slovo ve spamu*/
			if (type == 'S'){
				temp->spam_cnt++;
			}
			/*Pro slovo v hamu*/
			if (type == 'H'){
				temp->ham_cnt++;
			}
			return;
		}
		temp = temp->next;
	}
	
	/*Pokud slovo nebylo nalezeno ve slovniku, prida se na zacatek*/
	temp = (Dictionary *)malloc(sizeof(Dictionary));
	if (temp == NULL) exit(5);
	strcpy(temp->word, word);
	temp->count = 1;
	/*Pro slovo ve spamu*/
	if (type == 'S'){
		temp->spam_cnt = 1;
		temp->ham_cnt = 0;
	}
	/*Pro slovo v hamu*/
	if (type == 'H'){
		temp->spam_cnt = 0;
		temp->ham_cnt = 1;
	}

	/*Pridani na zacatek spojoveho seznamu*/
	temp->next = *head;
	*head = temp;
}

/*Vypis slovniku*/
void printDictionary(Dictionary *head){
	/*Vsechna slova ve slovniku*/
	while (head != NULL){
		printf("%s \t %d \t %d \t %d \n %f \t %f\n", head->word, head->count, head->spam_cnt, head->ham_cnt, head->spam, head->ham);
		head = head->next;
	}
}

/*Spocte velikost slovniku - pocet unikatnich slov*/
int sizeDict(Dictionary *dict){
	int size = 0;

	/*Vsechna slova ve slovniku*/
	while (dict != NULL){
		size++;
		/*size+= dict->count;*/
		dict = dict->next;
	}

	return size;
}

/*Vypocet pravdepodobnosti vyskytu ve spamu nebo hamu pro kazde slovo*/
void pravdepodobnost(Dictionary *dict, int dict_size, int spam_words_cnt, int ham_words_cnt){
	int counter;
	
	/*pro vsechna slova ve slovniku*/
	while (dict != NULL){
		/*spam*/
		dict->spam = log((dict->spam_cnt + 1) / (double)(spam_words_cnt + dict_size));

		/*ham*/
		dict->ham = log((dict->ham_cnt + 1) / (double)(ham_words_cnt + dict_size));

		dict = dict->next;
	}
}

/*Uvolneni pameti slovniku*/
freeDict(Dictionary *head){
	Dictionary *tmp;

	/*Vsechny polozky spojoveho seznamu*/
	while (head != NULL){
		tmp = head;
		head = head->next;
		free(tmp);
	}
}





/*Funkce stareho spojoveho seznamu slov*/

/*Pridani slovo do listu slov*/
void addWord(Word **head, char *word){
	Word *temp = (Word *)malloc(sizeof(Word));
	if (temp == NULL) exit(4);
	strcpy(temp->word, word);

	temp->next = *head;
	*head = temp;
}

/*Vypis listu slov*/
void printWords(Word *head){
	while (head != NULL){
		printf("%s ", head->word);
		head = head->next;
	}
	printf("\n");
}

/*Spocte vyskyt slova v listu slov*/
int countWord(Word *words, char *word){
	int counter = 0;

	/*Pro vsechna slova*/
	while (words != NULL){
		/*Pokud je aktualni slovo shodne s hledanym, pricte se 1*/
		if (strcmp(words->word, word) == 0){
			counter++;
		}
		words = words->next;
	}

	return counter;
}

/*Uvolneni pameti seznamu slov*/
freeWord(Word *head){
	Word *tmp;

	while (head != NULL){
		tmp = head;
		head = head->next;
		free(tmp);
	}
}