# Demo
## Prepare
1. Start up Docker Desktop.
2. Start up Postman with the demo collection and variables preloaded.
3. Check if all tests successfully run
4. Start the app
5. Open AppHost.cs in Visual Studio and open Aspire-dashboard 

## Begin

```
Vandaag tonen we hoe we een gedistribueerde .NET-applicatie testen met Aspire en Testcontainers. Onze applicatie bestaat uit twee API's en een PostgreSQL-database. Reading List beheert de titels en hun leesstatus. Reading Summary vraagt die gegevens via HTTP op en berekent een samenvatting.

We tonen eerst de werkende applicatie. Daarna demonstreren we hoe we de database gericht testen en hoe we de volledige samenwerking tussen beide API's en de database controleren.
```

## 1. Showing the AppHost code and Aspire dashboard
Show the AppHost code and Aspire dashboard showing the two APIs and PostgreSQL as distinct resources.
1. Show the ReadingList.AppHost/AppHost.cs code and explain the resource references

```
Dit is de Aspire AppHost code. Hier wordt er beschreven welke onderdelen onze applicatie nodig heeft om te draaien en hoe ze met elkaar verbonden zijn.
Met 'AddPostgres' registreren we een PostgreSQL-server die in een Docker-container draait. Met AddDatabase voegen we daar onze database aan toe. Dit is een database binnen dezelfde server, geen extra container.
Met de twee AddProject-aanroepen registreren we Reading List en Reading Summary. Die draaien elk als een afzonderlijk API-proces.
Reading List krijgt via WithReference de configuratie om met de database te verbinden. Reading Summary krijgt via de reference de endpointconfiguratie om Reading List via HTTP te bereiken. Daardoor hoeven we die verbindingsgegevens niet zelf in de applicatiecode vast te zetten.
WaitFor regelt het wachten op de afhankelijkheden. De healthchecks controleren vervolgens of de API's klaar zijn om hun werk uit te voeren.
```

2. Start the AppHost

```
De AppHost is dus gestart, hier is de dashboard.
```

3. Open Aspire Dashboard and show the two APIs and PostgreSQL as three distinct resources

```
Hier zien we dus alle onderdelen die runnen, de twee APIs en de PostgreSQL server met de database. We kunnen per resource de status zien, logs bekijken en URLs zien. De healthchecks zijn geslaagd en de app is opgestart.
```

4. Explain how Aspire provides connection strings and serice discovery information through resource references

```
Reading List krijgt via de database-referentie een connection string waarmee die PostgreSQL kan bereiken. Reading Summary krijgt via de reference de endpointconfiguratie van Reading List, zodat die via HTTP kan communiceren. Aspire regelt dus dat de applicatiecode niet hardcoded hoeft te weten waar de andere onderdelen draaien. 
```

## 2. Showing the public workflow via Postman
Show the public workflow via Postman: create a title, retrieve a summary, finish the title, and retrieve the changed summary.

```
Nu zullen we de publieke workflow demonstreren met Postman, dus eerst de summary ophalen en zien dat er geen titels zijn, dan een titel aanmaken, de summary opnieuw ophalen en zien dat er 1 ongelezen titel is, dan de titel afwerken en de summary opnieuw ophalen en zien dat er 1 gelezen titel is.
```

Run the Postman requests in order that they are in the collection.

## 3. Showing that the Summary API calls Reading List through HTTP
Show that the Summary API calls Reading List through HTTP, not a shared database or an in-process method call.

```
De Reading Summary API roept de Reading List API aan via HTTP, niet via een gedeelde database of een methode die wordt opgeroepen. Dit kan je hier zien:
```

1. Show Summary API code using HttpClient to call Reading List API

```
Hier in src/ReadingSummary.Api/Program.cs zie je dat er een HTTP-client aangemaakt wordt. Die wordt dan gebruikt in de /summary endpoint om de Reading List API aan te roepen. De URL van de Reading List API wordt via de resource reference van Aspire opgehaald, dus die hoeft niet hardcoded te zijn.
```

2. Explain how Aspire service discovery resolves the Reading List API's address

```
Aspire Service Discovery resolved de Reading List API's adres dynamisch via de resource reference. Daardoor hoeft de app code niet te weten waar de Reading List API draait, dat regelt Aspire voor ons.
```

3. Show that the Summary API does not access PostgreSQL directly or call Reading List through an in-process method

## 4. Showing a focused Testcontainers test
Show the focused Testcontainers test that starts a fresh PostgreSQL instance and proves persistence works.
1. Show the Testcontainers integration test

Open tests/ReadingList.IntegrationTests/TitlePersistenceTests.cs :
```
Hier kun je onze Testcontainers integration test zien. Het start hier (lijn 14) een nieuwe PostgreSQL container en wordt er gewacht tot deze gestart is. De migrations worden uitgevoerd (lijn 35) en dan wordt een titel opgeslagen in de Databaseen wordt die gechecked of die er ook echt in staat door een ReadingListDbContext te gebruiken. Dan wordt de titel als 'finished' gemarkeerd en wordt er opnieuw gechecked of die status ook echt is opgeslagen. 
```

2. Run the test and demonstrate that it starts a fresh PostgreSQL container

```
Run de test en toon Docker Desktop terwijl de test draait.
```

3. Explain that the test uses the dynamically supplied connection string

```
Op lijn 25, 26 en 27 zie je dat de connection string wordt opgehaald en gebruikt om de DbContext te configureren. Die connection string wordt dynamisch door Testcontainers geleverd, dus die hoeft niet hardcoded te zijn.
```

4. Show that saving a title, reading it back and peristing its finished status work correctly

```
Je kan dus de test zien dat alles werkt. We lezen telkens met een nieuwe DbContext zodat we zeker weten dat de data echt in de database staat en niet in cache ofzo.
```

5. Explain that this test does not require the AppHost or Summary API

```
Deze test controleert de databasecode, EF Core, PostgreSQL driver en database zelf. De AppHost en API-servers worden niet gestart, dus dit is een gefocuste test die alleen de databasecode test. De titels enzo worden niet via de API gemaakt, maar direct door de classes, en dan opgeslagen in de database.
```

## 5. Showing an Aspire full-application test
Show the Aspire full-application test that starts the AppHost and proves the workflow crosses both APIs and PostgreSQL.
1. Show the Aspire full-application test using Aspire.Hosting.Testing

```
Voor de Aspire full-application test wordt de AppHost gestart en wordt er een volledige workflow uitgevoerd via de publieke HTTP endpoints. De AppHost wordt eerst gestart, en dat start de twee API's en de PostgreSQL server. Dan wordt er een titel aangemaakt via de Reading List API, en daarna wordt de summary opgehaald via de Reading Summary API. Dan wordt de titel als 'finished' gemarkeerd via de Reading List API, en daarna wordt de summary opnieuw opgehaald via de Reading Summary API. De test controleert dat alles correct werkt.
```

2. Run the test and demonstrate that it starts the AppHost and its resources

```
Als we de test runnen en kijken in Docker Desktop, zien we de tijdelijke PostgreSQL containers. De AppHost en API's zijn .NET processen en verschijnen daarom niet als containers. 
```

3. Show that the test performs the complete workflow through public HTTP endpoints, crossing both APIs and PostgreSQL

```
Zoals je ziet slaagt de test, dus dat betekent dat de volledige workflow correct werkt.
Als we de Asserts overlopen, zien we dat de summary correct is na het aanmaken van de titel, en dat de summary correct is na het afwerken van de titel. Dat bewijst dat de volledige workflow correct werkt.
```

4. Explain why this test proves more than the focused Testcontainers test

```
De vorige test bewees dat de database reading en writing werkt met een echte PostgreSQL database. Deze test bewijst daarnaast dat de afzonderlijke processen ook samen correct werken. Beide testen vullen elkaar aan, een databaseprobleem kan je dus gericht zoeken met Testcontainers, de volledige samenwerking controleren we met Aspire.
```

## 6. Showing one negative scenario and its expected response
Show the negative scenario where a title with only spaces is rejected with `400 Bad Request`

```
We handelen ook negatieve scenarios, zoals het aanmaken van een titel met alleen maar spaties. Dat wordt door de Reading List API afgewezen met een 400 Bad Request.
```

1. Send a POST /titles request with a title containing only spaces

```
Hier zenden we via Postman een request met een titel die alleen maar spaties bevat. Dat wordt afgewezen door de Reading List API met een 400 Bad Request.
```

2. Show that the Reading List API rejects that request with `400 Bad Request`

```
Hier zie je dat de Reading List API het request afwijst met een 400 Bad Request. Dat is de verwachte response voor dit negatieve scenario.
```

3. Show the corresponding automated negative test and its successful result

```
Hier in ReadingWorkflowTests.cs zie je ReadingList_WithInvalidTitle_ReturnsBadRequest, als je die runt, zie je dat die slaagt. Deze test controleert dat de Reading List API correct reageert op een ongeldig request met een 400 Bad Request. Het gebruikt een HttpClient om die request te sturen en controleert de response. Dat bewijst dat de API correct reageert op dit negatieve scenario.
```


## 7. Showing resource logs, health state, or a trace
Show resource logs, health state, or a trace used to explain one request's path.
1. Open Aspire dashboard

```
Nu gaan we tonen hoe we 1 verzoek door de verschillende onderdelen van de app kunnen volgen. Daarmee kunnen we onderzoeken waar er tijd verloren gaat of een fout optreedt.
```

2. Show resource logs and health states

```
Als we op de apis klikken kan je zien dat ze healthy zijn. Liveness controleert of het proces reageert. Readiness controleert of het zijn basiswerk kan uitvoeren. 
```

3. Demonstrate a request trace, preferably showing the HTTP call from the Summary API to the Reading List API

```
Nu gaan we een request trace tonen, namelijk de HTTP call van de Summary API naar de Reading List API. 

We gaan via Postman een GET /summary request sturen. Dan gaan we naar de Traces tab in Aspire Dashboard.

Bovenaan komt onze aanvraag binnen bij Summary. Die vraagt via HTTP de titels op bij Reading List.
De eerste aanroep krijgt een 307, een doorverwijzing naar HTTPS. De client volgt die automatisch en krijgt daarna 200 OK.
Onderaan zien we dat Reading List de database bevraagt. Summary gebruikt die gegevens om de aantallen terug te geven.
Zo volgen we één verzoek door beide API’s en de database. De balken tonen hoelang elke stap duurt.
```

4. Explain how this information helps diagnose problems in a distributed application

## Additional
Explain these points:
**Dynamic ports**: Explain that host ports are assigned by the runtime and why hard-coded ports make automated and parallel tests brittle.

```
Onze testen gebruiken geen vaste poorten. Aspire bepaalt tijdens het opstarten op welke poort de API bereikbaar is. De test maakt via de resourcenaam een HTTP-client met het juiste adres. Daardoor botsen tests minder snel met andere draaiende applicaties of met elkaar. De test moet dus de poort vooraf niet te kennen.
```

**Test isolation**: Explain why automated tests must start with an empty database and should not reuse persistent development data.

```
In TitlePersistenceTests.cs zie je 'Assert.Empty(await db.Titles.ToListAsync(timeout.Token));'. Dit controleert dat de database leeg is bij het starten van de test, zodat de test niet afhankelijk is van data die al in de database staat. Als er al data in de database staat, kan dat leiden tot foutieve testresultaten. Daarom starten we met een lege database en gebruiken we geen persistente ontwikkeldata.
```

**Readiness**: Explain why tests wait for explicit resource-ready or health signals instead of using Thread.Sleep.

```
In ReadingWorkflowTests.cs zie je 'await app.ResourceNotifications.WaitForResourceHealthyAsync("reading-list-api", timeout.Token);'. Een gestart proces is niet automatisch klaar voor requests. Daarom wordt er eerst gecheckt of de resource 'healthy' is voordat de test verder gaat. Dat is betrouwbaarder dan een Thread.Sleep, omdat de tijd die een proces nodig heeft om op te starten kan variëren.
```

**Aspire vs production**: Explain that the AppHost is used for development and test orchestration and does not replace production deployment design.

```
In dit project gebruiken we Aspire AppHost om beide API's en PostgreSQL samen te starten voor ontwikkeling en tests. Die regelt de verbindingsconfiguraties en dependencies. Dat is geen productie-deployment. Voor production moeten we ook bepalen waar alles draait, hoe we secrets beheren, data backuppen en updates uitvoeren. 
```

## Einde

```
We hebben nu gezien hoe één aanvraag door beide API's tot aan de database loopt. Met Testcontainers controleren we gericht of gegevens correct worden opgeslagen in echte PostgreSQL. Met de Aspire-applicatietest controleren we daarnaast of de volledige workflow via HTTP werkt.

Beide tests vullen elkaar aan: we kunnen opslagproblemen gericht onderzoeken en tegelijk de samenwerking tussen de onderdelen controleren. Daarmee hebben we de belangrijkste workflow van onze applicatie aantoonbaar getest. Zijn er nog vragen?
```

# Research questions

De vragen hieronder combineren de onderzoeksvragen uit dit draaiboek en README.md. Overlappende vragen staan eenmaal vermeld.

- Leg proces, service, endpoint, host, poort, container, afhankelijkheid, service discovery en gedistribueerde applicatie in gewone taal uit. Hoe verschillen deze begrippen en hoe hangen ze in deze toepassing samen?

```Een **proces** is een draaiend programma. 
Een **service** biedt een verantwoordelijkheid aan; Reading List beheert bijvoorbeeld titels. Beide API's draaien hier als afzonderlijke processen. 
Een **host** is de machine of netwerknaam waarop een onderdeel bereikbaar is. 
Een **poort** bepaalt bij welke luisterende toepassing het netwerkverkeer aankomt. 
Een **endpoint** is een bereikbaar adres; een HTTP-operatie gebruikt daarnaast een pad zoals `/titles`.
Een **dependency** is een afhankelijkheid: Reading List heeft PostgreSQL nodig, Summary heeft Reading List nodig. 
**Service discovery** koppelt een logische servicenaam aan een werkelijk adres. 
Samen vormen de API's en database een **gedistribueerde applicatie**: afzonderlijke onderdelen communiceren via een netwerk, ook als ze allemaal op dezelfde laptop draaien. 
Een container is een geïsoleerde uitvoeromgeving voor software, zoals onze PostgreSQL-server.
```

- Waarom kan een HTTP-aanroep tussen twee API's falen terwijl beide projecten compileren, en hoe verschillen de mogelijke fouten van die bij een methodeaanroep binnen hetzelfde proces?

```
Compileren controleert geen netwerkverbinding. Een verkeerd adres, onbereikbare service, certificaatprobleem, time-out, foutstatus of onbruikbare JSON kan de aanvraag laten mislukken. Een geslaagde compilatie bewijst evenmin dat beide API's hetzelfde HTTP-contract verwachten.

Een methodeaanroep binnen hetzelfde proces gebruikt geen netwerkverbinding of HTTP-serialisatie, maar kan wel een exception opleveren. Bij HTTP komen daar afzonderlijke processen, transportproblemen en onzekere uitkomsten bij: na een time-out weet de client bijvoorbeeld niet altijd of de server een wijziging al heeft uitgevoerd.
```

- Wat biedt een container en wat lost hij niet automatisch op?

```
Een PostgreSQL-container maakt het mogelijk een echte database met een gekozen image-versie te starten zonder PostgreSQL handmatig op de machine te installeren. Voor tests kunnen we een verse instance gebruiken en na afloop verwijderen. Een vaste image-tag maakt de gekozen versie expliciet.

Een container garandeert geen correct databaseschema, juiste configuratie, readiness, bereikbaarheid of testisolatie. Die regelen we met migraties, connection strings, readinesschecks, eigen testresources en opruiming. Docker moet beschikbaar zijn; containers maken infrastructuurtests niet onafhankelijk van een container-runtime.
```

- Wat beschrijft de Aspire AppHost en waarom hoort daar geen businesslogica in thuis?

```
De AppHost beschrijft PostgreSQL, de database, beide API-projecten, hun references en de voorwaarden om te starten en gezond te worden. Hij stelt de toepassing samen voor ontwikkeling en testen. Titelvalidatie en samenvattingsberekening horen in de API's, zodat die verantwoordelijkheden onafhankelijk van de orkestratie blijven.

Onze lokale AppHost is ook geen volledig productieplan. Zaken zoals hosting, geheimenbeheer, blijvende opslag, back-ups en updates vragen afzonderlijke keuzes. Aspire kan deployment ondersteunen, maar onze lokale configuratie alleen maakt de toepassing niet productieklaar.
```

- Welke verbindingsconfiguratie of informatie voor service discovery levert elke AppHost-reference?

```
`WithReference(readingListDatabase)` levert Reading List de databaseverbindingsconfiguratie. `AddNpgsqlDbContext` gebruikt daarvoor de resourcenaam `reading-list-db`. `WithReference(readingList)` levert Summary de endpointconfiguratie voor service discovery van `reading-list-api`. Summary krijgt geen database-reference.

Een reference levert configuratie. `WaitFor` regelt afzonderlijk het wachten op een afhankelijkheid. `WithHttpHealthCheck("/health/ready")` bepaalt welke HTTP-controle de AppHost voor de readiness van de API gebruikt.
```

- Waarom moet de Summary API het publieke HTTP-endpoint van Reading List aanroepen in plaats van rechtstreeks de database te gebruiken?

```
Reading List is verantwoordelijk voor de opslag en het publieke titelcontract. Via HTTP hoeft Summary het interne databaseschema niet te kennen. Rechtstreekse databasetoegang zou Summary koppelen aan de opslagkeuzes van Reading List en die servicegrens omzeilen. In ons project is de HTTP-grens bovendien nodig om de samenwerking tussen afzonderlijke processen aantoonbaar te testen.
```

- Waarom zijn dynamisch toegewezen poorten nuttig voor geautomatiseerde integratietests?

```
Een vrije hostpoort voorkomt conflicten met de demo, andere toepassingen of andere testomgevingen. Testcontainers levert het werkelijke databaseadres via `GetConnectionString()`. Aspire levert de API-adressen via `CreateHttpClient` op basis van de resourcenaam. Tests hoeven die poorten dus niet vooraf te kennen. Dit ondersteunt parallelle uitvoering, maar vervangt geen data-isolatie of correcte opruiming.
```

- Hoe vinden twee API's elkaar over een netwerk?

```
Summary gebruikt een HTTP-client met het basisadres `http://reading-list-api`. Dit is een logische servicenaam. De AppHost levert via `WithReference(readingList)` de werkelijke endpointconfiguratie. De service-discoveryhandler gebruikt die informatie om het juiste adres en de juiste poort te vinden. Summary verstuurt vervolgens `GET /titles` en verwerkt het JSON-antwoord. Er staat geen vaste hostpoort van Reading List in de applicatiecode.
```

- Welk concreet risico onderzoekt een integratietest en waarom kan een unit test dat niet afdekken?

```
Een integratietest onderzoekt of onderdelen correct samenwerken over een bepaalde grens. Onze PostgreSQL-test controleert bijvoorbeeld of migraties, EF Core-mapping, de database-driver en echte PostgreSQL samen gegevens correct bewaren. Een unit test met objecten in het geheugen voert geen echte SQL uit en kan fouten in die samenwerking niet aantonen. De Aspire-test onderzoekt daarnaast de samenwerking via HTTP tussen afzonderlijke processen.
```

- Welke echte componenten werken in de test samen: database, HTTP-server, serializer, message broker of bestandssysteem? Welke daarvan zijn voor dit project relevant?

```
De Testcontainers-test combineert het titelmodel, de normale DbContext, EF Core, de PostgreSQL-driver en echte PostgreSQL. Hij start geen API-server en gebruikt geen HTTP.

De Aspire-test gebruikt daarnaast de AppHost, beide API-processen met hun HTTP-servers, HTTP-clients, service discovery en JSON-serialisatie. Ons project gebruikt geen message broker of functionele bestandsopslag. Docker- en buildbestanden zijn infrastructuur; we testen hier geen afzonderlijk bestandscontract.
```

- Hoe houd je tests onafhankelijk van elkaar en van de machine? Denk aan verse data, dynamische poorten en opgeruimde resources.

```
Elke infrastructuurtest gebruikt eigen resources zonder gedeeld datavolume. De tests controleren een lege beginsituatie of dat ongeldige invoer de lijst leeg laat. Ze gebruiken runtime-adressen, wachten op readiness en begrenzen het wachten met cancellation tokens. Door `await using` worden resources bij normale uitvoering ook na een testfout opgeruimd. Testvolgorde en bestaande demodata mogen het resultaat niet bepalen. De juiste SDK, Docker en beschikbare images blijven voorwaarden voor uitvoering.
```

- Welke bewijskracht levert de Testcontainers-persistentietest die een unit test niet kan leveren?

```
Hij bewijst voor de geteste workflow dat het schema via migraties aangemaakt wordt, de tabel leeg begint en een titel werkelijk opgeslagen en teruggelezen kan worden. Na het afwerken blijft de status ook opgeslagen. Nieuwe DbContexts voorkomen dat we alleen objecten uit de oorspronkelijke EF Core-tracker controleren. De test bewijst geen HTTP-contracten of correcte service discovery.
```

- Welke bewijskracht levert de volledige Aspire-applicatietest die de Testcontainers-test niet kan leveren?

```
Hij bewijst dat de AppHost-configuratie en references de echte processen correct verbinden en dat de publieke HTTP-workflow werkt. De positieve test maakt een titel aan, vraagt Summary op, werkt de titel af en controleert de gewijzigde samenvatting. De negatieve test verwacht `400` voor een titel met alleen spaties en controleert dat de lijst leeg blijft. Dit geeft bewijs voor die scenario's, geen garantie voor elke mogelijke storing of een productie-deployment.
```

- Hoe kies je een testscope die past bij een concreet risico, en waarom vullen Aspire en Testcontainers elkaar aan in plaats van elkaar te vervangen?

```
Kies de kleinste scope die het risico werkelijk kan aantonen. Titelvalidatie en samenvattingsberekeningen testen we zonder infrastructuur. Voor SQL, migraties en persistentie gebruiken we echte PostgreSQL via Testcontainers. Voor service discovery en de samenwerking van beide API's via HTTP gebruiken we de volledige Aspire-applicatie. Testcontainers levert gericht databasebewijs; Aspire levert aanvullend bewijs voor de procesketen. Zo onderzoeken we een databaseprobleem zonder telkens alle API's te starten en missen we tegelijk de fouten tussen die API's niet.
```

- Waarom horen er minder volledige applicatietests te zijn dan unit tests en gerichte integratietests?

```
Volledige applicatietests starten meer infrastructuur, kosten meer tijd en hebben meer mogelijke foutoorzaken. Unit tests geven snelle, gerichte feedback voor veel varianten van validatie en berekeningen. Database-integratietests onderzoeken opslag gericht. We bewaren volledige applicatietests voor belangrijke workflows en risico's die alleen over de procesgrenzen zichtbaar worden; een vaste aantalsverhouding is geen doel op zich.
```

- Wat is het verschil tussen readiness, liveness en een HTTP-endpoint dat alleen een respons teruggeeft?

```
Liveness controleert hier of het API-proces HTTP-aanvragen kan verwerken, zonder afhankelijkheidschecks. Readiness controleert of het zijn basiswerk kan uitvoeren: Reading List bevraagt de titeltabel, Summary haalt via Reading List bruikbare titels op en berekent de samenvatting. Een willekeurig endpoint dat een vaste `200` teruggeeft, bewijst alleen dat die aanvraag beantwoord wordt, niet dat database of upstreamservice beschikbaar zijn. Een afhankelijkheidsfout hoort readiness te laten falen zonder daardoor automatisch liveness te laten falen.
```

- Waarom kunnen retries een tijdelijke fout opvangen, maar niet garanderen dat een gedistribueerd verzoek slaagt?

```
Een volgende poging kan slagen als een tijdelijke storing intussen verdwenen is. Bij een blijvende storing blijft ook die poging falen. Onze client staat maximaal één retry toe, dus maximaal twee pogingen, met vertraging en jitter. Een poging heeft een timeout van twee seconden en de totale pipeline vijf seconden; die limiet kan de uitvoering eerder beëindigen. Retries zijn uitgeschakeld voor onveilige HTTP-methodes om ongewenste herhaalde wijzigingen te vermijden.

De retry-test simuleert eerst `503` en daarna `200`; een andere test geeft blijvend `503` en controleert de begrenzing. Een HTTP-redirect, zoals `307` naar HTTPS in de demo, is iets anders dan een resilience-retry.
```

- Hoe onderzoek je een falende test met resourcelogs, healthinformatie en het Aspire-dashboard?

```
Lees eerst de gefaalde assertion en de volledige exception: faalt een verwacht resultaat, de verbinding of het wachten op readiness? Controleer daarna welke resource niet gestart of ongezond is en bekijk haar logs en die van haar afhankelijkheid. Een falende databasecheck vraagt een ander onderzoek dan een foutieve JSON-response. Tijdelijke foutlogs tijdens opstarten zijn op zichzelf geen gefaalde test; controleer het uiteindelijke testresultaat.

Bij de handmatig gestarte demo kun je één `GET /summary` versturen en de bijbehorende trace openen. Volg Summary, de uitgaande HTTP-aanroep, Reading List en de databasequery. Vergelijk duur en status per span en gebruik dezelfde trace-ID om de gestructureerde logs te koppelen. Let op automatische healthchecktraces en redirects, zodat je het juiste verzoek onderzoekt. Tijdens een Aspire-test is een geopend dashboardvenster niet vanzelfsprekend; de testoutput bevat ook resource- en healthchecklogs.
```
