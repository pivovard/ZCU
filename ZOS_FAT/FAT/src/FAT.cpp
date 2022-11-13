// FAT.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

void help();

int main(int argc, char* argv[])
{
	//test poctu vstupnich parametru
	if (argc < 3 || argc > 5){
		cout << "Nespravny pocet argumentu!" << endl;
		help();
		return 1;
	}

	//test prvnich dvou argumentu na cisla
	if (!isdigit(*argv[1])){
		cout << "Vyber rezimu musi byt cislo!" << endl;
		help();
		return 1;
	}

	if (!isdigit(*argv[2])){
		cout << "Vyber poctu vlaken musi byt cislo!" << endl;
		help();
		return 1;
	}

	int rezim = atoi(argv[1]);
	int thread_count = atoi(argv[2]);
	
	//defaultni soubory
	char *input_file = INPUT_FILE;
	char *output_file = OUTPUT_FILE;

	if (argc > 3) {
		input_file = argv[3];
	}

	if (argc > 4) {
		input_file = argv[4];
	}

	//input_file = "bigfat.fat";
	//output_file = "bigfat.out.fat";

	//input_file = "test2.fat";
	//output_file = "test2.out.fat";

	time_t t1 = clock();

	//vyber rezimu a jeho spusteni
	switch (rezim){
	case 1: check(input_file, thread_count); break;
	case 2: shrink(input_file, output_file, thread_count, false); break;
	case 3: shrink(input_file, output_file, thread_count, true); break;
	default: cout << "Spatne zadany rezim!" << endl; help(); break;
	}

	time_t t2 = clock();
	float diff((float)t2 - (float)t1);

	//buffer vystupnich casu
	std::ofstream out;
	out.open("out.txt", std::fstream::out | std::fstream::app);
	out << "Rezim " << rezim << ", vlaken " << thread_count << ": ";
	out << diff << " ms" << endl;

	//read_default(input_file);
	//read_default(output_file);

	return 0;
}

//napoveda vstupu programu
void help()
{
	cout << "Povinne vstupy:" << endl;
	cout << "#1 : rezim > 1 - kontola velikosti souboru, 2 - setreseni volneho mista, 3 - setreseni volneho mista s relativnim zachovanim poradi" << endl;
	cout << "#2 : pocet vlaken" << endl;
	cout << "Volitelne vstupy:" << endl;
	cout << "#3 : vstupni soubor" << endl;
	cout << "#4 : vystupni soubor" << endl;
	cout << endl;
}