#include <stdio.h>

int calculate_sum(int *numbers, int n)
{
    int sum = 0;
    for (int i = 0; i < n; i++)
    {
        sum += numbers[i];
    }

    return sum;
}

int main() {
    int data_array[] = {1, 4, 2, 8, 9, 10};

    int n = sizeof(data_array) / sizeof(data_array[0]);
    int sum = calculate_sum(data_array, n);

    printf("Som van getallen: %d", sum);
}
