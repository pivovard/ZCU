#ifndef CLOVECE_H
#define CLOVECE_H

int hod_kostkou(hra_t *hra);
void posli_info_hraci(hra_t *hra);
int zkontroluj_tah(hra_t *hra, int figurka_index);
void tahni(hra_t *hra, int figurka_index);
void vyhod(hra_t *hra, figurka_t *figurka);
int muze_hrat(hra_t *hra);
void zkontroluj_konec(hra_t *hra);
void proved_posun(hra_t *hra, int figurka_index, int policko_index);
void info_stav(hra_t *hra);
void vynuluj_figurky(hrac_t *hrac);
void *serve_hra(void *arg);
void dalsi_hrac(hra_t *hra);

#endif

