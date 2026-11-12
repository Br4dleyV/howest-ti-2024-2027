# Oefening 1

De codevoorbeelden in de cursus werken enkel op arrays die getallen bevatten. Wat zou er moeten veranderen als de arrays strings bevatten in plaats van getallen? Op welke manier heeft dit invloed op de tijdscomplexiteit en de effectieve uitvoeringstijd?

*Om twee strings te vergelijken maken we gebruik van de lexicografische ordening, voor strings komt deze overeen met de [alfabetische volgorde](https://en.wikipedia.org/wiki/Alphabetical_order)*.

Implementeer vervolgens insertionsort voor een array van strings. Schrijf zelf de code om twee strings te vergelijken met elkaar. Probeer dit zo efficiënt mogelijk te doen.

# Oefening 2

Om de verschillende sorteeralgoritmes goed te begrijpen kan het helpen om ze op papier uit te proberen. Sorteer de volgende lijst met behulp van bubblesort, insertionsort, mergesort en quicksort.

```python
numbers = [5, 2, 9, 1, 5, 6]
```

# Oefening 3

Zoek op hoe het shell sort algoritme werkt en implementeer het.

# Oefening 4

Herschrijf het `mergesort` algoritme zodat er zo weinig mogelijk nieuwe lijsten aangemaakt moeten worden.

# Oefening 5

Herschrijf het `mergesort` algoritme zodat er geen gebruik wordt gemaakt van recursie.
Zorg er ook hier voor dat je zo weinig mogelijk nieuwe lijsten aanmaakt.
Het basisidee is hier om *bottom-up* te werken:

- Beschouw initieel elk element als een gesorteerde deellijst van grootte 1.
- In elke stap voeg je aangrenzende deellijsten samen (*merge*).
- Zo krijg je na stap 1 gesorteerde deellijsten van grootte 2, na stap 2 van grootte 4, na stap 3 van grootte 8, enz.
- Blijf dit doen tot je uiteindelijk de volledige lijst gesorteerd hebt.

Denk erover na of deze aanpak dezelfde asymptotische tijdscomplexiteit heeft als de recursieve variant.
Voer tijdsmetingen uit om de *bottom-up* en recursieve implementatie met elkaar te vergelijken.
Welke werkt in de praktijkt het snelst?

# Oefening 6

Bij `quicksort` is het dus belangrijk om de spil goed te kiezen. Doe onderzoek naar verschillende manieren waarop je dit kunt doen.

# Oefening 7

Ook voor `quicksort` worden in de voorbeeldimplementatie in de cursus veel nieuwe lijsten aangemaakt.
Herschrijf het algoritme zodat dit vermeden wordt.

