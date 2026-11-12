# Oefening 1

In de syllabus wordt bij de introductie van de *backtracking*-techniek gebruik gemaakt van het volgende voorbeeld:

- Gegeven een array met 7 vakjes, waarvan sommige al ingevuld zijn met verschillende getallen tussen 1 en 7.
- Opgave: vul de resterende vakjes in met getallen tussen 1 en 7 zodat:
    - Elk getal precies 1 keer voorkomt.
    - Aangrenzende vakjes geen opeenvolgende getallen bevatten.

Er werd een aanzet gedaan voor de code die je hiervoor kan schrijven, maar die werd niet volledig afgewerkt.
Zo ontbreken er checks om na te gaan of de reeds vooringevulde vakjes niet gewijzigd werden in de oplossing,
en of aangrenzende vakjes geen opeenvolgende cijfers bevatten.

Implementeer een volledige *backtracking* oplossing voor dit probleem.

# Oefening 2

Het *branch-and-bound* algoritme voor het 0/1-knapzakprobleem dat in de syllabus wordt beschreven,
maakt in elke recursiestap een kopie van de huidige inhoud van de knapzak.
Dit is niet efficiënt qua geheugenverbruik en zorgt ook voor een worst case
tijdscomplexiteit die $O(2^n ⋅ n)$ is i.p.v. $O(2^n)$.

Pas de code uit de cursus aan zodat er geen kopieën gemaakt worden.

# Oefening 3

In de syllabus wordt een gretige aanpak beschreven voor het muntwisselprobleem.
Hierbij moet je bepalen wat het kleinste aantal munten is dat je nodig hebt om een gegeven bedrag te vormen.
Bij de gretige aanpak wordt als volgende muntstuk telkens de grootste munt gebruikt die kleiner of gelijk is aan het resterende bedrag.
Dit werkt optimaal voor zogenaamde *kanonieke* muntsystemen (zoals euromunten en de meeste andere echte muntsystemen).

Voor *niet-kanonieke* muntsystemen kan je een optimale oplossing berekenen via dynamisch programmeren.
De aanpak lijkt op deze die in de syllabus werd toegelicht voor het 0/1-knapzakprobleem (tabulatie).
Zoek op hoe dit algoritme werkt en implementeer het.

Een voorbeeld van een *niet-kanoniek* muntsysteem is een systeem met volgende munten:
1, 2, 5, 10, 20, 25, 50. Als je hier bv. het beddrag 40 wil maken, dan zal de gretige aanpak
hiervoor 3 munten gebruiken (25, 10 en 5) terwijl het ook kan met 2 munten (2 keer 20).
Het alternatieve algoritme via dynamisch programmeren moet hier wel in staat zijn de optimale oplossing te vinden.

