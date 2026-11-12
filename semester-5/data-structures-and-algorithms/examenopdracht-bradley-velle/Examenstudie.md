# Examenstudie: String Matching en algemene cursus

## Wat moet je vooral leren voor de algemene vragen?

De precieze accenten hangen af van je cursus. Op basis van je hoofdstukken zou ik dit prioriteren.

### 1. Complexiteitsanalyse — hoogste prioriteit

Kun code bekijken en de tijdscomplexiteit uitleggen:

| Complexiteit | Typisch voorbeeld |
| --- | --- |
| `O(1)` | Een array-element via index lezen |
| `O(log n)` | Binair zoeken |
| `O(n)` | Een lijst eenmaal doorlopen |
| `O(n log n)` | Mergesort |
| `O(n²)` | Alle paren vergelijken |
| `O(2ⁿ)` | Alle deelverzamelingen onderzoeken |

Leer vooral:

- **Best-case, average-case en worst-case** onderscheiden.
- Opeenvolgende lussen: kosten optellen. Geneste lussen: aantallen iteraties onderzoeken; twee lussen betekenen niet automatisch `O(n²)`.
- Constante factoren en lagere termen weglaten: `3n² + 5n + 2` wordt `O(n²)`.
- Tijd tegenover extra geheugen afwegen.
- Bij jouw opdracht zijn er **twee invoergroottes**, `n` en `m`. `O(n × m)` wordt alleen `O(n)` als `m` constant blijft.

### 2. Datastructuren — werking, kosten en toepassing

| Structuur | Wat je moet onthouden |
| --- | --- |
| Array | Indexering `O(1)`; invoegen/verwijderen in het midden meestal `O(n)` |
| Gelinkte lijst | Zoeken `O(n)`; invoegen/verwijderen bij een geschikte bekende knoop kan `O(1)` |
| Stack | **LIFO**: laatst toegevoegd, eerst verwijderd; bijvoorbeeld functieaanroepen |
| Queue | **FIFO**: eerst toegevoegd, eerst verwijderd; bijvoorbeeld BFS |
| Hashtabel | Zoeken/invoegen gemiddeld `O(1)`; botsingen kunnen prestaties verslechteren |
| Binaire zoekboom | Links kleiner, rechts groter; bewerkingen `O(h)` met `h` de boomhoogte |
| Heap | Minimum of maximum snel beschikbaar; toevoegen en wortel verwijderen `O(log n)` |

Bij een zoekboom: **gebalanceerd `O(log n)`, scheef mogelijk `O(n)`**. Een heap is niet volledig gesorteerd.

### 3. Sorteren — kunnen uitvoeren op een klein voorbeeld

| Algoritme | Belangrijkste idee | Tijd |
| --- | --- | --- |
| Bubble sort | Verwissel verkeerde buurparen | Gemiddeld/worst `O(n²)` |
| Selection sort | Zoek telkens het kleinste resterende element | `O(n²)` |
| Insertion sort | Voeg elk element in het al gesorteerde deel in | Best `O(n)`, worst `O(n²)` |
| Mergesort | Verdeel, sorteer de helften en voeg samen | `O(n log n)` |
| Quicksort | Kies een pivot en verdeel errond | Gemiddeld `O(n log n)`, worst `O(n²)` |
| Heapsort | Bouw een heap en haal telkens het grootste weg | `O(n log n)` |

Ken ook **stabiel sorteren**: elementen met dezelfde sleutel behouden hun onderlinge volgorde. Bij arrays gebruikt mergesort doorgaans `O(n)` extra geheugen.

### 4. Grafen — vooral BFS, DFS en kortste paden

- **BFS:** zoekt laag per laag met een **queue**. Vindt kortste paden in een ongewogen graaf.
- **DFS:** gaat eerst zo diep mogelijk met een **stack of recursie**.
- Met een adjacentielijst kosten BFS en DFS **`O(V + E)`**.
- **Dijkstra:** kortste paden bij **niet-negatieve gewichten**.
- **Adjacentiematrix:** `O(V²)` geheugen, verbinding controleren in `O(1)`.
- **Adjacentielijst:** `O(V + E)` geheugen, handig bij weinig verbindingen.

Kun een kleine graaf doorlopen en de bezochte volgorde geven. Die volgorde hangt ook af van de volgorde waarin buren bekeken worden.

### 5. Ontwerptechnieken — herken het idee

- **Brute-force:** probeer alle relevante mogelijkheden.
- **Divide-and-conquer:** splits een probleem, los de delen op en combineer. Voorbeeld: mergesort.
- **Greedy:** kies telkens wat lokaal het beste lijkt. Dit geeft alleen bij geschikte problemen gegarandeerd het optimum.
- **Dynamisch programmeren:** bewaar oplossingen van overlappende deelproblemen en hergebruik ze.
- **Backtracking:** bouw een kandidaat op en keer terug zodra die niet meer geldig kan worden.

### 6. C — vergeet dit hoofdstuk niet

Herhaal vooral:

- Pointers: `&x` geeft een adres, `*p` benadert de waarde op dat adres.
- Arrays beginnen op index 0 en hebben geen automatische grenscontrole.
- C-strings eindigen op `'\0'`.
- `malloc` reserveert geheugen; `free` geeft het vrij.
- `struct` groepeert gegevens, bijvoorbeeld voor een knoop in een gelinkte lijst.
- Herken geheugenlekken, ongeldige pointers en toegang buiten een array.

**Oefen elk onderwerp met dezelfde vier vragen: wat doet het, hoe werkt het op een klein voorbeeld, wat kost het en wanneer gebruik je het?** Daarmee bereid je je veel beter voor op mondelinge bijvragen dan door alleen definities te memoriseren.
