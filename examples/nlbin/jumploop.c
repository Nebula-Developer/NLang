#define false 0
#define true 1
#define bool int
#define string char*
#define print printf
#include <stdio.h>

int main(int argc, string argv[]) {
    int iteration = 0;
    JUMP:

    printf("This is an iteration loop using the 'jump' keyword! - %d\n", iteration);
    iteration++;

    if (iteration > 10) return 0;

    goto JUMP; // This is an alias for the 'goto' keyword in C/C++.

    // This is unreachable code:
    return -1;
}
