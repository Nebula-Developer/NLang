cimport stdio.h;
cimport stdlib.h;
cimport string.h;

func main(int argc, (arr)string argv) -> int {
    zero argv[2];
    
    for (int i = 0; i < argc; i++)
        print("%s\n", argv[i]);

    if (argv[1] s== "hello" && argv[1] s== argv[3]) {
        printf("Same\n");
    }

    return 0;
}