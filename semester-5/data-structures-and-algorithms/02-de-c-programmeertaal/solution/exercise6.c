#include <stdio.h>
#include <stdlib.h>

struct Person {
    char name;
    int age;
};

int main() {
    char names[] = { 'A', 'B', 'C', 'D' };
    int ages[] = {18, 29, 32, 46};

    int n = sizeof(names) / sizeof(names[0]);

    // optie 1: variable-length array
    struct Person people[n];
    for (int i = 0; i < n; i++) {
        struct Person person;
        person.name = names[i];
        person.age = ages[i];

        people[i] = person;
    }

    printf("%c (%d)\n", people[0].name, people[0].age);

    // optie 2: dynamische array
    struct Person *dyn_people = malloc(n * sizeof(struct Person));
    for (int i = 0; i < n; i++) {
        struct Person person;
        person.name = names[i];
        person.age = ages[i];

        dyn_people[i] = person;
    }

    printf("%c (%d)\n", dyn_people[0].name, dyn_people[0].age);

    free(dyn_people);
}
