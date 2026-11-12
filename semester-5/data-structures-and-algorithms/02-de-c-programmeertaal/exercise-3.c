#include <stdio.h>

int main() {
  int length = 4;
  int content = 2;

  //waarin alle elementen gelijk zijn aan `content`. Zorg ervoor dat alles blijft werken als de inhoud van `length` of `content` verandert.

  int arr[length];
  for (int i = 0; i < length; i++) {
    arr[i] = content;
    printf("%i ", i);
    printf("%i\n", arr[i]);
  }

  return 0;
}