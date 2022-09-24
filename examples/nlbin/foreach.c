#define false 0
#define true 1
#define bool int
#define string char*
#define print printf
#include <stdio.h>

int main(int argc, string argv[]) {
    char c[] = { 'a', 'b', 'c', '\n' };

    for (int i = 0; i < sizeof(c)/sizeof(c[0]); i++) { char a = c[i]; printf("%c", a); }

    string hello[] = { "Hello ", "world!\n" };
    for (int i = 0; i < sizeof(hello)/sizeof(hello[0]); i++) { string s = hello[i]; printf("%s", s); }

    return 0;
}
