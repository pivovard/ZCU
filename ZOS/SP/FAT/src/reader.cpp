#include "stdafx.h"
#include "reader.h"

//nacte fat a vytvori strukturu fat
struct fat * read(char *file_name){
	cout << "Nacitam soubor FAT: " << file_name << endl;

	FILE *p_file;

	//otevru soubor pro cteni a pro jistotu skocim na zacatek
	if (p_file = fopen(file_name, "rb")){
	}
	else{
		cout << "Nepodarilo se otevrit soubor pro cteni FAT" << endl;
	}
	fseek(p_file, SEEK_SET, 0);

	//cteni boot record                         
	struct boot_record *p_boot_record = (struct boot_record *) malloc(sizeof(struct boot_record));
	fread(p_boot_record, sizeof(struct boot_record), 1, p_file);

	//cteni fat
	uint32_t *fat_table = new uint32_t[p_boot_record->cluster_count];
	uint32_t *fat_item = (uint32_t *)malloc(sizeof(uint32_t));

	for (int i = 0; i < p_boot_record->fat_copies; i++){
		for (int j = 0; j < p_boot_record->cluster_count; j++)
		{
			fread(fat_item, sizeof(uint32_t), 1, p_file);
			fat_table[j] = *fat_item;
		}
	}

	//cteni root directory
	struct root_directory *root_directory_table = new struct root_directory[p_boot_record->root_directory_max_entries_count];
	struct root_directory *p_root_directory = (struct root_directory *) malloc(sizeof(struct root_directory));

	for (int i = 0; i < p_boot_record->root_directory_max_entries_count; i++)
	{
		fread(p_root_directory, sizeof(struct root_directory), 1, p_file);
		root_directory_table[i] = *p_root_directory;
	}

	//cteni clusteru
	uint32_t real_cluster_count = p_boot_record->cluster_count + p_boot_record->reserved_cluster_count;
	char *cluster_table = new char[real_cluster_count * p_boot_record->cluster_size];
	char *p_cluster = (char *)malloc(sizeof(char) * p_boot_record->cluster_size);

	for (int i = 0; i < real_cluster_count; i++)
	{
		fread(p_cluster, sizeof(char) * p_boot_record->cluster_size, 1, p_file);
		//cluster_table[i* p_boot_record->cluster_size] = *p_cluster;
		strncpy(&cluster_table[i* p_boot_record->cluster_size], p_cluster, sizeof(char) * p_boot_record->cluster_size);
	}

	fclose(p_file);

	//vytvoreni fat v pameti
	struct fat *p_fat = (struct fat *) malloc(sizeof(p_boot_record) + sizeof(fat_table) + sizeof(root_directory_table) + sizeof(cluster_table));
	p_fat->p_boot_record = p_boot_record;
	p_fat->fat_table = fat_table;
	p_fat->root_directory_table = root_directory_table;
	p_fat->cluster_table = cluster_table;

	cout << "Soubor nacten." << endl;

	return p_fat;
}

//vytvori novy soubor a ulozi do nej data ze structury fat
void write(char *file_name, struct fat *p_fat)
{
	cout << "Ukladam soubor FAT: " << file_name << endl;

	FILE *p_file;
	
	//otevru soubor pro zapis a pro jistotu skocim na zacatek
	if (p_file = fopen(file_name, "wb")){
	}
	else{
		cout << "Nepodarilo se otevrit soubor pro zapis FAT" << endl;
	}
	fseek(p_file, SEEK_SET, 0);
	
	//zapis boot record
	fwrite(p_fat->p_boot_record, sizeof(struct boot_record), 1, p_file);

	//zapis fat
	uint32_t *fat_item = (uint32_t *)malloc(sizeof(uint32_t));

	for (int i = 0; i < p_fat->p_boot_record->fat_copies; i++){
		for (int j = 0; j < p_fat->p_boot_record->cluster_count; j++)
		{
			*fat_item = p_fat->fat_table[j];
			fwrite(fat_item, sizeof(uint32_t), 1, p_file);
		}
	}

	//zapis root directory
	struct root_directory *p_root_directory = (struct root_directory *) malloc(sizeof(struct root_directory));

	for (int i = 0; i < p_fat->p_boot_record->root_directory_max_entries_count; i++)
	{
		*p_root_directory = p_fat->root_directory_table[i];
		fwrite(p_root_directory, sizeof(struct root_directory), 1, p_file);
	}

	//zapis clusteru
	uint32_t real_cluster_count = p_fat->p_boot_record->cluster_count + p_fat->p_boot_record->reserved_cluster_count;
	char *p_cluster = (char *)malloc(sizeof(char) * p_fat->p_boot_record->cluster_size);

	for (int i = 0; i < real_cluster_count; i++)
	{
		//*p_cluster = cluster_table[i* p_boot_record->cluster_size];
		strncpy(p_cluster, &p_fat->cluster_table[i* p_fat->p_boot_record->cluster_size], sizeof(char) * p_fat->p_boot_record->cluster_size);
		fwrite(p_cluster, sizeof(char) * p_fat->p_boot_record->cluster_size, 1, p_file);
	}

	fclose(p_file);

	cout << "Soubor ulozen." << endl;
}

//nacte a vypise fat (upravena verze ze zadani)
void read_default(char *file_name)
{
	FILE *p_file;

	int i;
	//pointery na struktury root a boot                         
	struct boot_record *p_boot_record;
	struct root_directory *p_root_directory;

	//alokujeme pamet
	p_boot_record = (struct boot_record *) malloc(sizeof(struct boot_record));
	p_root_directory = (struct root_directory *) malloc(sizeof(struct root_directory));


	//otevru soubor a pro jistotu skocim na zacatek           
	p_file = fopen(file_name, "rb");
	fseek(p_file, SEEK_SET, 0);

	//prectu boot
	fread(p_boot_record, sizeof(struct boot_record), 1, p_file);
	printf("-------------------------------------------------------- \n");
	printf("BOOT RECORD \n");
	printf("-------------------------------------------------------- \n");
	printf("volume_descriptor :%s\n", p_boot_record->volume_descriptor);
	printf("fat_type :%d\n", p_boot_record->fat_type);
	printf("fat_copies :%d\n", p_boot_record->fat_copies);
	printf("cluster_size :%d\n", p_boot_record->cluster_size);
	printf("root_directory_max_entries_count :%ld\n", p_boot_record->root_directory_max_entries_count);
	printf("cluster count :%d\n", p_boot_record->cluster_count);
	printf("reserved clusters :%d\n", p_boot_record->reserved_cluster_count);
	printf("signature :%s\n", p_boot_record->signature);
	
	//prectu fat_copies krat 
	printf("-------------------------------------------------------- \n");
	printf("FAT \n");
	printf("-------------------------------------------------------- \n");
	int64_t fat_items = p_boot_record->cluster_count;
	int64_t l;

	uint32_t *fat_item;
	fat_item = (uint32_t *)malloc(sizeof(uint32_t));
	int j;
	for (j = 0; j < p_boot_record->fat_copies; j++)
	{
		printf("\nFAT KOPIE %d\n", j + 1);
		for (l = 0; l < p_boot_record->cluster_count; l++)
		{
			fread(fat_item, sizeof(uint32_t), 1, p_file);

			if (*fat_item != FAT_UNUSED)
			{
				if (*fat_item == FAT_FILE_END)
					printf("%d - FILE_END\n", l);
				else if (*fat_item == FAT_BAD_CLUSTER)
					printf("%d - BAD_CLUSTER\n", l);
				else{
					//printf("%d - %d\n", l, *fat_item);
					printf("%d - ", l);
					printf("%d\n", *fat_item);
				}
			}
		}
	}
	
	//prectu root tolikrat polik je maximalni pocet zaznamu v bootu - root_directory_max_entries_count        
	printf("-------------------------------------------------------- \n");
	printf("ROOT DIRECTORY \n");
	printf("-------------------------------------------------------- \n");


	for (i = 0; i < p_boot_record->root_directory_max_entries_count; i++)
	{
		fread(p_root_directory, sizeof(struct root_directory), 1, p_file);
		printf("FILE %d \n", i);
		printf("file_name :%s\n", p_root_directory->file_name);
		printf("file_mod :%s\n", p_root_directory->file_mod);
		printf("file_type :%d\n", p_root_directory->file_type);
		printf("file_size :%d\n", p_root_directory->file_size);
		printf("first_cluster :%d\n", p_root_directory->first_cluster);
	}
	
	printf("-------------------------------------------------------- \n");
	printf("CLUSTERY - OBSAH \n");
	printf("-------------------------------------------------------- \n");

	char *p_cluster = (char *)malloc(sizeof(char) * p_boot_record->cluster_size);
	for (i = 0; i < p_boot_record->cluster_count + p_boot_record->reserved_cluster_count; i++)
	{
		fread(p_cluster, sizeof(char) * p_boot_record->cluster_size, 1, p_file);
		//pokud je prazdny (tedy zacina 0, tak nevypisuj obsah)
		if (p_cluster[0] != '\0')
			printf("Cluster %d:%s\n", i, p_cluster);
	}

	//uklid
	free(p_cluster);
	free(p_root_directory);
	free(p_boot_record);
	fclose(p_file);
}