#pragma once

struct fat * read(char *file_name);
void write(char *file_name, struct fat *p_fat);
void read_default(char *file_name);