all: client server

client:
	cd java_src && make

server:
	cd c_src && make

clean: clear

clear:
	cd c_src && make clear
	cd java_src && make clear