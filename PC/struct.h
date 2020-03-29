#ifndef _STRUCT_H
#define _STRUCT_H

/*Slovnik - slovo, pocet vyskytu*/
typedef struct Dictionary {
	char word[30];
	int count;
	int spam_cnt;
	int ham_cnt;
	double spam;
	double ham;
	struct Dictionary *next;
} Dictionary;

/*Pridani slova do slovniku*/
void addToDict(Dictionary **head, char *word, char type);

/*Vypis slovniku*/
void printDictionary(Dictionary *head);

/*Velikost slovniku*/
int sizeDict(Dictionary *dict);

/*Vypocet pravdepodobnosti vyskytu ve spamu nebo hamu pro kazde slovo*/
void pravdepodobnost(Dictionary *dict, int dict_size, int spam_words_cnt, int ham_words_cnt);

/*Uvolneni pameti slovniku*/
freeDict(Dictionary *head);







/*Funkce stareho spojoveho seznamu slov*/

/*List slov*/
typedef struct Word{
	char word[30];
	struct Word *next;
} Word;

/*Pridani slovo do listu slov*/
void addWord(Word **head, char *word);

/*Vypis listu slov*/
void printWords(Word *head);

/*Spocte vyskyt slova v listu slov*/
int countWord(Word *words, char *word);

/*Uvolneni pameti seznamu slov*/
freeWord(Word *head);



#endif