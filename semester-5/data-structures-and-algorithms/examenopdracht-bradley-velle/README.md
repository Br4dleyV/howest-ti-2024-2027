# Examenopdracht Bradley Velle - Groep 6 - Mondeling examen 12/11/2026
Gekozen opdracht: String Matching

## PowerPoint-presentatie

De presentatie staat in [presentatie/String-matching-Bradley-Velle.pptx](./presentatie/String-matching-Bradley-Velle.pptx), met een [PDF-versie](./presentatie/String-matching-Bradley-Velle.pdf). De 12 hoofddia’s hebben een richttijd van 9 minuten 30 seconden. Vier verborgen reservedia’s ondersteunen de vragen achteraf. Bij elke dia staan sprekersnotities.

Voor het oefenen: [spreektekst met timing](./presentatie/Sprekersnotities.md) en [examenvragen met antwoorden](./presentatie/Examenvoorbereiding.md). De opdracht werd individueel uitgevoerd omdat de klas een oneven aantal studenten telde.

Vereisten voor de presentatie:
- PowerPoint-presentatie van maximum 10 minuten (je zal onderbroken worden na 10 minuten) (na presentatie krijg je vragen over de opdracht, aangevuld met algemene vragen over de cursus)
    - De context van de gekozen opdracht
    - Toelichting van de gemaakte keuzes bij de implementatie van het gekozen onderwerp (cfr. README)
    - De werking van de gekozen datastructuren en algoritmen. Gebruik hierbij specifieke voorbeelden en leg de zaken conceptueel uit, zonder al te veel tekst of ingewikkelde codefragmenten
    - Zeg iets over de performantie van je eigen implementatie (cfr. README)
    - Bespreek indien mogelijk een paar problemen waar je op botste tijdens de uitwerking en hoe je die hebt opgelost
    - Maak duidelijk hoe de taakverdeling was bij het uitwerken van de opdracht: wie heeft waarop de focus gelegd?

## Structuur
```
examenopdracht-bradley-velle/
├── README.md
├── brute_force.py
├── kmp.py
├── compare_algorithms.py
├── compare_wildcards.py
└── input_data/
    ├── voorbeeld.txt
    ├── snelle_mismatch.txt
    ├── veel_vergelijkingen.txt
    └── wildcards.txt
```

- **README.md**: beschrijving van de opdracht, implementatiekeuzes, uitvoerinstructies en performantie.
- **brute_force.py**: implementatie van het brute-force string-matching algoritme (deel 1).
- **kmp.py**: KMP-zoekfunctie en opbouw van de prefix-tabel (deel 2).
- **compare_algorithms.py**: vergelijking van brute-force en KMP op dezelfde inputbestanden.
- **compare_wildcards.py**: metingen met verschillende aantallen en posities van wildcards (deel 3).
- **input_data/voorbeeld.txt**: korte voorbeeldtekst waarin het patroon `kat` twee keer voorkomt.
- **input_data/snelle_mismatch.txt**: 100.000 keer `a`, doorzocht met `bbbbbb` voor onmiddellijke mismatches.
- **input_data/veel_vergelijkingen.txt**: 100.000 keer `a`, doorzocht met `aaaaab` voor veel gedeeltelijke matches; ook gebruikt voor de wildcardmetingen.
- **input_data/wildcards.txt**: korte tekst met `goed`, `goud`, `goad`, `god` en `goood` om te controleren dat `?` precies één karakter accepteert.

## Gemaakte keuzes
### Programmeertaal
Voor de implementatie heb ik Python gekozen omdat ik hiermee het algoritme het simpelste kon uitleggen en implementeren. 

### Brute-force implementatie
Het brute-force algoritme gebruikt twee while-lussen. De buitenste lus doorloopt de mogelijke startposities in de tekst. De binnenste lus vergelijkt de karakters van het patroon met de tekst. Zodra twee karakters verschillen, stopt de binnenste lus en wordt de volgende startpositie geprobeerd.

De functie geeft een lijst met startposities van gevonden matches en het aantal uitgevoerde karaktervergelijkingen terug. De karaktervergelijkingen worden teruggegeven om de performantie te kunnen onderzoeken en later te vergelijken met KMP.

Er wordt gezocht naar alle matches, inclusief overlappende matches. De vergelijking is hoofdlettergevoelig omdat de tekst niet eerst wordt omgezet naar kleine letters. Een patroon dat langer is dan de tekst geeft geen matches en nul karaktervergelijkingen. Een leeg patroon wordt geweigerd.

### KMP-implementatie zonder wildcards

KMP gebruikt een prefix-tabel, ook wel LPS-tabel (Longest Proper Prefix which is also Suffix). Voor iedere patroonpositie bewaart de tabel de lengte van het langste voorstuk dat ook een eindstuk is van het patroon tot die positie. Het volledige stuk zelf telt niet mee.

Bij het patroon `ababaca` is de tabel `[0, 0, 1, 2, 3, 0, 1]`. Het stuk `ababa` heeft bijvoorbeeld `aba` als voorstuk en eindstuk: daarom staat op index 4 de waarde 3.

Bij een mismatch gebruikt KMP de tabel om te bepalen hoeveel al gematchte karakters bruikbaar blijven. De tekstpositie gaat niet achteruit. Als bijvoorbeeld `ababa` al overeenkomt en het volgende karakter afwijkt van de verwachte `c`, blijft `aba` bruikbaar en wordt hetzelfde tekstkarakter vergeleken met patroonpositie 3. Zo hoeft de zoektocht niet volledig opnieuw te beginnen. Deze manier om eerder gevonden informatie te hergebruiken is het centrale idee van KMP. Bron: [Knuth, Morris en Pratt, Fast Pattern Matching in Strings](https://epubs.siam.org/doi/10.1137/0206024).

Ik heb de tabelopbouw en het zoeken in aparte functies gezet, zodat beide stappen apart te begrijpen en te controleren zijn. `build_lps(pattern)` geeft de tabel en het aantal patroonvergelijkingen terug. `kmp_search(text, pattern)` geeft drie waarden terug: matchposities, zoekvergelijkingen en tabelvergelijkingen. De teller wordt direct voor elke karaktervergelijking verhoogd, ook bij een mismatch.

Net als brute-force zoekt KMP hoofdlettergevoelig naar alle matches. Na een match wordt de prefix-tabel gebruikt om ook overlappende matches te vinden. Een leeg patroon wordt geweigerd en een te lang patroon geeft direct geen matches en nul vergelijkingen. De brute-force voorbeelden worden alleen bij rechtstreeks starten uitgevoerd, zodat de vergelijkingscode de zoekfunctie kan importeren.

### Wildcards (deel 3)

Een `?` in het patroon accepteert precies één willekeurig karakter, ook een spatie, regeleinde of Unicode-karakter. Bijvoorbeeld: `go?d` matcht `goed`, `goud` en `goad`, maar niet `god` of `goood`. Alleen het patroon kent wildcards; een `?` in de tekst is gewoon een tekstkarakter. Er is geen aparte escape-syntax om naar een letterlijke `?` te zoeken.

Bij brute-force wordt een wildcard geaccepteerd zonder de waarde van het tekstkarakter te vergelijken. Alle overige karakters worden zoals voorheen vergeleken. De buitenste lus zorgt ervoor dat het volledige patroon in de tekst past, ook als het patroon eindigt met wildcards.

Bij KMP kan de gewone prefix-tabel niet zomaar wildcardvergelijkingen gebruiken. Een `?` kan zowel `a` als `b` accepteren, maar daaruit volgt niet dat `a` en `b` gelijk zijn. Na een match van `a?` in de tekst `abb` mag bijvoorbeeld niet zomaar worden aangenomen dat de tweede letter `b` het begin van een nieuwe match is. Het lastige hergebruik bij zulke niet-transitieve overeenkomsten wordt ook besproken in [Dehghani, Mhaskar en Smyth, Practical KMP/BM Style Pattern-Matching on Indeterminate Strings](https://arxiv.org/abs/2204.08331).

Daarom heb ik KMP uitgebreid met het zoeken naar vaste stukken:

1. Verdeel het patroon in aaneengesloten stukken zonder `?` en onthoud hun offset.
2. Zoek ieder vast stuk met de bestaande, exacte KMP-zoekfunctie.
3. Trek de offset af van iedere gevonden positie. Dit geeft een mogelijke startpositie van het volledige patroon.
4. Bewaar alleen startposities waar alle vaste stukken op de juiste afstand voorkomen en waar het volledige patroon in de tekst past.

Voor `go?d` liggen de vaste stukken `go` op offset 0 en `d` op offset 3. Als `go` op tekstpositie 5 staat en `d` op positie 8, leveren beide startpositie 5 op. De tussenliggende positie hoort bij `?` en hoeft niet op waarde gecontroleerd te worden. Een startpositie die door alle stukken wordt bevestigd is dus een match. Dat vindt ook overlappende matches en wildcards aan het begin of einde.

Dit is een uitbreiding met KMP als zoekalgoritme voor de vaste stukken. De wildcardvariant voert mogelijk meerdere KMP-zoekopdrachten uit en heeft daardoor een andere complexiteit dan gewone KMP. Zonder wildcards blijft de oorspronkelijke KMP-route in gebruik. Als het patroon uitsluitend uit `?` bestaat, worden direct alle passende startposities teruggegeven, zonder karaktervergelijkingen.

### Inputdata
De inputteksten worden ingelezen uit aparte tekstbestanden zodat de teksten kunnen worden aangepast zonder het algoritme te wijzigen. De bestandsnamen en bijbehorende zoekpatronen staan in de lijst `voorbeelden` in `brute_force.py` voor deel 1 en in `compare_algorithms.py` voor deel 2. De wildcardvoorbeelden staan in `VOORBEELDEN` in `compare_wildcards.py`. De korte voorbeeldtekst is gegenereerd met behulp van AI. De twee grotere bestanden zijn bewust opgebouwd uit herhaalde karakters om onmiddellijke mismatches en het slechtste geval te onderzoeken. Ze hebben dezelfde tekstlengte en hun patronen hebben dezelfde lengte, zodat de inhoud het verschil in aantal vergelijkingen bepaalt.

## Instructies om uit te voeren
### Deel 1
Python 3.9 of hoger is vereist. Er zijn geen externe dependencies nodig. De inputbestanden staan in [input_data/](./input_data/).

Voer vanuit de hoofdmap van de repository het volgende commando uit:

```bash
python3 brute_force.py
```

Het programma doorzoekt automatisch alle drie de voorbeelden. Per voorbeeld toont het de tekstlengte, het patroon, het aantal matches, de eerste tien startposities (geteld vanaf nul), het aantal karaktervergelijkingen en de mediane zoektijd in milliseconden over vijf uitvoeringen.

Om andere input te gebruiken, zet je een tekstbestand in `input_data/` en pas je de lijst `voorbeelden` in [brute_force.py](./brute_force.py) aan. Iedere regel bevat een bestandsnaam en een zoekpatroon, bijvoorbeeld `("voorbeeld.txt", "kat")`. Het aantal tijdsmetingen kan worden aangepast via `herhalingen` (minimaal één).

### Deel 2

Gebruik dezelfde Python-versie en inputbestanden als bij deel 1. Vergelijk beide algoritmen met:

```bash
python3 compare_algorithms.py
```

Ook `python3 kmp.py` start deze vergelijking. Per bestand worden brute-force en KMP ieder vijf keer uitgevoerd. De uitvoer toont matches, zoekvergelijkingen, tabelvergelijkingen, het totale aantal vergelijkingen en de mediane uitvoeringstijd. De resultaten van beide algoritmen moeten dezelfde matchposities bevatten; het programma controleert dat automatisch.

Wijzig voor andere input de lijst `voorbeelden` in `compare_algorithms.py`. Voor meer tijdsmetingen pas je `herhalingen` in `main()` aan. Het uitvoerlabel gebruikt automatisch hetzelfde aantal.

### Deel 3

Start de wildcardmetingen vanuit de hoofdmap van de repository:

```bash
python3 compare_wildcards.py
```

Het programma vergelijkt beide algoritmen op `go?d` in `wildcards.txt` en op patronen met nul tot zes wildcards in een tekst met 100.000 keer `a`. Ieder lang patroon bevat precies zes karakters. Daardoor blijven de tekst- en patroonlengte gelijk terwijl het aantal of de positie van wildcards verandert.

Iedere zoekopdracht wordt vijf keer gemeten. De uitvoer bevat het aantal matches, de eerste tien startposities, de zoekvergelijkingen, de tabelvergelijkingen, het totaal en de mediane tijd. Ook het verdelen in vaste stukken en het combineren van posities vallen bij KMP binnen de tijdsmeting. De meetcode controleert dat beide algoritmen dezelfde matchposities opleveren.

Pas `VOORBEELDEN` in `compare_wildcards.py` aan voor andere bestanden of patronen. Het aantal metingen stel je in via `herhalingen` in `main()` van dat bestand. `brute_force_search(text, pattern)` en `kmp_search(text, pattern)` ondersteunen beide rechtstreeks `?`; er is geen aparte optie nodig.

## Toelichting performantie
### Deel 1
Het brute-force algoritme probeert het patroon op elke mogelijke startpositie in de tekst. Met n als tekstlengte en m als patroonlengte zijn er n - m + 1 mogelijke startposities, wanneer het patroon in de tekst past. Per startpositie worden maximaal m karakters vergeleken.

De worst-case tijdscomplexiteit is O(n * m). Bij een vaste patroonlengte is dit lineair in de tekstlengte. Als de patroonlengte bijvoorbeeld ongeveer de helft van de tekstlengte is en beide groeien, kan het slechtste geval kwadratisch worden, oftewel O(n²).

Wanneer bij iedere startpositie het eerste karakter al verschilt, is er slechts één karaktervergelijking per startpositie nodig. Voor een vaste patroonlengte is de tijdscomplexiteit dan O(n), oftewel lineair.

#### Meetmethode

Elk bestand wordt één keer ingelezen. Vervolgens wordt de zoekfunctie vijf keer uitgevoerd, gemeten met `perf_counter()`. De mediaan is de middelste tijd na het sorteren van de vijf tijden. Het inlezen van het bestand en het printen vallen buiten de meting; het zoeken, tellen van vergelijkingen en verzamelen van matches vallen erin.

#### Meetresultaten

Gemeten op 14 oktober 2026 met Python 3.14.4 op Ubuntu Server. Tijden kunnen per computer en uitvoering verschillen. Bij dezelfde input blijft het aantal karaktervergelijkingen gelijk. De korte tekst is vooral een correctheidsvoorbeeld; de zeer kleine zoektijd is gevoelig voor meetruis.

| Inputbestand | Tekstlengte `n` | Patroon | Patroonlengte `m` | Matches | Karaktervergelijkingen | Mediane tijd (5 runs) |
| --- | ---: | --- | ---: | ---: | ---: | ---: |
| `voorbeeld.txt` | 35 | `kat` | 3 | 2 | 37 | 0,006 ms |
| `snelle_mismatch.txt` | 100.000 | `bbbbbb` | 6 | 0 | 99.995 | 22,560 ms |
| `veel_vergelijkingen.txt` | 100.000 | `aaaaab` | 6 | 0 | 599.970 | 71,523 ms |

#### Vergelijking met de theorie

In `voorbeeld.txt` wordt `kat` gevonden op indices 3 en 25. Er zijn 33 mogelijke startposities en 37 karaktervergelijkingen: sommige pogingen vergelijken meer dan één karakter.

Bij `snelle_mismatch.txt` verschilt het eerste patroonkarakter `b` meteen van het tekstkarakter `a`. Er is precies één vergelijking per startpositie: `100.000 - 6 + 1 = 99.995`. Dit illustreert lineair gedrag bij een vaste patroonlengte.

Bij `veel_vergelijkingen.txt` komen eerst vijf `a`-karakters overeen, maar verschilt het zesde karakter van de `b` in het patroon. Elke startpositie vereist dus alle zes vergelijkingen: `(100.000 - 6 + 1) * 6 = 599.970`. Dit bereikt de worst-case grens voor deze tekst- en patroonlengte.

De laatste zoekopdracht vereist precies zes keer zoveel karaktervergelijkingen en heeft een langere gemeten uitvoeringstijd. De tijd is niet precies zes keer zo lang, omdat ook de lussen en andere Python-instructies tijd kosten. De voorbeelden sluiten daarmee aan bij de theoretische aantallen vergelijkingen. Omdat hier vaste lengtes worden gebruikt, bewijzen deze metingen op zichzelf geen kwadratische groei wanneer beide lengtes toenemen.

#### Correctheidscontroles

De functie is gecontroleerd op overlappende matches (`aaaaa` met `aaa` geeft `[0, 1, 2]` en negen vergelijkingen), geen matches, een patroon langer dan de tekst, een lege tekst, matches aan het begin en einde, hoofdlettergevoeligheid en het weigeren van een leeg patroon. Ook de aantallen matches en vergelijkingen van de drie inputbestanden zijn gecontroleerd.

### Deel 2

#### Theoretische vergelijking

| Algoritme | Voorbereiding | Worst-case zoektijd | Totaal | Extra geheugen, inclusief `k` matches |
| --- | --- | --- | --- | --- |
| Brute-force | Geen tabel | `O(n * m)` | `O(n * m)` | `O(k)` |
| KMP met prefix-tabel | `O(m)` | `O(n)` | `O(n + m)` | `O(m + k)` |

KMP is lineair in de gezamenlijke tekst- en patroonlengte. De tekstpositie gaat alleen vooruit. Bij een mismatch kan de patroonpositie terugvallen, maar de totale hoeveelheid terugvallen blijft lineair: de positie moet eerst vooruitgaan voordat ze weer kan terugvallen. De tabelopbouw gebruikt hetzelfde principe. Een tekstkarakter kan dus meer dan één keer worden vergeleken; lineair betekent hier niet dat ieder karakter precies één keer wordt vergeleken. Het oorspronkelijke artikel beschrijft het vinden van alle matches in lineaire tijd: [Fast Pattern Matching in Strings](https://epubs.siam.org/doi/10.1137/0206024).

Brute-force en KMP zijn bij een vaste patroonlengte beide lineair in `n`, maar KMP voorkomt veel herhaalde vergelijkingen. Als beide lengtes groeien, kan brute-force kwadratisch worden, terwijl KMP lineair in `n + m` blijft.

#### Meetmethode en resultaten

Beide algoritmen krijgen exact dezelfde ingelezen tekst en hetzelfde patroon. Iedere functie wordt vijf keer uitgevoerd; de gerapporteerde tijd is de mediaan. Bij KMP wordt de prefix-tabel bij iedere uitvoering opnieuw gemaakt en volledig meegemeten. Zo wordt de voorbereiding niet verborgen. Het lezen van bestanden en het printen vallen buiten de tijdsmeting. Zoekvergelijkingen zijn vergelijkingen tussen tekst en patroon; tabelvergelijkingen zijn vergelijkingen tussen twee patroonkarakters. Voor de totale werklast worden beide opgeteld.

Deze vergelijking is gemeten op 14 oktober 2026 met Python 3.14.4 op Linux x86_64. Dit is een nieuwe meting; de brute-force tijden kunnen daarom afwijken van deel 1. De tekstlengtes en patronen zijn dezelfde als in de tabel van deel 1.

| Inputbestand | Algoritme | Matches | Zoekvergelijkingen | Tabelvergelijkingen | Totaal | Mediane tijd (5 runs) |
| --- | --- | ---: | ---: | ---: | ---: | ---: |
| `voorbeeld.txt` | Brute-force | 2 | 37 | 0 | 37 | 0,006 ms |
| `voorbeeld.txt` | KMP | 2 | 35 | 2 | 37 | 0,004 ms |
| `snelle_mismatch.txt` | Brute-force | 0 | 99.995 | 0 | 99.995 | 20,082 ms |
| `snelle_mismatch.txt` | KMP | 0 | 100.000 | 5 | 100.005 | 10,935 ms |
| `veel_vergelijkingen.txt` | Brute-force | 0 | 599.970 | 0 | 599.970 | 69,060 ms |
| `veel_vergelijkingen.txt` | KMP | 0 | 199.995 | 9 | 200.004 | 24,418 ms |

#### Bespreking

- In de korte voorbeeldtekst vinden beide algoritmen matches op indices 3 en 25. KMP gebruikt minder zoekvergelijkingen, maar inclusief de tabel gebruiken beide 37 vergelijkingen. De tijden zijn zo klein dat meetruis een grote rol speelt.
- Bij onmiddellijke mismatches gebruikt brute-force 99.995 vergelijkingen. KMP onderzoekt alle 100.000 tekstkarakters en maakt eerst de tabel: 100.005 vergelijkingen in totaal. KMP bespaart hier dus geen vergelijkingen. Dat deze Python-implementatie toch sneller is, komt door verschillen in de uitgevoerde instructies en lussen; vergelijkingstellingen voorspellen tijden niet exact.
- Bij veel gedeeltelijke matches gebruikt brute-force 599.970 vergelijkingen, tegenover 200.004 bij KMP inclusief de voorbereiding: ongeveer drie keer minder. KMP is in deze meting ongeveer 2,83 keer sneller. Na het vergelijken van de eerste vijf `a`-karakters hergebruikt KMP bij een mismatch vier daarvan, in plaats van weer helemaal opnieuw te beginnen.

KMP heeft dus vooral voordeel wanneer eerdere matches opnieuw bruikbaar zijn. De lineaire worst-case grens is beter, maar op iedere mogelijke input minder vergelijkingen of een kortere uitvoeringstijd is niet gegarandeerd. De drie voorbeelden illustreren dit; ze vormen geen afzonderlijk schaalonderzoek met groeiende tekst- en patroonlengtes.

#### Correctheidscontroles

De exacte zoekmethodes zijn tijdens de ontwikkeling gecontroleerd op bekende prefix-tabellen, vergelijkingstellingen, overlap, matches aan begin en einde, geen matches, lege tekst, te lange en lege patronen, hoofdlettergevoeligheid en Unicode. De uitvoer is daarnaast gecontroleerd met een onafhankelijke referentie. De vergelijkingsprogramma’s controleren bij iedere meting automatisch dat beide algoritmen dezelfde matches opleveren.

### Deel 3

#### Wat wordt geteld?

De teller voor zoekvergelijkingen telt uitsluitend vergelijkingen tussen een tekstkarakter en een vast patroonkarakter. Een `?` accepteert een karakter zonder een dergelijke vergelijking en verhoogt deze teller niet. De test of een patroonkarakter `?` is, luscontroles, offsetberekeningen en setbewerkingen worden niet als karaktervergelijking geteld; hun tijd valt wel binnen de meting. Tabelvergelijkingen zijn de patroon-patroonvergelijkingen bij het opbouwen van alle gebruikte prefix-tabellen. Het totaal is de som van beide tellers.

Dit verklaart waarom nul karaktervergelijkingen niet betekent dat de uitvoering geen tijd kost. Het verwerken van wildcards en het opslaan van matches kost ook werk. Zonder wildcards blijven de vergelijkingstellingen van deel 1 en 2 gelijk; extra controles kunnen de uitvoeringstijden wel veranderen.

#### Impact op de asymptotische complexiteit

| Methode | Worst-case tijd | Extra geheugen |
| --- | --- | --- |
| Brute-force, met of zonder wildcards | `O(n * m)` | `O(k)` voor de matches |
| Gewone KMP, zonder wildcards | `O(n + m)` | `O(m + k)` |
| KMP-uitbreiding met `r` vaste stukken | `O(m + (r + 1) * n)` | `O(n + m)` |
| KMP-uitbreiding, uitsluitend wildcards | `O(n + m)` | `O(k)` |

Hierbij is `k` het aantal matches. Bij brute-force verandert de worst-case complexiteit niet: per startpositie kunnen nog steeds `m` patroonposities worden doorlopen. Zelfs bij alleen wildcards doorloopt deze implementatie die posities, hoewel de teller voor vaste karaktervergelijkingen dan nul is.

Bij de KMP-uitbreiding kost het opdelen `O(m)`. Elk van de `r` vaste stukken wordt met gewone KMP doorzocht. Het omrekenen en combineren van hun posities kost per stuk maximaal `O(n)`; ook de uiteindelijke uitvoer wordt in lineaire tijd gemaakt. Voor setbewerkingen wordt de gebruikelijke gemiddelde constante tijd per bewerking aangenomen. De totale tijd is daarom `O(m + (r + 1) * n)`, in het slechtste geval `O(n * m)` als het aantal vaste stukken met `m` groeit. Bij een vast aantal stukken blijft dit lineair in tekst- en patroonlengte. Een lege kandidatenset zorgt voor vroegtijdig stoppen.

Het aantal stukken hangt af van zowel het aantal als de plaats van de wildcards. Als `w` het aantal wildcards is, geldt `r <= min(w + 1, m - w)`. Aaneengesloten wildcards maken dus niet telkens een nieuw vast stuk. Een patroon met alleen wildcards heeft geen vaste stukken en wordt rechtstreeks afgehandeld.

#### Meetresultaten

Gemeten op 15 oktober 2026 met Python 3.14.4 op Linux x86_64. De methode is dezelfde als bij deel 2: vijf uitvoeringen, mediane tijd, voorbereiding inbegrepen en bestandsinvoer en printen uitgesloten. De langere voorbeelden gebruiken allemaal hetzelfde bestand `veel_vergelijkingen.txt` met 100.000 keer `a` en patronen van zes karakters. `KMP-stukken` is de hierboven beschreven wildcarduitbreiding. De tijden zijn momentopnamen en kunnen variëren.

| Patroon | Wildcards | Methode | Matches | Zoekvergelijkingen | Tabelvergelijkingen | Totaal | Mediane tijd (5 runs) |
| --- | ---: | --- | ---: | ---: | ---: | ---: | ---: |
| `aaaaab` | 0 | Brute-force | 0 | 599.970 | 0 | 599.970 | 84,295 ms |
| `aaaaab` | 0 | KMP | 0 | 199.995 | 9 | 200.004 | 24,514 ms |
| `aaaa?b` | 1 | Brute-force | 0 | 499.975 | 0 | 499.975 | 77,773 ms |
| `aaaa?b` | 1 | KMP-stukken | 0 | 200.000 | 3 | 200.003 | 52,280 ms |
| `aaa??b` | 2 | Brute-force | 0 | 399.980 | 0 | 399.980 | 71,177 ms |
| `aaa??b` | 2 | KMP-stukken | 0 | 200.000 | 2 | 200.002 | 51,462 ms |
| `a?a?a?` | 3 | Brute-force | 99.995 | 299.985 | 0 | 299.985 | 70,784 ms |
| `a?a?a?` | 3 | KMP-stukken | 99.995 | 300.000 | 0 | 300.000 | 129,479 ms |
| `a????b` | 4 | Brute-force | 0 | 199.990 | 0 | 199.990 | 57,309 ms |
| `a????b` | 4 | KMP-stukken | 0 | 200.000 | 0 | 200.000 | 48,639 ms |
| `a?????` | 5 | Brute-force | 99.995 | 99.995 | 0 | 99.995 | 56,238 ms |
| `a?????` | 5 | KMP-stukken | 99.995 | 100.000 | 0 | 100.000 | 42,232 ms |
| `?????b` | 5 | Brute-force | 0 | 99.995 | 0 | 99.995 | 50,473 ms |
| `?????b` | 5 | KMP-stukken | 0 | 100.000 | 0 | 100.000 | 14,311 ms |
| `??????` | 6 | Brute-force | 99.995 | 0 | 0 | 0 | 49,314 ms |
| `??????` | 6 | KMP-stukken | 99.995 | 0 | 0 | 0 | 1,890 ms |

In de korte tekst `wildcards.txt` vinden beide algoritmen `go?d` op indices 0, 5 en 10: `goed`, `goud` en `goad`. Brute-force gebruikt 32 zoekvergelijkingen; KMP gebruikt 50 zoekvergelijkingen en één tabelvergelijking. De korte tekst toont vooral de correctheid; de overhead van twee zoekopdrachten kan hier groter zijn dan de besparing door KMP.

#### Antwoorden op de onderzoeksvragen

- **Heeft dit impact op de tijdscomplexiteit?** Brute-force blijft worst-case `O(n * m)`. De gekozen KMP-wildcarduitbreiding kan meerdere lineaire zoekopdrachten uitvoeren en heeft daarom een grens die ook van `r` afhangt. Gewone KMP zonder wildcards blijft `O(n + m)`.
- **Heeft dit impact op de tijden en vergelijkingstellingen?** Ja. In de reeks `aaaaab`, `aaaa?b`, `aaa??b` en `a????b` daalt bij brute-force het aantal vaste karaktervergelijkingen. De laatste mismatch blijft, maar wildcards vervangen eerdere vergelijkingen. Bij KMP veroorzaken meerdere stukken ook extra zoekopdrachten en het combineren van posities. Minder vergelijkingen betekent niet automatisch een evenredig kortere tijd.
- **Hangt de impact af van het aantal wildcards?** Ja, maar het aantal alleen volstaat niet. `a?a?a?` heeft drie vaste stukken en vereist drie KMP-zoekrondes; in deze meting is dat langzamer dan brute-force. De patronen `a?????` en `?????b` hebben allebei vijf wildcards, maar leveren respectievelijk veel en geen matches op. Dat beïnvloedt het verzamelen van posities en de uitvoeringstijd. Alleen wildcards geeft nul vaste karaktervergelijkingen; de KMP-uitbreiding kan de passende posities dan direct teruggeven.

#### Correctheidscontroles

Tijdens de ontwikkeling zijn alle 15.240 combinaties van teksten met nul tot zes karakters uit `a` en `b`, en patronen met één tot vier karakters uit `a`, `b` en `?` gecontroleerd. Een onafhankelijke regex-referentie met lookahead controleerde alle matchposities, inclusief overlap. Alle combinaties zijn geslaagd.

Daarnaast zijn de voorbeeldwoorden, wildcards aan begin en einde, uitsluitend wildcards, te lange patronen, lege tekst, lege patronen, hoofdlettergevoeligheid, Unicode en een wildcard die een regeleinde accepteert gecontroleerd. Bekende prefix-tabellen en vergelijkingstellingen zonder wildcards zijn opnieuw gecontroleerd. Ook deze controles zijn geslaagd.

#### Eindcontrole

Bij de eindcontrole op 15 oktober 2026 zijn daarnaast 5.000 reproduceerbaar willekeurig gegenereerde tekst-patrooncombinaties gecontroleerd met een onafhankelijke regex-referentie. Deze bevatten ook Unicode, regeleindes en vraagtekens in de tekst. Voor brute-force zijn de vergelijkingstellingen onafhankelijk nagerekend. Ook 510 prefix-tabellen zijn met een rechtstreekse prefix/suffix-referentie vergeleken. Alle controles zijn geslaagd.

Alle 25 resultaatrijen in de tabellen van deel 1, 2 en 3 zijn opnieuw gecontroleerd: de inputlengtes, matches en vergelijkingstellingen kloppen. De tijden zijn eerdere meetmomenten en hoeven bij opnieuw uitvoeren niet exact gelijk te zijn. Alle vier uitvoercommando's werken ook vanuit een andere werkmap. Het importeren van de zoekfuncties voert geen benchmarks uit.

## Onderdelen van de opdracht
### Deel 1: Brute-force string-matching algoritme
Het brute-force string-matching algoritme dat een tekstbestand doorzoekt is geïmplementeerd in [brute_force.py](./brute_force.py). 

Deel 1 is uitgewerkt: de functie doorzoekt tekstbestanden en telt de karaktervergelijkingen. Drie voorbeelden worden elk vijf keer gemeten. De meetresultaten, tijdscomplexiteit en vergelijking met de theorie staan onder [Toelichting performantie](#toelichting-performantie).

### Deel 2: Knuth-Morris-Pratt algoritme
Deel 2 is uitgewerkt in [kmp.py](./kmp.py). De prefix-tabel en de zoekfunctie zijn geïmplementeerd en gecontroleerd. [compare_algorithms.py](./compare_algorithms.py) vergelijkt KMP met brute-force op dezelfde input, inclusief voorbereidingstijd en alle karaktervergelijkingen. De uitleg en resultaten staan bij de gemaakte keuzes en onder [Toelichting performantie](#toelichting-performantie).

### Deel 3: Eenvoudige wildcards in patroon
Deel 3 is uitgewerkt in [brute_force.py](./brute_force.py) en [kmp.py](./kmp.py). Beide ondersteunen `?` als precies één willekeurig karakter. De wildcardvariant van KMP zoekt vaste stukken met exacte KMP en combineert hun posities. [compare_wildcards.py](./compare_wildcards.py) vergelijkt verschillende aantallen en posities van wildcards. De implementatiekeuzes, complexiteit, meetresultaten en antwoorden op de onderzoeksvragen staan hierboven.
