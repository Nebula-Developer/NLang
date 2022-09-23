#define string char*
#define print printf
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, string **argv) {
    string s[] = { "hello", "world" };
    char* p = s[0];
    memcpy(p, s[1], sizeof(s[1]));
    printf("%s\n", p);
    return 0;
}
