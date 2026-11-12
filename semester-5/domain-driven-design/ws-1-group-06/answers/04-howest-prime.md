# Part 4 - Improving Howest Prime

[Workshop instructions and submission checklist](../readme.md)

### Question 8: Identify the missing parts

Review the Howest Prime implementation used in class.

#### 1. Missing, incomplete, or implicit elements

Which architectural elements or responsibilities were missing, incomplete, or insufficiently explicit?

**Answer:**

1. Wanneer integratie events werden verzonden kon het zijn dat de verwerkende microservice faalt. Indien dit gebeurde werd de verzender van het event hier niet van op de hoogte gesteld. Zo geraakt het systeem in een gedeeltelijk aangepaste staat, zonder enig middel om dit op te lossen.
2. Onze integratie events werden verzonden na de database transactie. Dit is de correcte volgorde. Maar wanneer er zich een plotse networkfout voordoet dan kan er een situatie zijn waarin de database transactie gelukt is maar dit echter nooit gecommuniceerd werd naar andere microservices.   

#### 2. Proposed improvements

What improvements would you propose, and why?

**Answer:**

1. Compensating actions, met compensating actions worden er nieuwe events uitgestuurd die de voorgaande moeten compenseren (negatieve operatie)
2. Outbox patroon, het outbox patroon garandeert 1 publicatie van het event.

#### 3. Priorities

Which improvements would you prioritize?

**Answer:**

Gelijke hoogte
