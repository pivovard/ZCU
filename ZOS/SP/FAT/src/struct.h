#pragma once

//buffer vystupnich casu
//std::ofstream out("out.txt");

//definice na vyznam hodnot FAT tabulky
const int FAT_UNUSED = 65535;
const int FAT_FILE_END = 65534;
const int FAT_BAD_CLUSTER = 65533;

//defaultni hodnoty vstupniho a vystupniho souboru
#define INPUT_FILE "output.fat"
#define OUTPUT_FILE "output.out.fat"

//struktura cele fat
struct fat{
	struct boot_record *p_boot_record;				//struktura boot record
	uint32_t *fat_table;							//fat tabulka (pole uint)
	struct root_directory *root_directory_table;	//pole root directory
	char *cluster_table;							//pole clusteru
};

//struktura na boot record
struct boot_record {
    char volume_descriptor[251];				//popis
	int32_t fat_type;							//typ FAT - pocet clusteru = 2^fat_type (priklad FAT 12 = 4096)
	int32_t fat_copies;							//kolikrat je za sebou v souboru ulozena FAT
	uint32_t cluster_size;						//velikost clusteru ve znacich (n x char) + '/0' - tedy 128 znamena 127 vyznamovych znaku + '/0'
	int64_t root_directory_max_entries_count;   //pocet polozek v root_directory = pocet souboru MAXIMALNE, nikoliv aktualne - pro urceni kde zacinaji data - resp velikost root_directory v souboru
	uint32_t cluster_count;						//pocet pouzitelnych clusteru (2^fat_type - reserved_cluster_count)
	uint32_t reserved_cluster_count;			//pocet rezervovanych clusteru pro systemove polozky
    char signature[4];							//pro vstupni data od vyucujicich konzistence FAT - "OK","NOK","FAI" - v poradku / FAT1 != FAT2 != FATx / FAIL - ve FAT1 == FAT2 == FAT3, ale obsahuje chyby, nutna kontrola
};

//struktura na root directory
struct root_directory{
    char file_name[13];             //8+3 format + '/0'
    char file_mod[10];              //unix atributy souboru+ '/0'
	int16_t file_type;              //0 = soubor, 1 = adresar
	int64_t file_size;              //pocet znaku v souboru 
	uint32_t first_cluster;			//cluster ve FAT, kde soubor zacina - POZOR v cislovani root_directory ma prvni cluster index 0 (viz soubor a.txt)
};