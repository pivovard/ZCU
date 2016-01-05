CC=gcc

all:	clean servery

servery: servery.c stats.c hra.c hrac.c
	${CC} stats.c clovece.c servery.c hra.c hrac.c -o server -lpthread -Wall


java:
	javac *.java

clean: 
	rm -f server
	rm -f *.*~


