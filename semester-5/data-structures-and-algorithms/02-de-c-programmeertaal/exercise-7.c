#include <stdio.h>
#include <stdlib.h>

struct Person {
  char name;
  int age;
};

int main() {
  char names[] = { 'A', 'B', 'C', 'D' };
  int ages[] = {18, 29, 32, 46};

  int arr_length = sizeof(names) / sizeof(names[0]);

  struct Person *persons[arr_length];

  for (int i = 0; i < arr_length; i++) {
    persons[i] = malloc(sizeof(struct Person));
    persons[i]->name = names[i];
    persons[i]->age = ages[i];
  }

  for (int i = 0; i < arr_length; i++) {
    printf("Person %d: name = %c, age = %d\n", i, persons[i]->name, persons[i]->age);
  }

  for (int i = 0; i < arr_length; i++) {
    free(persons[i]);
  }

  return 0;
}