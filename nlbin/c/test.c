#define string char*
#define print printf
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, string argv[]) {
    memset(&argv[2], 0, sizeof(argv[2]));
    
    for (int i = 0; i < argc; i++)
        print("%s\n", argv[i]);

    if (strcmp(argv[1], "hello") == 0&&strcmp(argv[1], argv[3]) == 0) {
        printf("Same\n");
    }

    return 0;
}
