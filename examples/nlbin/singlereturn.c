#define false 0
#define true 1
#define bool int
#define string char*
#define print printf
#include <stdio.h>

int math_add(int a, int b) { return a + b; }

int main(int argc, string argv[]) {
    printf("%d\n", math_add(1, 2));
}
