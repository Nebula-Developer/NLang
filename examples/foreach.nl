cimport stdio.h;

func main(int argc, (arr)string argv) -> int {
    (arr)char c = { 'a', 'b', 'c', '\n' };

    foreach (char a in c)
        printf("%c", a);

    (arr)string hello = { "Hello ", "world!\n" };
    foreach (string s in hello)
        printf("%s", s);

    return 0;
}