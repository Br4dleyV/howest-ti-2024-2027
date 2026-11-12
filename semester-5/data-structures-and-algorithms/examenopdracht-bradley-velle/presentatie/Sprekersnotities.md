# Sprekersnotities ? String matching

De eerste 12 dia?s vormen de presentatie. Richttijd: **9 min 30 s**. Dia?s 13?16 zijn verborgen reserve voor vragen. Dezelfde notities staan in PowerPoint bij de dia?s.

De opdracht is individueel uitgevoerd wegens een oneven aantal studenten. De meettijden zijn historische resultaten uit de README.

## Dia 1 ? String Matching (0:00?0:25)

Ik heb string matching uitgewerkt: zoeken waar een patroon in een tekst voorkomt. Ik begin met de eenvoudige brute-forceaanpak uit de cursus. Daarna toon ik hoe KMP eerder vergelijkingswerk hergebruikt. Tot slot bespreek ik mijn uitbreiding met vraagtekens als wildcards en wat die doet met de performantie.

## Dia 2 ? Agenda (0:25?0:45)

Ik begin met de doelstelling en mijn aanpak in Python. Daarna leg ik brute-force en KMP uit. Vervolgens bespreek ik de wildcards en waarom die een andere aanpak vragen. Ik eindig met mijn meetresultaten, problemen en conclusie.

## Dia 3 ? Doelstelling en verwachte resultaten (0:45?1:25)

De opdracht is een tekstbestand doorzoeken op een woord of patroon. Ik implementeer brute-force en KMP, vergelijk hun karaktervergelijkingen en uitvoeringstijd en breid beide uit met het vraagteken als wildcard. In het korte voorbeeld kat kat komt kat voor op startposities 0 en 4. Ik zoek alle matches, ook overlap: aaa in aaaaa geeft 0, 1 en 2. De zoekopdracht is hoofdlettergevoelig. Een leeg patroon weiger ik en een te lang patroon kan nergens passen.

## Dia 4 ? Mijn aanpak in Python (1:25?2:05)

Ik koos Python omdat de lussen daarin leesbaar blijven. Mijn programma gebruikt tekstbestanden uit input_data. De bestandsnamen en patronen staan in voorbeeldlijsten in de scripts, dus ik kan de input aanpassen zonder het zoekalgoritme te wijzigen. Beide algoritmen krijgen exact dezelfde tekst en hetzelfde patroon. KMP splitst de tabelopbouw en het zoeken in aparte functies. De matchposities bewaar ik in een Python-lijst. De uitvoer bevat ook de vergelijkingstellingen en de mediane tijd. De vergelijkingsscripts controleren dat beide algoritmen dezelfde startposities teruggeven.

## Dia 5 ? Brute-force als eenvoudige baseline (2:05?3:00)

Brute-force is de eenvoudigste aanpak. Ik probeer het patroon op de eerste plaats in de tekst. Ik vergelijk de letters één voor één. Zodra een letter verschilt, schuif ik één plaats op en begin ik opnieuw. Hier zoek ik "aaaaab" in een tekst met alleen a’s. De eerste vijf letters kloppen. Pas bij de b gaat het fout. Op de volgende plaats controleer ik opnieuw vijf a’s en daarna de b. Er wordt dus veel werk herhaald. In het slechtste geval controleer ik op bijna elke tekstpositie het hele patroon. Daarom is de tijdscomplexiteit O(n × m): n is de tekstlengte en m de patroonlengte. Als de eerste letter al verschilt, ben ik sneller klaar met die poging.

## Dia 6 ? KMP: Knuth-Morris-Pratt (3:00?4:15)

KMP staat voor Knuth-Morris-Pratt en voorkomt dat ik bekende letters opnieuw vergelijk. Het patroon ABABAC heeft LPS-waarden 0, 0, 1, 2, 3, 0. LPS geeft de lengte van het langste echte voorstuk dat ook een eindstuk is. Echt betekent korter dan het volledige stuk zelf. Stel dat ABABA al matcht en de volgende tekstletter een B is in plaats van de verwachte C. ABABA eindigt op ABA, dat ook het begin van het patroon is. Daarom blijven drie letters bruikbaar. Ik zet de patroonindex op 3 en vergelijk hetzelfde tekstteken met de B op die positie. De tekstindex gaat niet achteruit. build_lps maakt de tabel in O(m), de zoekfase kost O(n), dus samen O(n + m). Een tekstteken kan daarbij wel meer dan één vergelijking vragen. Na een match gebruikt KMP de tabel ook om overlap te behouden.

## Dia 7 ? Hoe wildcards werken (4:15?4:50)

Een vraagteken in het patroon staat voor precies één willekeurig teken. Met go?d vind ik goed, goud en goad. god heeft een teken te weinig en goood een teken te veel. Alleen het patroon kent wildcards. Brute-force accepteert de wildcard en vergelijkt de vaste letters zoals voorheen. Bij KMP wordt het moeilijker, want de prefix-tabel steunt op exacte gelijkheid. Die kan ik niet zomaar vervangen door wildcardovereenkomst.

## Dia 8 ? Probleem met KMP en mijn oplossing (4:50?5:55)

Het lastigste probleem was de uitbreiding van KMP met wildcards. Een vraagteken kan a en b accepteren, maar a en b zijn daarom niet gelijk. In de tekst abb matcht a? op start 0, maar niet op start 1. Na de eerste match mag ik de b dus niet zomaar als een nieuwe a hergebruiken. Mijn oplossing verdeelt het patroon in vaste stukken en onthoudt hun offsets. Voor go?d zoek ik go met offset 0 en d met offset 3. go op tekstpositie 5 geeft start 5. d op tekstpositie 8 geeft ook start 8 min 3 is 5. Met sets houd ik alleen starts over die door alle stukken worden bevestigd en waarvoor het hele patroon in de tekst past. De wildcard zelf hoeft niet op waarde gecontroleerd te worden. Dit gebruikt gewone KMP voor ieder vast stuk.

## Dia 9 ? Performantie zonder wildcards (5:55?7:00)

Ik heb beide algoritmen op dezelfde tekst en hetzelfde patroon getest. De tekst bevat honderdduizend a’s. Het patroon is "aaaaab" en wordt nergens gevonden. Brute-force vergelijkt bijna zeshonderdduizend keer twee letters. KMP doet dat ongeveer tweehonderdduizend keer, inclusief het maken van de tabel. Dat is ongeveer drie keer minder. In deze meting duurde brute-force ongeveer 69 milliseconden en KMP ongeveer 24 milliseconden. Ik voerde iedere zoekopdracht vijf keer uit en nam de middelste tijd. Het lezen van het bestand telde niet mee. De tabel maken telde wel mee. KMP is vooral nuttig als er veel gedeeltelijk gelijke stukken zijn. Het is niet op elke tekst automatisch sneller. De exacte meetcijfers komen uit mijn README; op een andere computer kunnen de tijden verschillen.

## Dia 10 ? Performantie met wildcards (7:00?7:55)

Met wildcards is mijn uitbreiding niet altijd sneller dan brute-force. Bij "a?a?a?" zijn er drie vaste stukken: drie losse a’s. Mijn code kan daardoor drie keer door de tekst zoeken. Daarna moet ze de gevonden plaatsen ook nog combineren. In mijn meting duurt dat ongeveer 129 milliseconden, tegenover ongeveer 71 voor brute-force. Bij "??????" zijn er juist geen vaste stukken. Elk stuk tekst van zes tekens past dan. Mijn KMP-uitbreiding kan meteen alle passende beginplaatsen teruggeven. Dat duurt hier ongeveer 2 milliseconden, tegenover 49 voor brute-force. Het hangt dus niet alleen af van hoeveel ? er staan, maar ook van waar ze staan. De uitgebreidere formule staat op een reservedia voor vragen.

## Dia 11 ? Problemen en controles (7:55?8:45)

Het belangrijkste ontwikkelprobleem was dat gewone LPS-terugval met wildcards foute aannames kan maken. Dat loste ik op door exacte stukken met offsets te combineren. De oplossing heeft een keerzijde: meerdere vaste stukken kunnen meerdere zoekrondes door de hele tekst vragen. Daarom vergelijk ik ook de uitvoeringstijd. De vergelijkingsteller omvat alleen vaste karaktervergelijkingen en tabelvergelijkingen. Wildcardtests, luscontroles en setbewerkingen vallen wel in de gemeten tijd. Voor correctheid controleerde ik 15.240 kleine en 5.000 willekeurige gevallen met een onafhankelijke zoekmethode via reguliere expressies. Die 20.240 gevallen slaagden. Ik controleerde ook overlap, lege tekst, te lange patronen en bijzondere tekens.

## Dia 12 ? Conclusie en taakverdeling (8:45?9:30)

Brute-force geeft een eenvoudige baseline: op iedere startpositie opnieuw proberen. KMP gebruikt de LPS-tabel om herhaling te vermijden en blijft zonder wildcards lineair in n plus m. Mijn wildcarduitbreiding zoekt vaste stukken met KMP en combineert hun startposities. De efficiëntie hangt daardoor ook af van het aantal en de positie van die stukken. Meer wildcards maakt mijn programma dus niet automatisch sneller of trager. Ik voerde de opdracht individueel uit wegens een oneven aantal studenten. Ik nam de analyse, implementatie, metingen, tests en documentatie op mij, met ondersteuning van hulpmiddelen die ik bij vragen kan toelichten.

## Dia 13 ? Reserve · een prefix-tabel stap voor stap (reserve)

Deze dia gebruik je alleen bij vragen. Voor ababaca zijn de LPS-waarden 0,0,1,2,3,0,1. Bij index vijf staat c. Vergelijk met patroonindex drie: b, mismatch. Val terug naar LPS van index twee: één. Vergelijk met patroonindex één: b, weer mismatch. Val terug naar nul. Vergelijk met a, mismatch: de waarde blijft nul en de index gaat vooruit. Na de laatste a krijg je één. De opbouw gebruikt dus zelf eerder berekende prefixinformatie.

## Dia 14 ? Reserve · complexiteit en telafspraken (reserve)

De bounds gelden voor niet-lege patronen; een te lang patroon wordt direct afgehandeld. Het aantal vaste stukken is hoogstens min(w plus één, m min w), met w het aantal wildcards. Bij uitsluitend wildcards is de preciezere tijd O(m plus k), begrensd door O(m plus n). De teller omvat geen wildcardtests, luscontroles of setbewerkingen. Die vallen wel in de gemeten tijd. Daarom is de teller geen exacte maat voor het totaal aantal instructies.

## Dia 15 ? Reserve · koppeling met ontwerptechnieken (reserve)

Het stoppen van de binnenste brute-forcelus bij mismatch is vroegtijdig stoppen, maar niet de recursieve backtrackingtechniek uit §7.2. De KMP-patroonindex kan terugvallen; dat is ook niet hetzelfde als een recursieve zoekboom doorlopen. De wildcardvariant splitst wel het patroon op, maar is geen klassieke recursieve verdeel-en-heersoplossing. KMP gebruikt eerder berekende informatie; leg de precieze prefixrecurrence uit voordat je het als een voorbeeld van dynamisch programmeren zou willen classificeren.

## Dia 16 ? Reserve · bronnen en reproduceerbaarheid (reserve)

De meetcijfers op de dia’s komen uit de README van het project. Ze zijn gemeten op 14 en 15 oktober 2026 met Python 3.14.4 op Linux x86_64, als mediaan van vijf uitvoeringen. De matches en vergelijkingstellingen zijn opnieuw uitgevoerd en gecontroleerd op 5 november; nieuwe tijden kunnen verschillen. De originele paperverwijzingen voor KMP en de wildcardproblematiek staan in de README. Een aparte volledige projectrubriek is hier niet toegevoegd: controleer die vóór het indienen als die op het leerplatform staat.
