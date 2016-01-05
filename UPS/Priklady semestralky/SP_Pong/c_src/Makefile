CC=gcc

all: server move

server: pong_server.c
	$(CC) -o server pong_server.c -lpthread

move:
	mv server ../c_bin/

clean: clear

clear:
	rm -Rf ../c_bin/*