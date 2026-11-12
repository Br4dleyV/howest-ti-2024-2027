# Examenvoorbereiding — String matching

## Wat je klaar hebt

- `String-matching-Bradley-Velle.pptx`: 11 hoofddia’s met sprekersnotities en 4 verborgen reservedia’s. In de gewone diavoorstelling eindig je na dia 12; de reservedia’s kun je achteraf selecteren.
- `Sprekersnotities.md`: dezelfde spreektekst, met richttijden per dia.
- De opdracht is individueel uitgevoerd omdat de klas een oneven aantal studenten telde.
- De dia’s gebruiken de historische meetresultaten uit de README, geen nieuwe benchmarktijden.

## Eerst begrijpen: drie ideeën

**Brute-force:** probeer het patroon op elke mogelijke plaats. Als een letter verschilt, schuif je één plaats op en begin je opnieuw.

**KMP:** onthoud wat je al weet. Bij `aaaaab` kloppen eerst vijf a’s. Als de volgende letter geen b is, kunnen de laatste vier a’s het begin van een nieuwe poging vormen. Je hoeft die vier letters niet opnieuw te controleren. Een hulptabel zegt hoeveel letters je kunt behouden.

**Wildcards:** `go?d` betekent: `go`, daarna precies één willekeurig teken, daarna `d`. Mijn uitbreiding zoekt `go` en `d` apart met KMP. Daarna controleert ze of er precies één teken tussen staat.

Woorden die je op de dia’s tegenkomt:

| Woord | Betekenis |
| --- | --- |
| Patroon | Het stukje tekst dat je zoekt, bijvoorbeeld `kat` of `go?d` |
| Match | Een plaats waar het hele patroon past |
| Mismatch | Een letter die verschilt van wat je verwacht |
| Prefix | Een beginstuk |
| Suffix | Een eindstuk |
| LPS-tabel | Hulptabel die zegt hoeveel letters KMP kan hergebruiken |
| Offset | De plaats van een stuk binnen het patroon, geteld vanaf 0 |
| Mediaan | De middelste tijd als je de vijf gemeten tijden sorteert |

De hoofdpresentatie gebruikt nu eenvoudiger taal. De uitgebreidere vragen hieronder zijn voor het gesprek achteraf. Je hoeft ze niet allemaal in je presentatie te vertellen.

## Zo oefen je

1. Lees de notities één keer naast de dia’s. Zorg dat je de uitleg begrijpt voordat je probeert ze te onthouden.
2. Presenteer de eerste 11 dia’s hardop met een timer. Mik op 9 à 9,5 minuten. Na dia 6 zit je rond minuut 4:15; na dia 10 rond minuut 7:55.
3. Oefen daarna alleen met de dia’s. Gebruik de notities als vangnet, niet als voorleestekst.
4. Begin met het voorbeeld uit de hoofdpresentatie: `aaaaab` heeft de tabel `[0, 1, 2, 3, 4, 0]`. Leg uit waarom na vijf passende a’s vier a’s bruikbaar blijven. Probeer daarna pas het moeilijkere reservevoorbeeld `ababaca`. Oefen ook `aaa` in `aaaaa` voor overlap.
5. Verklaar zonder code hoe `go?d` in vaste stukken wordt verdeeld en hoe de offsets de matchstart opleveren.
6. Laat iemand hieronder willekeurige vragen kiezen. Geef eerst een direct antwoord van 20–40 seconden; werk een voorbeeld uit als er wordt doorgevraagd.

Bij tijdsdruk: kort de toelichting bij de meetmethode en de testaantallen in. Behoud het concrete KMP-voorbeeld en de wildcardoplossing: die tonen je begrip. Stop na dia 12; reserve is alleen voor vragen.

## Vragen over je opdracht

### 1. Waarom koos je dit onderwerp?

String matching is een concreet probleem waarbij je een eenvoudige brute-forceaanpak kunt vergelijken met een slimmer algoritme. Je kunt zichtbaar maken welk werk wordt herhaald, hoe KMP dat hergebruikt en waarom een uitbreiding met wildcards nieuwe correctheidsproblemen veroorzaakt. Pas dit antwoord aan je eigen motivatie aan.

### 2. Wat zijn de invoer, uitvoer en afspraken?

De invoer is een tekst en een niet-leeg patroon. De uitvoer bevat alle startposities, geteld vanaf nul, inclusief overlappende matches. Het zoeken is hoofdlettergevoelig. Een `?` in het patroon accepteert precies één karakter; in de tekst is het gewoon een teken. Een te lang patroon geeft geen matches. Een leeg patroon veroorzaakt `ValueError`. Er is geen escape-syntax voor een letterlijke `?` in het patroon.

### 3. Waarom is brute-force O(n × m)?

Er zijn maximaal `n - m + 1` passende startposities. Per positie controleer je maximaal `m` patroonposities. Het preciezere maximum is dus `(n - m + 1) × m` voor `1 <= m <= n`, met als gebruikelijke bovengrens `O(n × m)`. Dit maximum wordt bereikt bij een tekst van a’s en een patroon met a’s gevolgd door b.

### 4. Is brute-force altijd kwadratisch?

Nee. Er zijn twee invoerparameters. Bij een vaste patroonlengte `m` is de tijd lineair in `n`. Als beide groeien en bijvoorbeeld `m` ongeveer `n/2` is, kan de worst-case kwadratisch in `n` worden. Als `m = n`, is er juist maar één passende startpositie. Zeg daarom niet dat iedere groei van beide lengtes kwadratisch gedrag veroorzaakt.

### 5. Wat betekent een LPS-waarde precies?

`lps[i]` is de lengte van het langste echte voorstuk dat ook een eindstuk is van `pattern[:i + 1]`. Het volledige stuk zelf telt niet mee. In `ababa` is `aba` zo’n voorstuk en eindstuk, dus de waarde is drie. Voor `ababaca` is de hele tabel `[0, 0, 1, 2, 3, 0, 1]`.

### 6. Wat gebeurt er bij een mismatch in KMP?

Als al `j` karakters matchten, probeer ik `j = lps[j - 1]`. De tekstindex blijft gelijk. De tabel zegt welk korter voorstuk nog overeenkomt met het al bekende eindstuk. Pas als er geen bruikbaar voorstuk is en ook de eerste letter verschilt, gaat de tekstindex vooruit.

### 7. Waarom is KMP lineair ondanks de terugvallen?

De tekstindex gaat alleen vooruit. De patroonindex stijgt tijdens succesvolle stappen en daalt bij terugvallen. Hij kan in totaal niet meer dalen dan hij eerder is gestegen. Daardoor blijft het aantal zoekstappen `O(n)`. Dezelfde redenering geldt voor de tabelopbouw: `O(m)`. Samen `O(n + m)`.

### 8. Hoe vind je overlappende matches?

Na een match zet ik de patroonindex terug naar `lps[m - 1]`, in plaats van altijd naar nul. Voor `aaa` is de laatste LPS-waarde twee. In `aaaaa` blijven daardoor na iedere match twee a’s bruikbaar en vind ik startposities `[0, 1, 2]`.

### 9. Is KMP altijd sneller dan brute-force?

Nee. KMP heeft een betere worst-casegrens, maar maakt ook een tabel. Bij onmiddellijke mismatches doet brute-force één vergelijking per passende startpositie. In mijn voorbeeld zijn dat 99.995 vergelijkingen, tegenover 100.005 voor KMP inclusief tabel. De gemeten tijd hangt ook af van de lussen, Python-instructies en de computer.

### 10. Waarom mag je de gewone LPS-opbouw niet zomaar wildcardbewust maken?

Wildcardovereenkomst is niet transitief. `?` kan op a en b passen, zonder dat a gelijk is aan b. De exacte prefixlogica veronderstelt dat eerder vastgestelde overeenkomsten herbruikbaar zijn. Een naïeve aanpassing kan daardoor ongeldige overlap aannemen. Voor `a?` in `abb` is start nul geldig, maar start één niet.

### 11. Waarom is je oplossing met vaste stukken correct?

Elke echte match bevat alle vaste stukken op hun juiste offsets. Omgekeerd: als alle vaste stukken dezelfde kandidaatstart bevestigen en het volledige patroon past, kloppen alle vaste karakters. De overige posities zijn wildcards en accepteren ieder karakter. Dat is precies de definitie van een match. De doorsnede van de kandidaatsets houdt dus precies de geldige starts over.

### 12. Is dit nog één gewone KMP-zoekopdracht?

Het is een wildcarduitbreiding die exacte KMP als bouwsteen gebruikt. Elk vast stuk krijgt een eigen KMP-zoekopdracht. Het combineren van de resultaten is een extra stap. Daarom claim ik voor de wildcardvariant niet de gewone `O(n + m)`-grens voor elk patroon.

### 13. Waar komt O(m + (r + 1) × n) vandaan?

Het opdelen kost `O(m)`. Elk van de `r` vaste stukken wordt doorzocht in `O(n + lengte van het stuk)`. De som van alle stuklengtes is maximaal `m`. Kandidaten omrekenen en combineren kost maximaal `O(n)` per stuk, en de laatste uitvoerpassage kost `O(n)`. Samen geeft dit `O(m + (r + 1) × n)`, bij gebruikelijke gemiddelde constante kosten van setbewerkingen. Als `r` met `m` groeit, kan dit `O(n × m)` zijn.

### 14. Wat gebeurt er bij uitsluitend vraagtekens?

De KMP-uitbreiding vindt geen vaste stukken en retourneert direct `list(range(n - m + 1))`. Het opdelen en maken van de uitvoer kost `O(m + k)`, en dus maximaal `O(m + n)`. Brute-force doorloopt nog steeds de `m` patroonposities per start. Beide hebben nul vaste tekst-patroonvergelijkingen, maar doen wel ander werk.

### 15. Welke datastructuren gebruik je en waarom?

Strings bewaren tekst en patroon. Een lijst bewaart de LPS-lengtes en biedt directe toegang via een index. Een lijst verzamelt de matches. De wildcardvariant gebruikt een lijst van `(offset, vast stuk)`-paren en sets met kandidaatstarts. Sets maken een doorsnede eenvoudig en ondersteunen gemiddeld constante kosten voor toevoegen en opzoeken. De finale passage door de startposities geeft gesorteerde uitvoer zonder sorteerfase.

### 16. Hoeveel extra geheugen kost het?

Brute-force bewaart `k` matches: `O(k)` extra geheugen. Exacte KMP bewaart daarnaast een tabel van lengte `m`: `O(m + k)`. De wildcardvariant bewaart stukken, hun resultaten en kandidaatsets: `O(n + m)`. Bij alleen wildcards is de uitvoerlijst `O(k)`. Dit zijn grenzen bovenop de reeds ingelezen invoer.

### 17. Wat tel je als een vergelijking?

Een zoekvergelijking is een vergelijking tussen een tekstkarakter en een vast patroonkarakter. Een tabelvergelijking vergelijkt twee patroonkarakters tijdens de LPS-opbouw. Wildcardtests, lusvoorwaarden en setbewerkingen tellen niet mee. Hun uitvoeringstijd wordt wel gemeten. Daarom voorspelt het aantal karaktervergelijkingen de totale tijd niet exact.

### 18. Hoe heb je de performantie gemeten?

Ieder bestand wordt één keer ingelezen vóór het meten. De zoekfunctie wordt vijf keer gestart met `perf_counter()` en de mediaan wordt gerapporteerd. De voorbereiding, tellers en matchlijsten vallen binnen de meting; lezen en printen niet. De dia’s gebruiken de bestaande README-metingen van 14 en 15 oktober 2026 op Python 3.14.4 / Linux x86_64. Nieuwe tijden verschillen per omgeving. De vergelijkingstellingen en matches zijn opnieuw gecontroleerd.

### 19. Wat tonen je metingen niet aan?

Ze illustreren gedrag op gekozen voorbeelden. Met één vaste tekstlengte en vaste patroonlengtes toon ik geen asymptotische groeicurve aan. De complexiteitsgrenzen volgen uit de analyse van de lussen en het hergebruik. Voor een schaalonderzoek zou ik gecontroleerd verschillende waarden van `n`, `m` en bij wildcards `r` variëren.

### 20. Waarom is alleen brute-force met KMP vergelijken geen volledige correctheidscontrole?

Beide implementaties kunnen dezelfde fout bevatten. Daarom gebruik ik daarnaast een onafhankelijke regex-referentie met lookahead voor overlap en een optie die `.` ook regeleindes laat accepteren. Kleine invoer kan exhaustief worden getest; grotere en bijzondere invoer controleer ik met willekeurige en gerichte gevallen. Tests vervangen het correctheidsargument niet.

### 21. Welke verbeteringen zou je nog kunnen maken?

Identieke vaste stukken eenmaal zoeken en de resultaten voor hun verschillende offsets hergebruiken. Een zeldzaam of selectief stuk eerst zoeken kan kandidaten sneller uitsluiten. Hergebruik van de LPS-tabel is nuttig als hetzelfde patroon in meerdere teksten wordt gezocht. Streaming zou het inlezen van de volledige tekst kunnen vermijden, maar vraagt dat toestand en matches over blokgrenzen behouden blijven. Deze verbeteringen zijn voorstellen, geen claims over de huidige code.

### 22. Waarom Python? Wat is het effect daarvan op de resultaten?

Python maakt de algoritmen leesbaar en laat me de stappen rechtstreeks uitleggen. De zoekcode gebruikt geen externe packages. Absolute tijden omvatten Python-overhead en zijn dus niet zonder meer vergelijkbaar met C of C#. De asymptotische redenering over de gebruikte algoritmen blijft wel bruikbaar.

### 23. Wat was de taakverdeling en welke hulp gebruikte je?

Ik werkte alleen omdat de klas een oneven aantal studenten telde. Ik nam de analyse, implementatie, metingen, controles en verslaggeving op mij. Licht eerlijk toe welke ondersteuning je gebruikte. Volgens `Info.md` is AI toegestaan; presenteer AI-ondersteunde onderdelen en controles niet als volledig zonder hulp uitgevoerd. Zorg dat je elke keuze en elke regel die besproken wordt zelf kunt uitleggen.

## Vragen over de rest van hoofdstuk 7

| Techniek | Kernidee | Voorbeeld uit de cursus |
| --- | --- | --- |
| Brute-kracht | Alle mogelijkheden proberen | String matching, knapzak |
| Backtracking | Recursie stoppen zodra een deeloplossing ongeldig is | Zevenvakjespuzzel; te zware knapzak |
| Branch-and-bound | Tak stoppen als zelfs een optimistische grens de beste oplossing niet verbetert | 0/1-knapzak, TSP |
| Verdeel-en-heers | Kleinere problemen afzonderlijk oplossen en resultaten combineren | Merge sort, dichtste puntenpaar |
| Memoisatie | Recursief berekende subproblemen in een cache bewaren | Fibonacci |
| Tabulatie | Subproblemen iteratief in afhankelijkheidsvolgorde berekenen | Fibonacci, 0/1-knapzak |
| Greedy | Telkens kiezen wat nu het gunstigst lijkt | Munten wisselen, dichtste buur bij TSP |
| Local search | Vanuit een kandidaatoplossing betere buren zoeken | TSP met 2-opt; sudokuvoorbeeld |

**Is de mismatchstop bij brute-force backtracking?** Het is vroegtijdig stoppen met één poging. In de cursus is backtracking het afbreken van een tak in een recursieve zoekboom. Het terugvallen van de KMP-patroonindex is ook geen recursieve backtracking.

**Is de wildcardoplossing verdeel-en-heers?** Ze deelt een patroon op en combineert resultaten, maar volgt geen klassieke recursieve verdeel-en-heersstructuur. Benoem liever concreet wat de implementatie doet.

**Is KMP dynamisch programmeren omdat het een tabel heeft?** Alleen een tabel is daarvoor geen voldoende argument. De LPS-opbouw hergebruikt berekende prefixinformatie; beschrijf dat precies. Gebruik Fibonacci en knapzak als de expliciete cursusvoorbeelden van dynamisch programmeren.

**Backtracking versus branch-and-bound?** Backtracking sluit ongeldige deeloplossingen uit. Branch-and-bound richt zich op optimalisatie en sluit ook geldige takken uit die de beste reeds gevonden oplossing niet kunnen verbeteren. Daarvoor heb je een veilige, optimistische grens nodig.

**Waarom werkt greedy niet altijd bij munten?** Met munten 1, 2, 5, 10, 20 en 25 maakt greedy 40 met 25 + 10 + 5: drie munten. Twee keer 20 gebruikt maar twee munten. Voor kanonieke muntsystemen is greedy wel optimaal. Voor de DP-variant kun je `dp[0] = 0` en `dp[a] = 1 + min(dp[a-c])` gebruiken over bruikbare munten en bereikbare restbedragen.

**Waarom is knapzak-DP pseudo-polynomiaal?** De tabel kost `O(nW)`, met `W` de numerieke capaciteit. De invoerlengte voor `W` is bij binaire codering slechts evenredig met `log W`. De tijd is dus polynomiaal in de waarde van `W`, maar niet noodzakelijk in de lengte van de invoercodering.

**Wat is een lokaal optimum?** Een kandidaat waarvoor de gekozen buurfunctie geen betere buur oplevert. Dat hoeft geen globaal optimum te zijn. Mogelijke ontsnappingen zijn opnieuw starten, grotere buurten gebruiken of soms een slechtere stap toelaten, zoals bij simulated annealing.

## Korte demonstratie, alleen indien gevraagd

Voer vanuit de projectmap uit:

```powershell
python compare_algorithms.py
python compare_wildcards.py
```

Beperk je toelichting tot `aaaaab` en één wildcardvoorbeeld. Tijden veranderen; de matchposities en vergelijkingstellingen moeten gelijk blijven. Een live demonstratie is niet nodig om de hoofdpresentatie te begrijpen en zit niet in de richttijd.

## Laatste controle vóór het examen

- Oefen één volledige keer op de computer waarmee je presenteert, met de notities zichtbaar in de presentatorweergave.
- Gebruik de PDF als reserve wanneer de PowerPoint-opmaak niet goed wordt weergegeven.
- Zorg dat je `lps`, `n`, `m`, `k` en `r` zonder aarzelen kunt definiëren.
- Controleer eventuele aanvullende indieningsvoorwaarden op het leerplatform.
- Leer ook de overige cursushoofdstukken: de mondelinge vragen zijn niet tot ontwerptechnieken beperkt.
