cimport stdio.h;

func main(int argc, (arr)string argv) -> int {
    int iteration = 0;
    JUMP:

    printf("This is an iteration loop using the 'jump' keyword! - %d\n", iteration);
    iteration++;

    if (iteration > 10) return 0;

    jump JUMP; // This is an alias for the 'goto' keyword in C/C++.

    // This is unreachable code:
    return -1;
}