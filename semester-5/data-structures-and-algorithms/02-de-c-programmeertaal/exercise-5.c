#include <stdio.h>
#include <stdlib.h>

void fill_array(int *arr, int length, int content) {
  for (int i = 0; i < length; i++) {
    arr[i] = content;
  }
}

int main() {
  int length = 4;
  int content = 2;

  int *data_array = malloc(length*sizeof(int));

  fill_array(data_array, length, content);
  for (int i = 0; i < length; i++) {
    printf("data_array[%d] = %d\n", i, data_array[i]);
  }

  free(data_array);
}