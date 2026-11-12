# ✅Oefening 1

Jullie krijgen een interface `IHashTable.cs` en een gedeeltelijke implementatie `ChainedLinearHashTable.cs` cadeau. 
Deze klasse maakt gebruik van linear hashing om de hashtabel uit te breiden, en gebruikt chaining om collisions op te lossen.

Begin met uit te zoeken hoe deze twee technieken werken en toegepast worden in de code. 
Vul daarna de methode `Contains` aan zodat deze teruggeeft of de parameter `item` aanwezig is in de collectie.

# Oefening 2

Schrijf een klasse `LinearProbingHashTable` die niet langer gebruikt maakt van chaining om collisions op te lossen, maar wel van *linear probing*. Hiervoor is de geavanceerde *linear hashing* techniek uit `ChainedLinearHashTable.cs` niet langer van toepassing: je mag in de *probing* variant gewoon het aantal buckets telkens meteen verdubbelen als de maximale *load factor* overschreden wordt en zal bij elke uitbreiding *alle* elementen terug op de juiste plek moeten zetten.

# Oefening 3 (optioneel)

Implementeer het *MurmurHash3* algoritme voor bitarrays in *C*. Zoek op welke waarden er exact gebruikt worden voor de constanten.

# Oefening 4 (optioneel)

Denk na over, en implementeer een manier om structs **met pointers** te serialiseren.
Je mag er initieel van uitgaan dat er geen circulaire referenties zijn.
Start eenvoudig en voeg geleidelijk aan extra functionaliteit toe (arrays, circulaire referenties, ...).
