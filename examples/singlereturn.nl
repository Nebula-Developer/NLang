cimport stdio.h;

func math.add(int a, int b) -> int => return a + b;

func main(int argc, (arr)string argv) -> int {
    printf("%d\n", math.add(1, 2));
}