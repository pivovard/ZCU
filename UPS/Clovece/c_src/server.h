
#ifndef SERVER_H
#define SERVER_H
void pripoj(hrac_t *hrac, hra_t *hra);
char* generuj_ID();
int najdi_ID(char *id);
void *recieving(void *arg);
void *serve_request(void *arg);
void odesli(int socket, char *zprava);
void broadcast(char *zprava);
void * serve_accepts(void *arg);
int main (int argv, char *args[]);
void zpracuj_zadost(char *zprava);
void spatna_zprava(char *zprava);
char **rozdel(char *zprava);
void odpoj(hrac_t *hrac, hra_t *hra);

#endif
