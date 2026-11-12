# Part 1 - Architectural Foundations

[Workshop instructions and submission checklist](../readme.md)

### Question 1: Benefits and trade-offs

Explain the main benefits and trade-offs of:

1. Ports and Adapters Architecture, also known as Hexagonal Architecture.
2. Clean Architecture.
3. Domain-Driven Design (DDD).

**Answer:**

<br>
1. 
Voordelen: Het is makkelijker testbaar omdat je geen database of webserver moet opstarten. Je kan makkelijker implementaties veranderen, bijvoorbeeld van MongoDB naar PostgreSQL zonder de hele applicatie opnieuw te programmeren.

Nadelen: Je moet meer interfaces aanmaken en meer bestanden enzo aanmaken en daardoor duurt het ook allemaal langer om te developen.

2. 
Voordelen: De business logica is niet afhankelijk van de technische implementaties, dus is het makkelijker om regels op te stellen en de code is beschermd. Aparte teams kunnen afzonderlijk aan een bepaalde laag werken, zonder de gehele codebase te moeten begrijpen.

Nadelen: Nieuwe features toevoegen gebeurt over meerdere lagen. Hierdoor duurt het opnieuw langer om te developen en dan is het ook duurder qua development kosten (lonen etc).

3.
Voordelen: De business experts en de ITers gebruiken dezelfde taal (ubiquitous language). Zo kunnen ze beide verstaan wat de code doet/waarvoor het dient. Het programmeren gaat veel sneller gaan omdat je alles begrijpt qua business. Latere aanpassingen gebeuren gemakkelijker. Je kan de code in bounded contexts opsplitsen zodat de verschillende contexten van eenzelfde onderwerp makkelijker te onderscheiden is en ook hier dezelfde taal gesproken wordt

Nadelen: Dat neemt heel veel tijd in beslag in het begin, omdat je dan de hele business moet analyseren en verstaan voordat je kan beginnen met programmeren. Stakeholders zien hier nog niet vaak het nut van in.
<br>

### Question 2: Applying Clean and Hexagonal Architecture

Consider an application that combines DDD with Clean Architecture or Ports and Adapters.

#### 1. Scalability

How does this approach support growth in functionality, domain complexity, and development teams?

**Answer:**

Dankzij het afzonderen van de business rules in de domain laag kan deze gemakkelijk evolueren naarmate de eisen veranderen en groeien. Het opsplitsen van de lagen en de onafhankelijkheid van bovenliggende lagen maakt het mogelijk dat één team zich toespitst op het begrijpen van de business concepten, terwijl een ander team (later) kan werken aan de infrastructure laag, zonder een diepe kennis van de gehele applicatie. Bounded contexts zorgen er verder voor dat verschillende teams aan verschillende delen van de applicatie werken, aangezien deze via duidelijk afgesproken manieren met elkaar communiceren waar nodig.
<br>

#### 2. Testability

What makes the application testable?

**Answer:**

<br>
Bij DDD en Clean Architecture is de Domain layer de absolute eigenaar van de code. Door DDD maak je eerst de domain layer, en door clean architecture is domain niet afhankelijk van de andere lagen, maar omgekeerd. Dat betekent dat je makkelijker kan testen omdat alle logica in de domain layer zit, in plaats van overal gescattered.

Bij DDD en Ports & Adapters is de Domain layer de eerste die je maakt. Zo kan je al de hele applicatie maken zonder een technische implementatie te voorzien. De infrastructuur laag moet je dan pas later voorzien. Het maakt testen dus ook makkelijker omdat je dan dingen zoals mock repositories kan aanmaken met die ports in plaats van met de productie database te moeten testen. Zo moet je niets opzetten van databases of web servers om die te testen.
<br>

#### 3. Design focus

In an application combining DDD with Clean or Hexagonal Architecture, what should drive the design: the domain, application use cases, or infrastructure concerns? Explain your reasoning and how the direction of dependencies supports your chosen focus.

**Answer:**

<br>
De domeinlaag komt altijd eerst. Daarna dient nagedacht te worden wat de applicatie juist dient aan te bieden. Dit gebeurt in de applicatielaag. Als laatste stap implementeren we pas de infrastructuur laag. Op deze manier kunnen we voorkomen dat 1 enkele verandering op vlak van  infrastructuur een verandering in domein teweegbrengt.
<br>

#### 4. Large applications

Could this approach support a large festival platform, such as one serving Tomorrowland? State your assumptions about its requirements; you do not need to know Tomorrowland's actual architecture.

**Answer:**

Ja, dit zou kunnen, maar dan ga je meer moeten focussen op de infrastructuur laag, dat die geoptimaliseerd is en heel performant is, want anders kan die crashen of niet genoeg mensen toelaten. De domain layer is ook belangrijk hiervoor, om te maken dat er niets fout gaat qua inschrijvingen/betalingen, maar infrastructuur is toch nog iets belangrijker, dus als je nu technische implementaties prioriseert kan het geen kwaad. Ook kan je hier goed gebruik maken van bounded contexts: ticketing, lineup, camping...
<br>

#### 5. Small applications

Is this approach also suitable for small applications? Explain when its structure adds value and when it may introduce unnecessary complexity.

**Answer:**

Het starten vanuit de business rules en domeinlaag is altijd zinvol, maar het opzetten van de volledige structuur kan een onnodige tijdslast met zich meedragen bij zeer minieme applicaties. Er moet natuurlijk wel over nagedacht worden of de applicatie later niet toch complexer zou worden of meer features zou vragen. In dit geval is een goede structuur van bij het begin een betere keuze omdat deze vaak makkelijker uit te breiden is. Moest de small application toch verder scalen kan je altijd overschakelen want dan voegt de complexere structuur wel degelijk iets toe.

<br>