JC=javac

all: client jar move del

client:
	${JC} *.java

jar:
	echo Main-class: App > manifest.txt
	jar cvfm Pong.jar manifest.txt *.class

move:
	mv *.class ../java_bin/
	mv Pong.jar ../

del:
	rm -f manifest.txt

clean: clear

clear:
	rm -Rf ../java_bin/*
	rm -f ../Pong.jar