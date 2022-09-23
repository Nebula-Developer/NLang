#define false 0
#define true 1
#define bool int
#define string char*
#define print printf
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, string argv[]) {

    if (strcmp(argv[1], "hello") == 0 && strcmp(argv[2], "world") == 0 && strcmp(argv[3], "machine") == 0) {
        printf("Hi\n");
    } else {
        printf("Bye\n");
    };

    return 0;
}
