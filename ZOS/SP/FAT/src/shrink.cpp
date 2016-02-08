#include "stdafx.h"
#include "shrink.h"

struct fat *p_fat;
uint32_t real_cluster_count;

//zamek clusteru (a fat tabulky)
std::mutex *mtx;
//zamek root directory
std::mutex mtx_root;

void make_shrink(int id);
void make_shrink_ordered(int id);


//setrese volne misto ve fat - spusteni
void shrink(char *input_file, char *output_file, int thread_count, bool ordered)
{
	cout << "Setreseni volneho mista." << endl;

	//vybere fci podle rezimu - zachovani relativniho poradi clusteru
	void(*shrink_t)(int);
	if (!ordered){
		shrink_t = make_shrink;
	}
	else{
		shrink_t = make_shrink_ordered;
	}

	//cteni dat
	p_fat = read(input_file);
	real_cluster_count = p_fat->p_boot_record->cluster_count + p_fat->p_boot_record->reserved_cluster_count;

	cout << "Zacinam setreseni volneho mista." << endl;

	//spusteni vlaken
	std::thread *thread = new std::thread[thread_count];
	mtx = new std::mutex[real_cluster_count];
	for (int i = 0; i < thread_count; i++){
		cout << "Spoustim vlakno #" << i << endl;
		thread[i] = std::thread(*shrink_t, i);
	}

	//cekani na dokonceni vlaken
	for (int i = 0; i < thread_count; i++){
		thread[i].join();
	}

	//kontrolni projeti, aby nezustaly mezery
	make_shrink(99);

	cout << "Setreseni volneho mista hotovo." << endl;

	//zapis dat
	write(output_file, p_fat);

	free(p_fat);

	cout << "Konec setreseni volneho mista." << endl << endl;
}

//setrese volne misto ve fat - vlakno
void make_shrink(int id)
{
	//velikost clusteru
	uint32_t cluster_size = p_fat->p_boot_record->cluster_size;

	//prazdny cluster - odpovida FAT_UNUSED
	char *p_cluster_null = (char *)malloc(sizeof(char) * cluster_size);
	p_cluster_null[0] = '\0';
	for (int i = 1; i < cluster_size; i++){
		p_cluster_null[i] = '0';
	}

	//hledani mezery
	for (int gap = 0; gap < real_cluster_count; gap++){
		if (p_fat->fat_table[gap] == FAT_UNUSED){
			
			//zamek na volne misto
			if (!mtx[gap].try_lock()){
				continue;
			}

			//hledani clusteru na vyplneni mezery
			//for (int sub = gap+1; sub < p_fat->p_boot_record->cluster_count; sub++){
			for (int sub = real_cluster_count - 1; sub > gap; sub--){
				if (p_fat->fat_table[sub] != FAT_UNUSED){
					
					//zamek na kopirovany cluster
					if (!mtx[sub].try_lock()){
						continue;
					}

					cout << "#" << id << " Kopiruji cluster " << sub << " do mezery " << gap << endl;

					//prekopirovani clusteru
					p_fat->fat_table[gap] = p_fat->fat_table[sub];
					//p_fat->cluster_table[space] = p_fat->cluster_table[sub];
					strncpy(&p_fat->cluster_table[gap*cluster_size], &p_fat->cluster_table[sub*cluster_size], sizeof(char) * cluster_size);

					//zkopirovani mezery na misto puvodniho prvku
					p_fat->fat_table[sub] = FAT_UNUSED;
					strncpy(&p_fat->cluster_table[sub*cluster_size], p_cluster_null, sizeof(char) * cluster_size);

					//aktualizace FAT tabulky - najde referenci na puvodni cluster
					for (int ref = 0; ref < real_cluster_count; ref++){
						if (p_fat->fat_table[ref] == sub){
							p_fat->fat_table[ref] = gap;
							break;
						}
					}


					//zamknuti root directory
					mtx_root.lock();

					//aktualizace root directory - najde referenci na puvodni cluster
					struct root_directory *p_root_directory;
					for (int ref = 0; ref < p_fat->p_boot_record->root_directory_max_entries_count; ref++){
						p_root_directory = &p_fat->root_directory_table[ref];
						if (p_root_directory->first_cluster == sub){
							p_root_directory->first_cluster = gap;
							break;
						}
					}

					//odemknuti root directory
					mtx_root.unlock();

					//odemknuti kopirovaneho clusteru
					mtx[sub].unlock();

					break;
				}
			}

			//odemknuti volneho mista
			mtx[gap].unlock();
		}
	}

	
}

//setrese volne misto ve fat (zachova relativni poradi clusteru) - vlakno
void make_shrink_ordered(int id)
{
	//velikost clusteru
	uint32_t cluster_size = p_fat->p_boot_record->cluster_size;

	//prazdny cluster - odpovida FAT_UNUSED
	char *p_cluster_null = (char *)malloc(sizeof(char) * cluster_size);
	p_cluster_null[0] = '\0';
	for (int i = 1; i < cluster_size; i++){
		p_cluster_null[i] = '0';
	}

	//hledani mezery
	for (int gap = 0; gap < real_cluster_count; gap++){
		if (p_fat->fat_table[gap] == FAT_UNUSED){

			//zamek na volne misto
			if (!mtx[gap].try_lock()){
				continue;
			}

			//hledani clusteru na vyplneni mezery
			for (int sub = gap+1; sub < p_fat->p_boot_record->cluster_count; sub++){
			//for (int sub = real_cluster_count - 1; sub > gap; sub--){
				if (p_fat->fat_table[sub] != FAT_UNUSED){

					//zamek na kopirovany cluster
					if (!mtx[sub].try_lock()){
						continue;
					}

					cout << "#" << id << " Kopiruji cluster " << sub << " do mezery " << gap << endl;

					//prekopirovani clusteru
					p_fat->fat_table[gap] = p_fat->fat_table[sub];
					//p_fat->cluster_table[space] = p_fat->cluster_table[sub];
					strncpy(&p_fat->cluster_table[gap*cluster_size], &p_fat->cluster_table[sub*cluster_size], sizeof(char) * cluster_size);

					//zkopirovani mezery na misto puvodniho prvku
					p_fat->fat_table[sub] = FAT_UNUSED;
					strncpy(&p_fat->cluster_table[sub*cluster_size], p_cluster_null, sizeof(char) * cluster_size);

					//aktualizace FAT tabulky - najde referenci na puvodni cluster
					for (int ref = 0; ref < real_cluster_count; ref++){
						if (p_fat->fat_table[ref] == sub){
							p_fat->fat_table[ref] = gap;
							break;
						}
					}

					//zamknuti root directory
					mtx_root.lock();

					//aktualizace root directory - najde referenci na puvodni cluster
					struct root_directory *p_root_directory;
					for (int ref = 0; ref < p_fat->p_boot_record->root_directory_max_entries_count; ref++){
						p_root_directory = &p_fat->root_directory_table[ref];
						if (p_root_directory->first_cluster == sub){
							p_root_directory->first_cluster = gap;
							break;
						}
					}

					//odemknuti root directory
					mtx_root.unlock();

					//odemknuti kopirovaneho clusteru
					mtx[sub].unlock();

					break;
				}
			}

			//odemknuti volneho mista
			mtx[gap].unlock();
		}
	}


}