#define false 0
#define true 1
#define bool int
#define string char*
#define print printf
#include <stdio.h>
#include <string.h>

int main(int argc, string argv[]) {
    for (int i = 0; i < argc; i++) {
        printf("%s\n", argv[i]);
    }

    if (strcmp(argv[1], "hello") == 0 && strcmp(argv[2], "world") == 0) {
        printf("Hello, world!\n");
    } else {
        printf("Try supplying 'hello world' as arguments.\n");
    }
    return 0;
}
