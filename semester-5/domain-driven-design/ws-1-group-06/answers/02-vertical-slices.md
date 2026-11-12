# Part 2 - Critical Analysis of Vertical Slice Architecture

[Workshop instructions and submission checklist](../readme.md)

### Question 3: Evaluate Jimmy Bogard's approach

Read [Vertical Slice Architecture by Jimmy Bogard](https://www.jimmybogard.com/vertical-slice-architecture/).

#### 1. Central idea

Explain the central idea of Vertical Slice Architecture in your own words.

**Answer:**
<br>
Het centrale idee achter vertical slices is dat we onze code gaan organiseren en groeperen rond features, en niet rond technische lagen. Ze gaan van het koppelen tussen features naar zo veel mogelijk koppelen binnen de features. ~~Op deze manier wordt een volledige feature op één enkele plaats omschreven en voorkomen we fragmentatie.~~
<br>

#### 2. Debatable, unclear, misleading, or incorrect claims

Identify at least three claims that you consider debatable, unclear, misleading, or incorrect. For each, explain the author's argument and give a reasoned response.

**Answer:**

<br>
1. Hij zegt dat je met de Vertical Slice Architecture abstracties wegdoet en dat je dan niet de shared layer nodig hebt en abstracties zoals Repositories, Services, Controllers enzo, maar door die abstracties en shared folders is er veel minder code duplicatie en is er consistentie. Je kan in de ene slice de database op de ene manier aanspreken, en in een andere slice op een andere manier, wat moeilijk te verstaan is en ook moeilijk te debuggen als er een probleem is.

2. Hij gebruikt vertical slicing om sneller features te implementeren. Het is misschien initieel sneller, maar veel minder overzichtelijk en onderhoudbaar. Hoe verder in het project je zit, hoe duurder en lastiger en moeilijker het gaat zijn om het project te onderhouden.

3. 'Clean Architecture is sowieso een monolith'. Dit is niet waar. Hij denkt dat de architectuur & mappenindeling hetzelfde is als de uitrol van de applicatie. Slices zijn vaak ook een monoliet omdat je alle slices in 1 project steekt. Bij Clean Architecture kan je later nog altijd makkelijk loskoppelen.
<br>

#### 3. Relationship to DDD

To what extent do the examples reflect the DDD principles studied in class? Identify where they align and where they differ.

**Answer:**

<br>
Wij starten telkens vanuit de business rules in de domeinlaag. VSA begint te denken vanuit de applicatielaag, wat de applicatie moet doen qua features. Ze laten het praten met en begrijpen van de domeinexperts achterwege.
<br>

#### 4. Repeated refactoring

What will happen as a team repeatedly refactors a growing application built around vertical slices? Explain your reasoning.

**Answer:**
<br>
Dan krijg je uiteindelijk ook gewoon een clean architecture. Dit komt doordat de developers voor de keuze komen om te stoppen met duplicaten (omdat dit risico's met zich meebrengt) en de gedeelde logica gaan samenbrengen op een centrale plaats.
<br>

### Question 4: Evaluate the implementation and its risks

Read [Vertical Slice Architecture in .NET by Adrian Bailador](https://adrianbailador.github.io/blog/47-vertical-slice-architecture/).

#### 1. Explicitly identified concerns

Summarize the concerns and pitfalls explicitly identified in the article.
**Answer:**

<br>
- De shared folder. De hele bedoeling van Vertical Slice Architecture is om de shared folder te voorkomen en weg te werken. Maar toch gaat er shared code zijn en zal er dus ook een shared folder zijn.
- Business rules in de endpoint handler/controller plaatsen. Deze hoort enkel een mediator/handler te callen, die dan op zijn beurt de validatie en business logica bevat.
- Een god slice zoals ProductManagement slice. Dit is te groot en doet alles dat kan. Een oplossing ervoor is opsplitsen maar dan kom je weer dichter bij clean architecture. 
- Als er twee keer dezelfde code wordt aangetroffen wordt deze per direct naar de algemene map verplaatst.
<br>

#### 2. Additional concerns

Identify additional concerns you see in the examples. Clearly distinguish your observations from the author's stated concerns.

**Answer:**

<br>
- De requesthandler bevat business logica. Zo combineer je het nut van een use case met het nut van een request handler, waardoor de business logica afhankelijk wordt van de request. Zo is het niet meer mogelijk om dezelfde command/query aan te spreken vanuit verschillende entrypoints (API/CLI/...).
<br>

#### 3. What drives the design?

What appears to drive the design of the examples: infrastructure concerns, domain modelling, or application use cases? Support your answer with examples.

**Answer:**

<br>

Applicatie use-cases bepalen duidelijk de structuur van de applicatie. De manier waarom de features wordt gegroepeerd toont telkens update, create...  

<br>

#### 4. Relationship to DDD

To what extent do the examples reflect the DDD principles studied in class? Identify where they align and where they differ.

**Answer:**

<br>
Net zoals bij DDD staan de namen van commands voor een echt businessactie (bv. CreateProduct). 
<br>

### Question 5: Assess the following concerns in vertical slicing

#### A. Code duplication and violations of DRY

**Answer:**

<br>
Dupliceren is oke zolang dat over oppervlakkige duplicatie gaat maar zodra het begint te gaan over echte domain logic/kennis die gedupliceert wordt die consistent moeten blijven zullen die duplicates langzamerhand uit sync geraken.
<br>

#### B. Loss of domain cohesion

**Answer:**

<br>
Omdat slices vrij zijn om met data om te gaan hoe zij willen kan het zijn dat eenzelfde domeinconcept niet overal hetzelfde betekent. Dit staat lijnrecht tegenover waar DDD voor staat. domaincohesie gaat dan dus verloren vanaf de moment dat er meerdere slices andere betekenissen bevatten van eenzelfde domainconcept.
<br>

#### C. Risk of a "Big Ball of Mud"

**Answer:**

<br>
Vertical Slice architecture moedigt aan om snel features te bouwen met zo weinig mogelijk abstracties, maar naarmate een applicatie groeit en er honderden slices zijn, onstaat er het risico dat er geen duidelijke overkoepelende architecturele grenzen meer zijn.
Elke slice gaat direct naar de database kunnen grijpen en random entiteiten kunnen bewerken, dus dan is er te veel door elkaar, een big ball of mud.
<br>

#### D. Vertical Slicing looks more like layered architecture

**Answer:**

<br>
Een Vertical Slice architecture lijkt gewoon op een miniatuurversie van een layered architecture. Binnen 1 slice zie je nog altijd technische stappen (een endpoint -> command -> db call ...)
In plaats van lagen te elimineren brengt het gewoon de mappenstructuur over van lagen naar individuele features. Hieredoor blijven nog altijd die technische stappen.
<br>

### Question 6: Take and defend a position

Accept or reject the following statement. You may qualify your position, but your conclusion must be clear.

> End-to-end Vertical Slice Architecture should not be used as the foundation of production software. Even when developers do not explicitly assume a small, stable domain, the architecture implicitly makes that assumption by treating requests as independent.
>
> It appears fast while there are few slices, but shared business rules, state, and transactions eventually expose hidden coupling. Teams must then introduce or restore aggregates, ports, adapters, transaction boundaries, policies, and dependency inversion - effectively moving towards Clean or Hexagonal Architecture - or risk maintaining a Big Ball of Mud.
>
> These boundaries should be established from the beginning. The apparent early speed represents deferred architectural work. Use cases may be grouped within the application layer, but slices should never cross architectural boundaries.

Your response must include:

- Your interpretation of **end-to-end vertical slicing** and **architectural boundaries**.
- Arguments supporting your conclusion.
- At least one credible counterargument.

Your work will be assessed on the quality of your reasoning and evidence, rather than agreement with the statement.

**Answer:**

<br>
Interpretatie: onder "end-to-end vertical slicing" verstaan we een aanpak waarbij slive volledig autonoom is van http-endpoint tot databanktoegang zonder gedeelde domeinlaag. Ook niet voor kernregels die eigenlijk overal gelden

Standpunt: We zijn het grotendeels eens met de stelling, met een nuancering.


Argumenten die het standpunt ondersteunen: Beide artikels tonen, elk op hun eigen manier, dat pure end-to-end VSA niet standhoudt zodra een applicatie groeit. Bogard erkent zelf dat complexe logica uiteindelijk naar het domein moet verhuizen. Bailador raadt letterlijk aan om VSA te combineren met een domeinlaag of DDD zodra de logica complex of gedeeld wordt, en beschrijft concrete symptomen (god slices, logica die weglekt, de Common-map als vuilnisbak) die precies wijzen op verborgen koppeling die alsnog beheerd moet worden. Dat bevestigt het kernargument van de stelling. Namelijk dat de schijnbare snelheid van slices deels uitgestelde architecturale arbeid is, die later alsnog moet gebeuren.

Credibel tegenargument: De stelling dat grenzen "van bij het begin" moeten vastgelegd worden, is zelf ook een risico: het kan leiden tot voorbarige abstractie voor regels die uiteindelijk nooit echt gedeeld blijken te zijn. Bailador's eigen "Rule of Three" is hier relevant niet elke gelijkenis tussen twee slices duidt op een echte, blijvende gedeelde regel. Soms is het toeval, en groeien de slices juist uit elkaar. Voor kleine, kortlevende applicaties of features die echt onafhankelijk zijn kan het vooraf opleggen van volledige Clean/Hexagonal-grenzen onnodige complexiteit en vertraging betekenen.

Conclusie: We aanvaarden de stelling voor elke applicatie met businesslogica die naar verwachting zal groeien of gedeeld zal worden over meerdere features. Daar is vooraf grenzen trekken de verstandigere investering. We gaan enkel niet akkoord met het universele, uitzonderingsloze karakter van de stelling: voor kleine, stabiele, echt onafhankelijke use cases blijft end-to-end VSA een geldige, pragmatische keuze, zolang het team bewust de afweging maakt en niet blind aanneemt dat die onafhankelijkheid blijft bestaan naarmate het project groeit.
<br>
