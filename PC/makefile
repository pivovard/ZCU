CC = gcc
CFLAGS = -Wall -pedantic -ansi
BIN = classify.exe
OBJ = classify.o load.o struct.o
%.o: %.c
	$(CC) -c $(CFLAGS) $< -o $@

$(BIN): $(OBJ)
	$(CC) -lm $^ -o $@

