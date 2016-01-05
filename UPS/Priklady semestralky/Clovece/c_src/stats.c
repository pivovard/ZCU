#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

time_t poc_cas, akt_cas;
long prijatych_bajtu = 0;
long odeslanych_bajtu = 0;
long prijatych_zprav = 0;
long odeslanych_zprav = 0;
long spatnych_zprav = 0;

/*
___________________________________________________________

	Vypise statistiky serveru na zadany vystup
___________________________________________________________
*/
void vypis_info(FILE *stream)
{
	time(&akt_cas);
	long sekundy = difftime(akt_cas, poc_cas);
	int minuty = sekundy / 60;
	int hodiny = minuty / 60;
	fprintf(stream, "================= Statistiky serveru ===================\n");
	fprintf(stream, "Server spusten %s\n", asctime(localtime(&poc_cas)));
	fprintf(stream, "Bezi %d hodin(y) %d minut(y) %ld sekund(y)\n", hodiny, minuty, sekundy%60);	
	fprintf(stream, "Pocet prijatych zprav %ld\n", prijatych_zprav);
	fprintf(stream, "Pocet prijatych bajtu %ld\n", prijatych_bajtu);
	fprintf(stream, "Pocet odeslanych zprav %ld\n", odeslanych_zprav);
	fprintf(stream, "Pocet odeslanych bajtu %ld\n", odeslanych_bajtu);
	fprintf(stream, "Pocet prijatych zprav neodpovidajiciho formatu %ld\n", spatnych_zprav);
	fprintf(stream, "========================================================\n");
}


/*
___________________________________________________________

		Zapise zpravu do logs.log a vypise na obrazovku
___________________________________________________________
*/
void zapis_log(char *log)
{
	FILE *logs = fopen("logs.log", "a");
	fprintf(logs, "%s\t%s", asctime(localtime(&poc_cas)), log);
	fprintf(stdout, "%s", log);
	fclose(logs);
}


/*
___________________________________________________________

	Zapise statistiky do souboru stats.txt
___________________________________________________________
*/
void zapis_stats()
{
	FILE *f = fopen("stats.txt", "w");
	vypis_info(f);
	fclose(f);
}
