# ✅ Oefening 1

Schrijf een C programma dat je naam (als string), gevolgd door je leeftijd afprint. Probeer dit te doen door gebruik te maken van het `printf` statement.

# ✅ Oefening 2

Start met de volgende `main` functie:

```c
int main() {
  int data_array[] = {1, 4, 2, 8, 9, 10};
  int data_array_size = sizeof(data_array) / sizeof(data_array[0]);

  // complete the code here
}
```

Vul de code aan zodat de som van alle getallen uit `data_array` uitgeprint worden. Zorg ervoor dat je code nog altijd werkt als de inhoud van `data_array` verandert.

# ✅ Oefening 3

Begin met de volgende `main` functie:

```c
int main() {
  int length = 4;
  int content = 2;

  // complete the code here
}
```

Vervolledig de `main` functie zodat er een array met lengte `length` aangemaakt wordt waarin alle elementen gelijk zijn aan `content`. Zorg ervoor dat alles blijft werken als de inhoud van `length` of `content` verandert.

# ✅ Oefening 4

Pas oefening 2 aan zodat je eigen geschreven code in een aparte functie staat. Laat deze functie de som van alle getallen teruggeven.

# ✅ Oefening 5

Start met de volgende code:

```c
#include <stdlib.h>

int main() {
  int length = 4;
  int content = 2;

  int *data_array = malloc(length*sizeof(int));

  // call the function here

  free(data_array);
}
```

Schrijf een functie `fill` die `data_array` opvult zodat ieder element gelijk is aan `content`. Zorg ervoor dat de code blijft werken als de inhoud van `length` of `content` verandert. Je eigen functie mag geen nieuwe array aanmaken.

# ✅ Oefening 6

Start met de volgende code:

```c
int main() {
  char names[] = { 'A', 'B', 'C', 'D' };
  int ages[] = {18, 29, 32, 46};
}
```

De array `names` bevat de namen van enkele personen. Voor simpliciteit duiden we deze aan met 1 letter. De leeftijd van de persoon met initiaal `names[i]` kan gevonden worden in `ages[i]`.

Maak een `struct Person` aan die de naam en de leeftijd van een persoon samen bundelt. Creëer vervolgens een array die alle personen met hun leeftijd bevat.

Zorg ervoor dat je code blijft werken als de inhoud van `names` of `ages` verandert. Je mag ervan uitgaan dat `names` en `ages` altijd een gelijke lengte zullen hebben.

# ✅ Oefening 7

Herhaal oefening 6, maar deze keer zou de array pointers naar de structs moeten bevatten in plaats van enkel de struct.

# Oefening 8 (geavanceerd)

Implementeer een LIFO datastructuur in C. Dit is een datastructuur met twee operaties:

- Een element toevoegen
- Het laatst toegevoegde element verwijderen

Deze datastructuur implementeer je het best door gebruik te maken van een `linked list`. Ieder element van deze linked list kan voorgesteld worden door de volgende struct.

```c
struct ll_element {
  int content;
  struct ll_element* next;
};
```

# Oefening 9 (geavanceerd)

Implementeer een FIFO datastructuur in C. Net zoals de LIFO datastructuur zijn er twee operaties:

- Een element toevoegen
- Het element dat het langst in de datastructuur zit verwijderen



