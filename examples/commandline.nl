cimport stdio.h;
cimport string.h;

func main(int argc, (arr)string argv) -> int {
    for (int i = 0; i < argc; i++) {
        printf("%s\n", argv[i]);
    }

    if (argv[1] s== "hello" && argv[2] s== "world") { // s== means 'string equals'
        printf("Hello, world!\n");
    } else {
        printf("Try supplying 'hello world' as arguments.\n");
    }
    return 0;
}