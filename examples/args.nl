cimport stdio.h;
cimport string.h;

func main(int argc, (arr)string argv) -> int {
    for (int i = 0; i < argc; i++)
        printf("%s\n", argv[i]);

    return 0;
}d