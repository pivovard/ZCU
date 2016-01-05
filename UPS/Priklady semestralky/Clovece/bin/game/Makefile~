CC=gcc

all:	clean servery java

client:	client.c
	${CC} -o client client.c

servery: servery.c stats.c
	${CC} stats.c clovece.c servery.c hra.c hrac.c -o server -lpthread -Wall


java:
	javac *.java

clean: 
	rm -f client
	rm -f server
	rm -f *.class
	rm -f *.*~

