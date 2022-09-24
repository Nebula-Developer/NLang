#define false 0
#define true 1
#define bool int
#define string char*
#define print printf
#include <stdio.h>

int main(int argc, string argv[]) {
    string hello_world[] = { "Hello", "World" };

    for (int i = 0; i < sizeof(hello_world)/sizeof(hello_world[0]); i++) { string s = hello_world[i];
        printf("%s\n", s);
    }

    return 1;
}
