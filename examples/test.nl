cimport stdio.h;

func main(int argc, (arr)string argv) -> int {
    (arr)string hello_world = { "Hello", "World" };

    foreach (string s in hello_world) {
        printf("%s\n", s);
    }

    return 0;
}