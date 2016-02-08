#include "stdafx.h"
#include "check.h"

struct fat *q_fat;
std::mutex mtx;
int file = 0;

//porovna velikost souboru v clusterech a v root directory - vlakno
void compare(int id)
{
	int64_t size;
	struct root_directory *p_root_directory;
	uint32_t *fat_item;
	uint32_t fat_item_index;
	
	//vsechny soubory
	while (file < q_fat->p_boot_record->root_directory_max_entries_count){
		//kriticka sekce - kazda polozka bude pridelena pouze jednomu vlaknu
		mtx.lock();
		p_root_directory = &q_fat->root_directory_table[file];
		file++;
		mtx.unlock();

		size = 0;
		
		//prvni cluster
		fat_item_index = p_root_directory->first_cluster;
		fat_item = &q_fat->fat_table[fat_item_index];

		//vsechny clustery souboru
		while (*fat_item != FAT_FILE_END){
			size += q_fat->p_boot_record->cluster_size;
			fat_item_index = *fat_item;
			fat_item = &q_fat->fat_table[fat_item_index];
		}
		
		//velikost posledniho clusteru
		uint32_t cl = fat_item_index * q_fat->p_boot_record->cluster_size;
		for (int i = 0; i < q_fat->p_boot_record->cluster_size; i++){
			size += sizeof(char);
			if (q_fat->cluster_table[cl + i] == '\0'){
				//size -= sizeof(char);
				break;
			}
		}

		cout << "#" << id << " ";
		cout << "Soubor" << p_root_directory->file_name <<  ": ";
		cout << "velikost v root je " << p_root_directory->file_size << "B, ";
		cout << "skutecna velikost je " << size << "B" << endl;
	}
}

//porovna velikost souboru v clusterech a v root directory - spusteni
void check(char *input_file, int thread_count)
{
	cout << "Kontrola, zda kazdy retez FAT ma spravnou delku." << endl;
	
	//cteni dat
	q_fat = read(input_file);
	
	//spusteni vlaken
	std::thread *thread = new std::thread[thread_count];
	for (int i = 0; i < thread_count; i++){
		cout << "Spoustim vlakno #" << i << endl;
		thread[i] = std::thread(compare, i);
	}

	//cekani na dokonceni vsech vlaken
	for (int i = 0; i < thread_count; i++){
		thread[i].join();
	}

	free(q_fat);

	cout << "Konec kontroly." << endl << endl;
}