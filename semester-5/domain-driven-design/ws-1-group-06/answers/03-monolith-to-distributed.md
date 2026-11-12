# Part 3 - Preparing a Monolith for Distribution

[Workshop instructions and submission checklist](../readme.md)

### Question 7: Design for a possible future transition

How would you structure a monolithic application today so that selected parts could be distributed in the future?

When would distribution be justified and when would retaining a monolith be preferable?

What do we need to be aware of when we actually go distributed?

**Answer:**

<br>
In duidelijke modules verdelen, dus je code groeperen per bedrijfsfunctie zoals bestellingen, voorraden, betalingen ...
Grenzen instellen om modules niet met elkaar rechtstreeks te praten of databases aan te raken. Enkel via duidelijke interfaces
Aparte database tabellen gebruiken, geen joins van een compleet andere tabel, en niet iets aanpassen in een tabel van een andere aggregate.
Events gebruiken en een message broker 

Monoliet enkel gebruiken wanneer het project nog nieuw is en snel moet veranderen en je een klein team hebt. 
Opsplitsen wanneer bepaalde onderdelen veel meer belastend zijn en apart moeten schalen voor het systeem, meerdere teams onafhankelijk van elkaar willen bouwen en uitrollen, en als je wil dat een fout in 1 onderdeel niet het hele systeem crasht.

Waar je rekening mee moet houden is netwerkvertraging en storingen. Het kan dus mislukken over het netwerk, dus je hebt retry en timeouts nodig. Ook kan je niet meer alles in 1 transactie scope opslaan, je moet eventual consistency hebben en patronen zoals het Saga patroon. Je gaat ook complexere monitoring hebben omdat het distributed is. Ook ga je meer infrastructuur nodig hebben zoals kubernetes, message queues, ci/cd per service ....
<br>
