#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <pthread.h>
#include <time.h>
#include <unistd.h>
#include <errno.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/un.h>
#include <sys/ioctl.h>
#include <arpa/inet.h>
#include <netinet/in.h>
#include <netinet/in.h>
#include <netdb.h>
#include <net/if.h>
#include <signal.h>


/*****************************************
 	constants
 *****************************************/
#define		BUFFSIZE	1000
/* Basic game variable */
#define		MAX_PADDLE_MOVE 15 /* O kolik pixelu se muze hracovo palka maximalne posunout behem "1 tahu" */


/* special data type */
typedef enum {
	FALSE, TRUE
}BOOLEAN;


/*****************************************
 	global variables
 *****************************************/
int server_socket;		/* serverovy socket */
pthread_t cli_listener;	/* vlakno, ktere prijima prikazy z terminalu */
int last_game_id = 1;	/* ID posledni zalozene hry */
int port = 10000;		/* Port na kterem server posloucha */
int time_to_wait = 30;	/* Jak dlouho bude server vyckavat na pripojeni druheho hrace */

int game_width = 800;	/* sirka herniho okna */
int game_height = 450;	/* vyska herniho okna */
int board_width = 10;	/* sirka palky */
int board_height = 65;	/* vyska palky */
int board_distance = 30;/* zdalenost palky od okraje okna */
int ball_size = 10;		/* velikost micku (sirka i vyska) */
int win_score = 3;


char *cli_delim = "----------------------------------------";
long log_msg_send = 0;		/* pocet celkove odeslanych zprav */
long log_msg_recv = 0;		/* pocet celkove prijatych zprav */
long log_byte_send = 0;		/* pocet celkove odeslanych bytu */
long log_byte_recv = 0;		/* pocet celkove prijatch bytu */
int current_client = 0;		/* cislo aktualne prijeneho klienta = pocet celkove pripojenych clientu */

FILE *log_games, *log_server;		/* log soubory */
time_t time_start, time_close;		/* promenne pro mereni casu */
int client_socket_buf[2] = {0, 0};	/* buffer pro 2 klientske sockety */
struct Game *allgames;				/* seznam vsech her */
pthread_t time_checker, recv_checker;	/* kontrolni vlakna */


/*****************************************
 	head of functions
 *****************************************/
void sigint_handler(int sig);
void log_server_init();
void *connectionChecker(void *arg);
void *receiveChecker(void *arg);
int pos_neg();
void *receiver(void *arg);
void resetBall(struct Game *hra);
void *ballHandler(void *arg);
void *cli_listen(void *arg);
void get_help();


/*****************************************
 	stuctures
 *****************************************/

/* Struktura obsahujici informace o jedne hre */
typedef struct Game {
	/* Basic information about game */
	int game_id, ball_x ,ball_y;
	int h_dir, v_dir; /* Horizontal (x axis) & vertical (y axis) direction */
	/* Information about Player 1 */
	int p1_socket, p1_paddle_y, p1_paddle_x, p1_score;
	char p1_nick[96];
	/* Information about Player 2 */
	int p2_socket, p2_paddle_y, p2_paddle_x, p2_score;
	char p2_nick[96];
	/* Next element of linked list*/
	struct Game *next;
}game;


/* Struktura pro predani dvou parametru do vlakna receiver */
typedef struct T_param {
	game *game;
	int socket;
	pthread_t *t_ball;
}t_param;

/* Struktura pro predani parametru do kontrolnich vlaken */
typedef struct Param_checker {
	int socket;
}param_checker;

/****************************************************
 	functon for manipulation with game structure
 ****************************************************/

/* tisk seznamu her */
void print_games(game *head) {
	while(head) {
		printf("GAME #%2d: %s vs. %s (%2d:%2d)\n", head->game_id, head->p1_nick, head->p2_nick, head->p1_score, head->p2_score);
		head = head->next;
	}
	if(!head){
		printf("Zadna hra neni spustena\n");
	}
}

/* pridani nove hry do seznamu her */
game *add_game_full(game **head, int p1_socket, int p2_socket, char *p1_nick, char *p2_nick) {
	game *newgame, *p;

	/* inicializace nove hry */
	newgame = malloc(sizeof(game));		/* alokace pameti pro strukturu */
	strcpy(newgame->p1_nick, p1_nick);	/* prezdivka 1. hrace */
	strcpy(newgame->p2_nick, p2_nick);	/* prezdivka 2. hrace */
	newgame->game_id = last_game_id; 	/* ID hry */
	last_game_id++;
	newgame->p1_socket = p1_socket;     /* socket 1. hrace */
	newgame->p2_socket = p2_socket;     /* socket 2. hrace */
	newgame->p1_paddle_y = 0;			/* pozice pálky 1. hrace */
	newgame->p2_paddle_y = 0;           /* pozice pálky 2. hrace */
	newgame->ball_x = 0;                /* X-ova pozice micku */
	newgame->ball_y = 0;                /* Y-ova pozice micku */
	newgame->p1_score = 0;              /* skore 1. hrace */
	newgame->p2_score = 0;              /* skore 2. hrace */

	/* zarazeni hry do seznamu her */
	if (*head) {
		p = *head;
		while (p->next) p = p->next;
		p->next = newgame;
	}
	else {
		*head = newgame;
	}

	return newgame;
}

/* pridani nove hry do seznamu her */
game *add_game(game **head, int p1_socket, int p2_socket) {
	game *newgame, *p;

	/* inicializace nove hry */
	newgame = malloc(sizeof(game));		/* alokace pameti pro strukturu */
	newgame->game_id = last_game_id; 	/* ID hry */
	last_game_id++;
	newgame->p1_socket = p1_socket;     /* socket 1. hrace */
	newgame->p2_socket = p2_socket;     /* socket 2. hrace */
	newgame->p1_paddle_y = 0;			/* pozice pálky 1. hrace */
	newgame->p2_paddle_y = 0;           /* pozice pálky 2. hrace */
	newgame->ball_x = 0;                /* X-ova pozice micku */
	newgame->ball_y = 0;                /* Y-ova pozice micku */
	newgame->p1_score = 0;              /* skore 1. hrace */
	newgame->p2_score = 0;              /* skore 2. hrace */
	newgame->h_dir = 1;					/* horizontalni (osa x) smer micku */
	newgame->v_dir = 1;					/* vertikalni (osa y) smer micku */
	newgame->p1_paddle_x = board_distance + 20;
	newgame->p2_paddle_x = game_width - board_distance + board_width;

	/* zarazeni hry do seznamu her */
	if (*head) {
		p = *head;
		while (p->next) p = p->next;
		p->next = newgame;
	}
	else {
		*head = newgame;
	}
	return newgame;
}

/* oderani hry (remove head) */
void list_remove (game **p){
	if (p && *p) {
		game *n = *p;
		*p = (*p)->next;
		close(n->p1_socket);
		close(n->p2_socket);
		free(n);
	}
}
/* list_remove(list_search(&games, 5));  => odstrani hru s ID = 5 */

/* nalezeni hry podle jejiho id (game_id) */
game **list_search (game **n, int id){
	if (n) {
		while (*n) {
			if ((*n)->game_id == id) return n;
			n = &(*n)->next;
		}
	}
	return NULL;
}

/*
 * Resetuje pozici micku a palek pro danou hru.
 * Nahodne urci smer micku.
 */
void resetBall(game *hra){
	hra->ball_x = game_width/2;
	hra->ball_y = (rand() % game_height)+1;
	hra->p1_paddle_y = hra->p2_paddle_y = (game_height - 65)/2;
	hra->h_dir = pos_neg();
	hra->v_dir = pos_neg();
	printf("Reset ball (%dx%d)\n",hra->ball_x,hra->ball_y);
}

/*
 * Vlakno, ktere slouzi pro poslouchani prichozich zprav od klientu.
 */
void *receiver(void *arg){
	/* pretypovani vstupnich parametru */
	t_param *param;
	param = (t_param *)arg;

	printf("GAME (RECV): Spusteno vlakno na obsluhu prichozich zprav pro hrace se socketem %d.\n", param->socket);

	/* ziskani hry z parametru */
	game *hra;
	hra = param->game;

	/* ziskani reference na t_ball */
	pthread_t *t_ball;
	t_ball = param->t_ball;

	/* ziskani socketu z parametru */
	int socket;
	socket = param->socket;

	/* nastaveni bufferu pro prijem */
	char cbuf[BUFFSIZE];
	bzero(cbuf,BUFFSIZE);
	char str[BUFFSIZE];
	bzero(str,BUFFSIZE);

	char* command;
	char* move;


	while(TRUE){
		if(recv(socket, &cbuf, BUFFSIZE*sizeof(char), 0) != 0){

			log_msg_recv++;
			log_byte_recv += strlen(cbuf);

			printf("GAME (RECV): Prijato od %d: %s\n", socket, cbuf);
			command = strtok(cbuf,";"); /*dostanu prvni slovo*/
			move = strtok(NULL, ";"); /*dostanu druhe slovo*/

			if(strcmp("move",command)==0 && move != NULL){
				if(hra->p1_socket == socket){ /*odehral hrac P1*/
					/*printf("Hrac 1 se pohnul %s\n", move);*/
					if(strcmp(move,"up")==0){ /*pohnul se nahoru*/
						if(hra->p1_paddle_y > 30)
							hra->p1_paddle_y = (hra->p1_paddle_y - 5);
					}
					else if (strcmp(move, "down")==0){ /*ppohnul se dolu*/
						if(hra->p1_paddle_y < 395)
							hra->p1_paddle_y = (hra->p1_paddle_y + 5);
					}
				}else if(hra->p2_socket == socket){
					/*printf("Hrac 2 se pohnul %s\n", move);*/
					if(strcmp(move,"up")==0){ /*pohnul se nahoru*/
						if(hra->p2_paddle_y > 30)
							hra->p2_paddle_y = (hra->p2_paddle_y - 5);
					}
					else if (strcmp(move, "down")==0){ /*ppohnul se dolu*/
						if(hra->p2_paddle_y < 395)
							hra->p2_paddle_y = (hra->p2_paddle_y + 5);
					}
				}
			}else if(strcmp("end",command)==0 || strcmp("kill",command)==0){

				bzero(cbuf,BUFFSIZE);
				usleep(10000); /* aby to JAVA stihla vstřebat */

				strcat(cbuf,"end;");
				if(hra->p1_socket == socket /*&& hra->p2_socket != 0*/){
					printf("Poslano end na %d\n", hra->p2_socket);
					send(hra->p2_socket,cbuf, strlen(cbuf)*sizeof(char),0);

					log_msg_send++;
					log_byte_send += strlen(cbuf)*sizeof(char);

				}else if(hra->p2_socket == socket /*&& hra->p1_socket != 0*/){
					printf("Poslano end na %d\n", hra->p1_socket);
					send(hra->p1_socket,cbuf, strlen(cbuf)*sizeof(char),0);

					log_msg_send++;
					log_byte_send += strlen(cbuf)*sizeof(char);

				}
				/*ukoncenim vlakna obsluhujiciho micek dojde k zavreni hry a tim i socketu*/
				pthread_cancel(*t_ball);
				break;
			}else {
				printf("GAME (RECV): Spatny prikaz!\n");
			}

			bzero(cbuf,BUFFSIZE);
		}else {
			pthread_cancel(*t_ball);
			bzero(cbuf,BUFFSIZE);
			strcat(cbuf,"end;");
			if(hra->p1_socket == socket){
				send(hra->p2_socket,cbuf, strlen(cbuf)*sizeof(char),0);

				log_msg_send++;
				log_byte_send += strlen(cbuf)*sizeof(char);

			}else if(hra->p2_socket == socket){
				send(hra->p1_socket,cbuf, strlen(cbuf)*sizeof(char),0);

				log_msg_send++;
				log_byte_send += strlen(cbuf)*sizeof(char);

			}
			break;
		}
	}
}

/*
 * Vlakno, ktere se stara o micek ve hre:
 * - pocita aktualni pozici micku
 * - kontroluje jeho odrazy micku
 * - kontroluje jestli padl gol
 */
void *ballHandler(void *arg){
	printf("GAME (BALL): Spusteno vlakno na obsluhu micku.\n");

	game *hra;
	hra = (game *)arg;
	char cbufIn[BUFFSIZE];
	bzero(cbufIn,BUFFSIZE);

	resetBall(hra);

	while(TRUE){
		char *num = (char *) malloc (sizeof(char)*2);

		usleep(10000);

		/* odeslani souradnic na oba klienty */
		bzero(cbufIn,BUFFSIZE);
		strcat(cbufIn,"coord;");

		sprintf(num, "%d;", hra->ball_x);
		strcat(cbufIn, num);

		sprintf(num, "%d;", hra->ball_y);
		strcat(cbufIn, num);

		sprintf(num, "%d;", hra->p1_paddle_y);
		strcat(cbufIn, num);

		sprintf(num, "%d;", hra->p2_paddle_y);
		strcat(cbufIn, num);

		sprintf(num, "%d;", hra->p1_score);
		strcat(cbufIn, num);

		sprintf(num, "%d;", hra->p2_score);
		strcat(cbufIn, num);

		send(hra->p1_socket,cbufIn, strlen(cbufIn)*sizeof(char),0);

		log_msg_send++;
		log_byte_send += strlen(cbufIn)*sizeof(char);

		send(hra->p2_socket,cbufIn, strlen(cbufIn)*sizeof(char),0);

		log_msg_send++;
		log_byte_send += strlen(cbufIn)*sizeof(char);



		/* pohyb micku */
		hra->ball_x += hra->h_dir;
		hra->ball_y += hra->v_dir;
		/*printf("ball (%dx%d), smer (x = %d, y = %d)\n",hra->ball_x,hra->ball_y, hra->h_dir, hra->v_dir);*/

		/* odrazeni micku od horni a dolni steny */
		if(hra->ball_y <= 30 || hra->ball_y+ball_size >= 440){
			hra->v_dir *= -1;
		}

		/* micek je za carou hrace P2 (prava palka) */
		if((hra->ball_x+ball_size >= hra->p2_paddle_x)){
			/* micek neni na palce */
			if((hra->ball_y > (hra->p2_paddle_y + board_height)) || ((hra->ball_y+ball_size) < hra->p2_paddle_y)){
				printf("Gol! Bod pro P1\n");
				resetBall(hra);
				hra->p1_score++;
			}else {
				hra->h_dir *= -1;
			}

		}/* micek je za carou hrace P1 (leva palka) */
		else if(hra->ball_x <= hra->p1_paddle_x){
			if((hra->ball_y > (hra->p1_paddle_y + board_height)) || ((hra->ball_y+ball_size) < hra->p1_paddle_y)){
				printf("Gol! Bod pro P2\n");
				resetBall(hra);
				hra->p2_score++;
			}else {
				hra->h_dir *= -1;
			}
		}

		/* The game is over one player wins */
		if(hra->p1_score == win_score){
			usleep(100000); /* aby to JAVA stihla vstřebat */
			/* P1 win */
			bzero(cbufIn,BUFFSIZE);
			strcat(cbufIn,"gameover;win;");
			send(hra->p1_socket,cbufIn, strlen(cbufIn)*sizeof(char),0);

			log_msg_send++;
			log_byte_send += strlen(cbufIn)*sizeof(char);

			/* P2 lose */
			bzero(cbufIn,BUFFSIZE);
			strcat(cbufIn,"gameover;lose;");
			send(hra->p2_socket,cbufIn, strlen(cbufIn)*sizeof(char),0);

			log_msg_send++;
			log_byte_send += strlen(cbufIn)*sizeof(char);

			pthread_exit(0);
		}else if(hra->p2_score == win_score){
			usleep(100000); /* aby to JAVA stihla vstřebat */
			/* P1 lose */
			bzero(cbufIn,BUFFSIZE);
			strcat(cbufIn,"gameover;lose;");
			send(hra->p1_socket,cbufIn, strlen(cbufIn)*sizeof(char),0);

			log_msg_send++;
			log_byte_send += strlen(cbufIn)*sizeof(char);

			/* P2 win */
			bzero(cbufIn,BUFFSIZE);
			strcat(cbufIn,"gameover;win;");
			send(hra->p2_socket,cbufIn, strlen(cbufIn)*sizeof(char),0);

			log_msg_send++;
			log_byte_send += strlen(cbufIn)*sizeof(char);

			pthread_exit(0);
		}

		free(num);
		num = NULL;


	}
}

/*
 * Vlakno, ktere se stara o vytvoreni nove hry (vzdy dva hraci).
 * Posle inicializacni zpravy a spusti ostatni pro hru dulezita vlakna.
 */
void *gameHandler(void *arg){
	printf("GAME: Spusteno vlakno na obsluhu hry.\n");

	game *hra;
	hra = (game *)arg;
	char cbufIn[BUFFSIZE];
	pthread_t t_ball, t_recv_one, t_recv_two;
	char *num = (char *) malloc (sizeof(char)*2);

	printf("GAME: Klienti se socketem %d %d\n", hra->p1_socket,hra->p2_socket);

	resetBall(hra);

	bzero(cbufIn,BUFFSIZE);
	strcat(cbufIn, "init;L;");

	/* pridani pocatecni pozice micku */
	sprintf(num, "%d;", hra->ball_x);
	strcat(cbufIn, num);
	sprintf(num, "%d;", hra->ball_y);
	strcat(cbufIn, num);

	/* pridani sirky a vysky herniho okna */
	sprintf(num, "%d;", game_width);
	strcat(cbufIn, num);
	sprintf(num, "%d;", game_height);
	strcat(cbufIn, num);

	send(hra->p1_socket,cbufIn, strlen(cbufIn)*sizeof(char),0);

	log_msg_send++;
	log_byte_send += strlen(cbufIn)*sizeof(char);


	/* totez pocatecni nastaveni pro druheho hrace */
	bzero(cbufIn,BUFFSIZE);
	strcat(cbufIn, "init;P;");
	sprintf(num, "%d;", hra->ball_x);
	strcat(cbufIn, num);
	sprintf(num, "%d;", hra->ball_y);
	strcat(cbufIn, num);
	sprintf(num, "%d;", game_width);
	strcat(cbufIn, num);
	sprintf(num, "%d;", game_height);
	strcat(cbufIn, num);
	send(hra->p2_socket,cbufIn, strlen(cbufIn)*sizeof(char),0);

	log_msg_send++;
	log_byte_send += strlen(cbufIn)*sizeof(char);


	t_param *param1, *param2;
	param1 = (t_param *)malloc(sizeof(t_param));
	param2 = (t_param *)malloc(sizeof(t_param));
	param1->game = param2->game = hra;
	param1->t_ball = param2->t_ball = &t_ball;

	param1->socket = hra->p1_socket;
	pthread_create(&t_recv_one, NULL, &receiver, (void *)param1);

	param2->socket = hra->p2_socket;
	pthread_create(&t_recv_two, NULL, &receiver, (void *)param2);

	pthread_create(&t_ball, NULL, &ballHandler, (void *)hra);

	/* kdyz skonci vlakno t_ball (ballHandler) tak ukoncim i ostatni veci */
	pthread_join(t_ball, NULL);
	pthread_cancel(t_recv_one);
	pthread_cancel(t_recv_two);
	printf("GAME: hra s id %d ukoncena.\n", hra->game_id);
	list_remove(list_search(&allgames, hra->game_id));
	printf("Zbyvajici hry:\n");
	print_games(allgames);
	pthread_exit(0);

}

/*
 * Zakladni funkce serveru.
 * Stara se nastaveni serveru, pripojovani klientu a vytvareni her.
 */
int main(int argc, char *argv[]) {
	/* system pro zpracovani parametru - lze neomezene pridavat dalsi */
	int i, j;
	i = argc;
	j = 1;
	while(i > 1){
		if(strcasecmp(argv[j],"-h") == 0 || strcasecmp(argv[j],"-help") == 0){
			get_help();
			exit(0);
		}
		else if((strcasecmp(argv[j],"-port") == 0 || strcasecmp(argv[j],"-p") == 0) && argv[j+1] != NULL){
			int pom;
			pom = atoi(argv[j+1]);
			if(pom > 1024 && pom <= 65535){
				port = pom;
				printf("Nastaven port na %d\n", port);
			}else {
				printf("Nastaveni portu je chybne! Zustava na %d\n", port);
			}
			i = i-2;
			j = j+2;
			continue;
		}
		else if((strcasecmp(argv[j],"-winscore") == 0 || strcasecmp(argv[j],"-s") == 0) && argv[j+1] != NULL){
			int pom;
			pom = atoi(argv[j+1]);
			if(pom > 2 && pom <= 60){
				win_score = pom;
				printf("Nastaven pocet bodu k vitezstvi na %d\n", win_score);
			}else {
				printf("Nastaveni bodu k vitezstvi je chybne! Zustava na %d\n", win_score);
			}
			i = i-2;
			j = j+2;
			continue;
		}
		else if((strcasecmp(argv[j],"-wait") == 0 || strcasecmp(argv[j],"-w") == 0) && argv[j+1] != NULL){
			int pom;
			pom = atoi(argv[j+1]);
			if(pom > 10 && pom < 300 || pom == 5){
				time_to_wait = pom;
				printf("Nastavena doba cekani na pripojeni druheho hrace na %d\n", time_to_wait);
			}else {
				printf("Nastaveni doby cekani na pripojeni druheho hrace je chybne! Zustava na %d\n", time_to_wait);
			}
			i = i-2;
			j = j+2;
			continue;
		}
		i--;
	}


	signal(SIGINT, sigint_handler);

	log_server_init();

	struct sockaddr_in my_addr;
	server_socket = socket(AF_INET, SOCK_STREAM, 0); /* inicializace server socketu */

	int flag = 1;
	/* zpusobi ze nenastane error bind v pripade padu serveru */
	setsockopt(server_socket, SOL_SOCKET, SO_REUSEADDR, &flag, sizeof(flag));

	memset(&my_addr, 0, sizeof(my_addr)); /* nulovani pameti */

	my_addr.sin_family = AF_INET;
	my_addr.sin_port = htons(port);
	my_addr.sin_addr.s_addr = INADDR_ANY;

	/* zjisteni lokalni IP adresy */
	struct ifreq ifr;
	ifr.ifr_addr.sa_family = AF_INET;
	strncpy(ifr.ifr_name, "eth0", IFNAMSIZ-1);
	ioctl(server_socket, SIOCGIFADDR, &ifr);
	char *local_ip = inet_ntoa(((struct sockaddr_in *)&ifr.ifr_addr)->sin_addr);

	/* zjisteni hostname */
	char hostname[BUFFSIZE];
	hostname[BUFFSIZE-1];
	gethostname(hostname, BUFFSIZE-1);

	/* zablokovani portu */
	if(bind(server_socket, (struct sockaddr *) &my_addr, sizeof(struct sockaddr_in)) == -1){
		printf("Bind - ERR\n");
		perror("Error: bind()");
		exit(1);
	}

	/* zahajeni naslouchani na portu */
	if (listen(server_socket, 5) == -1){
		printf("Listen - ERR\n");
		perror("Error: listen()");
		exit(1);
	}


	struct sockaddr_in client_addr;
	int client_addr_length;
	current_client = 0; /* cislo aktualniho klienta - inkrementuje je se od spusteni serveru */
	pthread_t hra; /* vlakno, ktere bude obsluhovat hru */
	game *hra_struct;


	param_checker *chck1, *chck2;
	chck1 = (param_checker *)malloc(sizeof(param_checker));
	chck2 = (param_checker *)malloc(sizeof(param_checker));

	printf("%s\n", cli_delim);
	printf("Server bezi na %s:%d nebo %s:%d\n", local_ip, port, hostname, port);
	printf("%s\n", cli_delim);


	pthread_create(&cli_listener, NULL, &cli_listen, NULL);

	while(TRUE){
		printf("SERVER: Cekam na prichozi spojeni...\n");
		client_addr_length = sizeof(client_addr);

		/* cekani na prichozi spojeji */
		if ((client_socket_buf[current_client % 2] = accept(server_socket, (struct sockaddr *)&client_addr, &client_addr_length)) == -1) {
			perror("SERVER: Chyba pri accept\n");
			//close(server_socket); exit(1);
		}else {
			current_client++;
			printf("SERVER: Nove prichozi spojeni %d.\n", current_client);
			printf("client_socket_buf[0] = %d\n", client_socket_buf[0]);
			printf("client_socket_buf[1] = %d\n", client_socket_buf[1]);

			if(client_socket_buf[1]==0 && client_socket_buf[0]!=0){ /* mam 1 hrace, cekam na druheho, kontroluji spojeni */
				chck1->socket = chck2->socket = client_socket_buf[0];
				pthread_create(&time_checker, NULL, &connectionChecker, (void *)chck1);
				pthread_create(&recv_checker, NULL, &receiveChecker, (void *)chck2);
			}else if(client_socket_buf[0]==0 && client_socket_buf[1]!=0){
				chck1->socket = chck2->socket = client_socket_buf[1];
				pthread_create(&time_checker, NULL, &connectionChecker, (void *)chck1);
				pthread_create(&recv_checker, NULL, &receiveChecker, (void *)chck2);
			}
		}

		/* Pokud uz jsou klienti 2, muzou zacit hrat */
		if(client_socket_buf[0] != 0 && client_socket_buf[1] != 0){
			/* ukoncim kontrolni vlakna */
			pthread_cancel(time_checker);
			pthread_cancel(recv_checker);

			printf("SERVER: Mam dva klienty (%d, %d)\n", client_socket_buf[0],client_socket_buf[1]);
			/*hra_struct = (game *)malloc(sizeof(game));
			hra_struct->p1_socket = client_socket_buf[0];
			hra_struct->p2_socket = client_socket_buf[1];*/
			hra_struct = add_game(&allgames,client_socket_buf[0],client_socket_buf[1]);
			print_games(allgames);

			if(pthread_create(&hra,NULL, &gameHandler ,(void *)hra_struct) != 0){
				printf("SERVER: Nemohl jsem vytvořit hru\n");
			}
			else{
				printf("SERVER: Vytvořena nová hra %d\n", hra_struct->game_id);
			}

			client_socket_buf[0] = 0;
			client_socket_buf[1] = 0;
		}
	}

	return 0;
}

/*
 * Pri aceptu jednoho hrace ceka server jen omeze dlouhou dobu na pripojeni druheho hrace,
 * po teto dobe prvniho hrace odpoji se zpravou ze se nepodarilo nalezt soupere.
 *
 * Doba cekani je urcena promenou time_to_wait
 */
void *connectionChecker(void *arg){
	/* pretypovani vstupnich parametru */
	param_checker *param;
	param = (param_checker *)arg;

	/* ziskani parametru */
	int socket;
	socket = param->socket;


	printf("CHECKER: Spusteno overovaci TIME vlakno pro socket %d.\n", socket);

	int a2read, fd;
	fd_set tests, client_socks;
	char cbuf[] = "fail;";

	struct timeval tv;
	tv.tv_sec = time_to_wait;
	tv.tv_usec = 0;


	FD_ZERO( &client_socks );
	FD_SET(socket, &client_socks);

	tests = client_socks;

	/* sada deskriptoru je po kazdem volani select prepsana sadou deskriptoru kde se neco delo */
	if (select(FD_SETSIZE, &client_socks, NULL, NULL, &tv) < 0) {
		printf("Select - ERR\n");
		exit(1);
	}
	/* vynechavame stdin, stdout, stderr */
	for( fd = 3; fd < FD_SETSIZE; fd++ ){
		if( FD_ISSET( fd, &tests ) ){
			ioctl( fd, FIONREAD, &a2read );
			if (a2read >= 0){
				send(socket, cbuf, strlen(cbuf)*sizeof(char),0);

				log_msg_send++;
				log_byte_send += strlen(cbuf)*sizeof(char);

				FD_ZERO(&client_socks);//
				client_socket_buf[0] = 0;
				client_socket_buf[1] = 0;
				printf("Zadny souper se nepripojil do %d sekund.\n", time_to_wait);
				pthread_cancel(recv_checker);

				/*
				if(client_socket_buf[0] != 0){
					close(client_socket_buf[0]);
					client_socket_buf[0] = 0;
				}else if(client_socket_buf[1] != 0){
					close(client_socket_buf[1]);
					client_socket_buf[1] = 0;
				}
				 */

				pthread_exit(0);
			}
		}
	}
}

/*
 * Vlakno posloucha zpravy od prvniho pripojeneho hrace a pokud posle end nebo kill,
 * tedy ze se prvni hrac odpojil, tak uzavre hlidaci vlakna a vynuluje sockety.
 */
void *receiveChecker(void *arg){
	/* pretypovani vstupnich parametru */
	param_checker *param;
	param = (param_checker *)arg;

	/* ziskani parametru */
	int socket;
	socket = param->socket;

	/* nastaveni bufferu pro prijem */
	char cbuf[BUFFSIZE];
	bzero(cbuf,BUFFSIZE);
	char* command;

	printf("CHECKER: Spusteno overovaci RECEIVE vlakno pro socket %d.\n", socket);

	while(TRUE){
		if(recv(socket, &cbuf, BUFFSIZE*sizeof(char), 0) != 0){

			log_msg_recv++;
			log_byte_recv += strlen(cbuf);

			printf("CHECKER (RECV): Prijato od %d: %s\n", socket, cbuf);
			command = strtok(cbuf,";"); /*dostanu prvni slovo*/
			printf("CHECKER (RECV): command = %s\n", command);

			/*puvodne: if(strcmp("move",command)==0 || strcmp("kill",command)==0){*/
			if(strcmp("end",command)==0 || strcmp("kill",command)==0){
				printf("CHECKER (RECV): ukoncuji hlidani\n");
				client_socket_buf[0] = 0;
				client_socket_buf[1] = 0;
				pthread_cancel(time_checker);
				pthread_exit(0);
			}
		}
	}
}

/*
 * Funkce vraci nahodne bud cislo 1 nebo -1
 */
int pos_neg(){
	int x;
	x = rand() % 2;
	if(x == 0) x = -1;
	return x;
}

/*
 * Funkce slouzi pro vytvoteni logu pro server
 */
void log_server_init(){
	time_t cas;
	struct tm *ltmcas;
	cas = time(NULL);
	ltmcas = localtime(&cas);

	time_start = time(NULL);
	log_server = fopen("server.log", "w");
	fclose(log_server);
	log_server = fopen("server.log", "a+");

	fprintf(log_server,"Server startuje v case %s a nasloucha na portu: %d\n", ctime(&cas), port);



	fclose(log_server);
}

void log_server_add(char *msg) {
	log_server = fopen("server.log", "a+");
	fprintf(log_server,"%s",msg);
	fclose(log_server);
}

void log_server_over(){
	time_t cas;
	struct tm *ltmcas;
	cas = time(NULL);
	ltmcas = localtime(&cas);
	int total_time;

	log_server = fopen("server.log", "a+");

	time_close = time(NULL);
	total_time = (int) difftime(time_close,time_start);

	fprintf(log_server,"Server byl ukoncen v case %s a bezel %d sekund.\n", ctime(&cas), total_time);
	fprintf(log_server,"%s\n",cli_delim);
	fprintf(log_server,"STATISTIKA\n");
	fprintf(log_server,"%s\n",cli_delim);
	fprintf(log_server, "Celkovy pocet odehranych her: %d\n", last_game_id-1);
	fprintf(log_server, "Celkovy pocet pripojenych klientu: %d\n", current_client);
	fprintf(log_server, "Celkovy pocet odeslanych zprav: %d\n", log_msg_send);
	fprintf(log_server, "Celkovy pocet odeslanych bytu: %d\n", log_byte_send);
	fprintf(log_server, "Celkovy pocet prijatych zprav: %d\n", log_msg_recv);
	fprintf(log_server, "Celkovy pocet prijatych bytu: %d\n", log_byte_recv);
}

/*
 * Funkce detekujici signaly, konketne ukonceni serveru klavesami CTRL+C
 */
void sigint_handler(int sig){
	log_server_add("Ukonceno pres CTRL+C\n");
	log_server_over();
	printf("\nSERVER: Detekovano stiknuti CTRL+C. Server ukoncen!\n");
	exit(1);
}

void *cli_listen(void *arg){
	int i;
	char command[BUFFSIZE];
	while(TRUE){
		scanf("%s",&command);
		if(strcasecmp(command, "exit")==0 || strcasecmp(command, "close")==0){
			log_server_add("Ukonceno pres prikaz serveru.\n");
			log_server_over();
			printf("\nSERVER: Ukonceno prikazem EXIT nebo CLOSE\n");
			/* TO-DO: uklid */
			exit(0);
		}
		else if(strcmp(command, "game")==0){
			printf("\nBezici hry:\n");
			print_games(allgames);
		}
		else if(strcmp(command, "help")==0){
			printf("EXIT (CLOSE): ukonci server\n");
			printf("GAME: vypise prave probihajici hry\n");
		}
		else{
			printf("Neznamy prikaz. Pouzijte HELP pro napovedu.\n");
		}

	}
}

/*
 * Vypsani napovedy k parametrum pro spousteni serveru
 */
void get_help(){
	printf("%s\n\tNAPOVEDA ke hre Pong\n%s\n", cli_delim, cli_delim);
	printf("Pong je jedna z nejstarsich her  \n");
	printf("-port || -p [cislo] \t\t nastavi port, na kterem bude server poslouchat (1024-65535).\n");
	printf("-winscore || -s [cislo] \t nastavi pocet bodu (golu) potrebnych pro vitezstvi (2 - 60).\n");
	printf("-wait || -w [cislo] \t\t nastavi koliv vterin bude server cekat na pripojeni druheho hrace (5, 10 - 300).\n");
}







