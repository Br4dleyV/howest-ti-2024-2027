# Perceived performance

Een eenvoudige productzoeker met HTML, CSS en JavaScript. Open `index.html` in een browser (of met VS Code Live Server). Er is een internetverbinding nodig; geen installatie of API-sleutel.

## Twee versies

Kies een versie bovenaan. Wisselen laadt dezelfde zoekopdracht opnieuw.

- **A — Basic:** alleen de tekst “Laden…” tijdens het wachten. Daarna verschijnen alle producten tegelijk.
- **B — Enhanced:** zes skeletonkaarten geven alvast een indruk van de indeling. Een duidelijke laadtekst legt uit wat er gebeurt. De echte kaarten verschijnen met een korte overgang van 200 ms.

Beide versies gebruiken dezelfde zoekfunctie, API-aanroep en weergave van maximaal zes producten. Elke aanvraag wacht eerst **2500 ms**; daarna komt de echte netwerktijd erbij. De verbeterde versie haalt de data dus niet sneller op.

## Technieken en reflectie

1. **Skeleton screens:** tonen onmiddellijk de verwachte structuur. Het scherm voelt daardoor minder leeg en de gebruiker weet waar de resultaten zullen verschijnen. Het aantal placeholders is vast; het voorspelt niet hoeveel zoekresultaten er zijn.
2. **Betekenisvolle laadfeedback:** vertelt dat producten worden opgehaald en de kaarten nog worden ingevuld. Dit vermindert onzekerheid zonder een verzonnen voortgangspercentage te tonen.
3. **Subtiele overgang:** de kaarten verschijnen in 200 ms. Dit verzacht de verandering, maar is vooral visuele afwerking. De inhoud staat meteen in de pagina; er komt geen extra wachttimer bij.

Bij A is de wachttijd opvallender doordat alleen een korte laadtekst zichtbaar is. B geeft eerder structuur en feedback. Vooral de skeletons en laadtekst kunnen de wachttijd begrijpelijker maken. De animatie alleen lost weinig op en kan bij herhaald gebruik afleiden. Daarom blijft deze kort en schakelt ze uit bij minder beweging. Dit is een ontwerpverwachting, geen bewezen uitkomst van een gebruikerstest. Een vergelijking met echte gebruikers is nodig om te bepalen welke versie zij prettiger vinden.

## Toegankelijkheid en UI states

- Labels, native knoppen, een zichtbare focusrand en bediening met het toetsenbord.
- `role="status"` met `aria-live="polite"` kondigt laden, resultaten en fouten aan. Het resultatengebied gebruikt `aria-busy`.
- Skeletons zijn verborgen voor schermlezers met `aria-hidden`.
- `prefers-reduced-motion: reduce` schakelt zowel het pulseren als de overgang uit.
- **Laden:** tekst in beide versies; extra skeletons in B. De zoekbediening is tijdelijk uitgeschakeld om overlappende aanvragen te vermijden.
- **Succes:** titel, categorie, beschrijving en prijs in USD. De API-data blijft Engelstalig.
- **Leeg:** uitleg dat er geen resultaten zijn, met de suggestie een andere zoekterm te proberen.
- **Fout:** een begrijpelijke melding en een knop om opnieuw te proberen. Een verzoek dat na tien seconden nog niet klaar is wordt afgebroken.

## Vergelijken

1. Zoek in A op `phone` en herhaal dit in B.
2. Let op hoe de gelijke extra wachttijd aanvoelt.
3. Zoek op `zzzz-no-such-product` voor de lege toestand.
4. Zet de browser offline en zoek opnieuw voor de fouttoestand. Zet de verbinding terug aan en kies **Opnieuw proberen**.
5. Activeer minder beweging in je systeem of browserontwikkeltools om de statische skeletons te controleren.

## API en controles

Bron: [DummyJSON productdocumentatie](https://dummyjson.com/docs/products).

`GET https://dummyjson.com/products/search?q=phone&limit=6&select=title,description,category,price`

De app leest de array `products` uit het JSON-antwoord. Live aanvragen zonder authenticatie zijn gecontroleerd voor `phone` (zes resultaten) en een onbekende zoekterm (lege array).

Gecontroleerd in Chrome: beide versies tonen zes producten via de echte API. Met de lokale simulatie zijn gelijke inhoud, de vertraging, skeletons, lege resultaten, HTTP-fouten, opnieuw proberen en een time-out gecontroleerd. Ook minder beweging en een schermbreedte van 375 px zijn gecontroleerd. Er waren geen JavaScript-fouten. De live-regio is in de code aanwezig; de aankondigingen zijn niet met een echte schermlezer getest.

De lokale Postman-simulatie is uitsluitend bedoeld om fouttoestanden herhaalbaar te controleren. Start vanuit deze map met:

```powershell
postman simulation run postman/simulations/products.sim.yaml --proxy --proxy-port 7017 --no-history
```

Deze proxy vervangt alleen `dummyjson.com`. Configureer de testbrowser om de proxy en de tijdelijke proxycertificaten te gebruiken. De mock geeft een vast voorbeeldproduct; `zzzz-no-such-product` geeft een lege lijst, `__error__` injecteert HTTP 503 en `__timeout__` laat het verzoek hangen. De laatste twee zijn testinvoer voor de mock, geen functies van de echte API. De mock is gebaseerd op de documentatie en een live antwoord; zoeken naar een mock van de eigenaar is overgeslagen omdat Postman niet is aangemeld. Er is niets gepubliceerd.
