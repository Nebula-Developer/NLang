#define false 0
#define true 1
#define bool int
#define string char*
#define print printf
#include <stdio.h>

void print_hello() {
    printf("Hello from imported C function\n");
    return;
}


int main(int argc, string argv[]) {
    print_hello();
    return 0;
}
