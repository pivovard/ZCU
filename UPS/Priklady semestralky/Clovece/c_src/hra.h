#ifndef HRA_H
#define HRA_H

hra_t *najdi_hru(char *id);
void info_hry();
void vytvor_hru(hrac_t *hrac);
void zarad_hru(hra_t *hra);
void zrus_hru(hra_t *hra);
void vymaz_hru(hra_t *hra);
void multicast_hra(hra_t *hra, char *zprava);
void uklid_hry();
void vypis_hry();

#endif
