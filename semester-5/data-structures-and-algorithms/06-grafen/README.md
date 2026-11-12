# Oefening 1

Ontwerp grafen voor elk van onderstaande situaties. Denk na over de volgende zaken:
- Wat stellen de toppen en bogen voor
- Welk type graaf is het (gewogen vs ongewogen / gericht vs ongericht)
- Wat stelt de graad van een top voor binnen de situatie


### Situatie 1

We willen de structuur van wikipedia voorstellen en voorstellen hoe je via links van 1 pagina naar een andere pagina kunt geraken.

Hoe zou je, door gebruik te maken van termen uit de grafentheorie uitdrukken dat je, door gebruik te maken van een aantal kliks, van 1 pagina naar een andere kunt navigeren.

Hoe zou je, door gebruik te maken van termen uit de grafentheorie het "aantal kliks" noemen.

### Situatie 2

We willen gaan bijhouden welke acteurs met elkaar samengewerkt hebben. De film waarin ze samengewerkt hebben is hierbij niet belangrijk.

Hoe zou je, door gebruik te maken van termen uit de grafentheorie uitdrukken wie de acteur is die met het meeste andere acteurs heeft samengewerkt.

Gebruik [https://www.imdb.com/search/common/](https://www.imdb.com/search/common/) om deze graaf op te stellen voor de volgende acteurs:

- Tom Holland (imdb-id: nm4043618)
- Florence Pugh (imdb-id: nm6073955)
- Andrew Garfield (imdb-id: nm1940449)
- Kate Walsh (imdb-id: nm0005532)
- Logan Lerman (imdb-id: nm0503567)
- Molly Gordon (imdb-id: nm1094137)
- Emma Watson (imdb-id: nm0914612)

Hou enkel rekening met films. Hiervoor kun je na het zoeken de filter *Title type* op *movie* instellen.

Onderzoek of deze graaf samenhangend is. Als dat zo is, wat is het minimaal aantal toppen dat verwijderd moet worden zodat de graaf niet meer samenhanged is.

Stel vervolgens de adjacentiematrix voor deze graaf op.

# Oefening 2

Beslis voor de volgende situaties of je een *binary heap* of een *binary search tree* wil gebruiken.

- Je werkt op een IT-helpdesk waar er tickets afgehandeld moeten worden. Ieder ticket heeft een bepaalde prioriteit. Tickets moeten afgewerkt worden van hoogste prioriteit naar laagste prioriteit.
- Je houdt een verzameling van getallen bij en wil heel snel kunnen tellen hoeveel getallen in de verzameling zitten tussen een bepaalde minimum- en maximumgrens.
- Je houdt voor je muziekbibliotheek bij hoe vaak je naar ieder nummer geluisterd hebt, en wil makkelijk de top 10 van je meest beluisterde nummers kunnen opvragen
- Je houdt een collectie van alle woorden bij en wil heel snel kunnen opzoeken of een woord bestaat.

# Oefening 3

Implementeer een oplossing voor het volgende probleem:

*Je houdt een verzameling van getallen bij en wil heel snel kunnen tellen hoeveel getallen in de verzameling zitten tussen een bepaalde minimum- en maximumgrens.*

# Oefening 4

Implementeer het volgende probleem:

*Je houdt voor je muziekbibliotheek bij hoe vaak je naar ieder nummer geluisterd hebt, en wil makkelijk de top 10 van je meest beluisterde nummers kunnen opvragen.*

# Oefening 6

Het bestand [movies.csv](movies.csv) bevat een lijst met films, samen met de acteurs van die film. Schrijf een algoritme dat het *Bacon number* van een acteur berekent aan de hand van die dataset.

Het *Bacon number* van een acteur $a$ is het kleinste getal $n$ dat aan de volgende voorwaarde voldoet:

Er is een lijst van $n+1$ acteurs, startend met $a$ en eindigend met *Kevin Bacon* zodat opeenvolgende acteurs in de lijst in dezelfde film hebben meegespeeld.

Met andere woorden:
- Het *Bacon number* van Kevin Bacon is 0
- Het *Bacon number* van iedereen die in een film geacteerd heeft waar Kevin Bacon in zit is 1
- ...

# Oefening 7

Bedenk (implementeren is niet nodig) een algoritme dat treinreizigers kan helpen om de kortste route (in tijd) tussen twee stations te vinden. Denk na over de volgende vragen:

- Hoe ziet de graaf eruit die je zal gebruiken
- Welk algoritme kun je gebruiken als je geen rekening moet houden met overstaptijden, en dus kunt doen alsof die nul is
- Hoe zou je het algoritme aanpassen zodat je wel rekening houdt met de nodige overstaptijden