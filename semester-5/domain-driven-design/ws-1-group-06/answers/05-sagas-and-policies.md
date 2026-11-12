# Part 5 - Sagas and Policies

[Workshop instructions and submission checklist](../readme.md)

### Question 9: Classify the policies in Howest Prime

Examine the policies implemented in Howest Prime.

#### 1. Saga behavior

Do they implement saga behavior? Explain the criteria you use to decide.

**Answer:**

<br>
Ja. Een saga breekt een grootschalige sequentie van events op in aparte transacties, vaak over bounded contexts heen. Ze staan toe om een grote actie op te splitsen in kleinere bouwstenen, die zo onderling afhankelijk kunnen blijven. Dit is net wat policies ook doen. Als je voor een bepaalde usecase meerdere aggregates wil aanpassen, of andere neveneffecten wil veroorzaken, dan splits je deze op in aparte use cases. Zo past elke use case één aggregate aan, en worden daarna nog andere use cases uitgevoerd, om zo de andere veranderingen te realiseren.
<br>

#### 2. Internal sagas

If you classify any of these policies as internal sagas, what kind of saga behavior do they implement? Explain your reasoning.

**Answer:**

<br>
Communication axis:
Het gaat hier over asynchrone saga's. Use cases callen niet zelf andere use cases. Dit gebeurt achteraf via publishers en subsribers.

Coordination axis:
Het gaat hier over choreography-based saga's. Er is geen centrale orchestrator die opeenvolgend de verschillende acties uitvoert en luistert naar results. Use cases publishen events en policies kiezen zelf om te reageren door een volgende use case aan te spreken.

Consistency axis:
Het gaat hier over eventual consistency. Elke use case heeft zijn eigen transactie, die telkens kan slagen of falen. Zo kan het dus zijn dat één deel van de data al is veranderd volgens een bepaalde actie van de gebruiker, terwijl een ander deel van de data dat hierop gebaseerd is nog niet is veranderd (data drift).

Policies zoals in Howest Prime zijn dus een voorbeeld van een Anthology Saga.
<br>