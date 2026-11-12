#include <stdio.h>

int main() {
  int data_array[] = {1, 4, 2, 8, 9, 10};
  int data_array_size = sizeof(data_array) / sizeof(data_array[0]);

  // Print sum of all elements in the array
  int sum = 0;
  for (int i = 0; i < data_array_size; i++) {
      sum += data_array[i];
  }

  printf("Sum of all elements in the array: %d\n", sum);
}