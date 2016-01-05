#ifndef HRAC_H
#define HRAC_H

void inicializuj_barvy();
void uvolni_barvy();
void zrus_hrace(hrac_t *hrac);
void vymaz_hrace(hrac_t *hrac);
hrac_t *najdi_hrace(char *id);
void pridej_hrace(hrac_t *hrac);
void udelej_figurky(hrac_t *hrac);
hrac_t *vytvor_hrace(int socket, char *jmeno);
void uklid_hrace();
void vypis_hrace();

#endif
