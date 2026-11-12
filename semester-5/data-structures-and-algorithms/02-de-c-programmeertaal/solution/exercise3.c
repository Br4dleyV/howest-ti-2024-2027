#include <stdlib.h>

int main() {
    int length = 4;
    int content = 2;

    // optie 1: variable-length array
    int numbers[length];
    for (int i = 0; i < length; i++)
    {
        numbers[i] = content;
    }

    // optie 2: dynamische array
    int *dyn_numbers = malloc(length * sizeof(int));
    for (int i = 0; i < length; i++)
    {
        dyn_numbers[i] = content;
    }
    free(dyn_numbers);
}
