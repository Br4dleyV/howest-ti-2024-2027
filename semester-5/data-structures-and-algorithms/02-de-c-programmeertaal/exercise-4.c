#include <stdio.h>

int sum_array(int arr[], int size) {
  int sum = 0;

  for (int i = 0; i < size; i++) {
      sum += arr[i];
  }

  return sum;
}

int main() {
  int data_array[] = {1, 4, 2, 8, 9, 10};
  int data_array_size = sizeof(data_array) / sizeof(data_array[0]);

  int sum = sum_array(data_array, data_array_size);
  printf("Sum of all elements in the array: %d\n", sum);
}