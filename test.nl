cimport stdio.h;
cimport stdlib.h;
cimport string.h;

func main(int argc, (arr)string argv) -> int {
    (arr)string s = [ "hello", "world" ];
    (ptr)char p = s[0];
    p = s[1];
    (arr)(zero)char z;
    printf("%s\n", z);
    printf("%s\n", p);
    return 0;
}