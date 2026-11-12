"""Genereer de presentatie. Vereist: python-pptx. Meetdata komen uit README.md."""
from pathlib import Path
from pptx import Presentation
from pptx.util import Inches, Pt
from pptx.dml.color import RGBColor
from pptx.enum.shapes import MSO_SHAPE

OUT = Path(__file__).resolve().parent
prs = Presentation()
prs.slide_width, prs.slide_height = Inches(13.333), Inches(7.5)
BG, PANEL, WHITE, MUTED, MINT, ORANGE, RED = '101C2B', '1C2C40', 'F5F7FA', 'B4C2D2', '62DFBB', 'FFBE76', 'FF8585'
notes = []

def box(s, x, y, w, h, fill=PANEL, radius=False):
    sh = s.shapes.add_shape(MSO_SHAPE.ROUNDED_RECTANGLE if radius else MSO_SHAPE.RECTANGLE, Inches(x), Inches(y), Inches(w), Inches(h))
    sh.fill.solid(); sh.fill.fore_color.rgb = RGBColor.from_string(fill)
    sh.line.fill.background()
    return sh

def txt(s, text, x, y, w, h=.5, size=24, color=WHITE, bold=False, font='Aptos'):
    sh = s.shapes.add_textbox(Inches(x), Inches(y), Inches(w), Inches(h))
    tf = sh.text_frame; tf.word_wrap = True
    tf.margin_left = tf.margin_right = Inches(.02)
    tf.margin_top = tf.margin_bottom = Inches(.015)
    for i, line in enumerate(text.split('\n')):
        p = tf.paragraphs[0] if i == 0 else tf.add_paragraph()
        p.text = line; p.font.name = font; p.font.size = Pt(size)
        p.font.bold = bold; p.font.color.rgb = RGBColor.from_string(color)
        p.space_after = Pt(9)
    return sh

def slide(title, kicker, seconds=0):
    s = prs.slides.add_slide(prs.slide_layouts[6])
    s.background.fill.solid(); s.background.fill.fore_color.rgb = RGBColor.from_string(BG)
    txt(s, kicker.upper(), .65, .35, 12, .3, 12, MINT, True)
    txt(s, title, .65, .95, 12, .95, 33, WHITE, True)
    box(s, .65, 6.96, 12, .015, PANEL)
    n = len(prs.slides)
    txt(s, 'BRADLEY VELLE  /  STRING MATCHING', .65, 7.08, 9, .22, 10, MUTED)
    txt(s, f'{n:02}', 12, 7.05, .6, .25, 11, MINT)
    notes.append({'title': title, 'seconds': seconds, 'text': ''})
    return s

def note(s, text):
    notes[-1]['text'] = text.strip()
    s.notes_slide.notes_text_frame.text = text.strip()

def cells(s, text, x, y, highlight=(), bad=(), width=.65):
    for i,c in enumerate(text):
        fill = MINT if i in highlight else RED if i in bad else PANEL
        box(s, x+i*width, y, width-.045, .6, fill)
        txt(s, c, x+i*width+.12, y+.07, width-.1, .42, 25, BG if i in highlight or i in bad else WHITE, True, 'Consolas')

def card(s, x, y, w, title, body, accent=MINT):
    box(s,x,y,w,2.35,radius=True)
    txt(s,title,x+.22,y+.22,w-.44,.5,24,accent,True)
    txt(s,body,x+.22,y+.9,w-.44,1.3,21)

s=slide('Een patroon zoeken, met minder herhaling', 'Examenopdracht · 12 november 2026',30)
txt(s,'STRING\nMATCHING',.8,2.25,7,2.2,58,WHITE,True)
txt(s,'Brute-force → KMP → wildcards',.85,5.2,11,.6,28,MINT)
txt(s,'Bradley Velle · Groep 6 · Python',.85,6.15,11,.45,19,MUTED)
note(s,'Ik heb string matching uitgewerkt: zoeken waar een patroon in een tekst voorkomt. Ik begin met de eenvoudige brute-forceaanpak uit de cursus. Daarna toon ik hoe KMP eerder vergelijkingswerk hergebruikt. Tot slot bespreek ik mijn uitbreiding met vraagtekens als wildcards en wat die doet met de performantie.')

s=slide('Wat moet de zoekfunctie opleveren?', '01 · Probleem en afspraken',40)
txt(s,'Tekst',.8,2.15,2,.4,18,MUTED)
cells(s,'kat kat',2.6,2.05,highlight=(0,1,2,4,5,6))
txt(s,'Patroon',.8,3.05,2,.4,18,MUTED); cells(s,'kat',2.6,2.95)
txt(s,'Startposities: [0, 4]',.8,4.1,11,.65,34,MINT,True)
txt(s,'Alle matches, inclusief overlap · hoofdlettergevoelig\n? accepteert precies één karakter · leeg patroon geweigerd',.8,5.25,11.7,1.1,23)
note(s,'Mijn programma krijgt een tekst en iets om in die tekst te zoeken. Dat zoekstuk noem ik het patroon. In "kat kat" vind ik "kat" op positie 0 en 4. Ik tel vanaf 0, zoals Python dat ook doet. Ik zoek alle plaatsen, niet alleen de eerste. Ook overlappende resultaten tellen mee: "aaa" staat in "aaaaa" op positie 0, 1 en 2. Hoofdletters en kleine letters zijn verschillend. Een leeg patroon weiger ik. Als het patroon langer is dan de tekst, kan het nergens passen.')

s=slide('Brute-force begint op elke positie opnieuw', '02 · Brute-kracht · cursus §7.1',55)
txt(s,'Tekst',.8,2.15,1.5,.4,18,MUTED); cells(s,'aaaaaaaa',2.4,2.05)
cells(s,'aaaaab',2.4,3.05,highlight=range(5),bad=(5,))
cells(s,'aaaaab',3.05,4.05,highlight=range(5),bad=(5,))
txt(s,'5 letters kloppen. De b verschilt. Begin één plaats verder opnieuw.',.8,5.1,12,.7,25,ORANGE,True)
txt(s,'Slechtste geval: O(n × m) · n = tekstlengte · m = patroonlengte',.8,6.1,12,.5,23)
note(s,'Brute-force is de eenvoudigste aanpak. Ik probeer het patroon op de eerste plaats in de tekst. Ik vergelijk de letters één voor één. Zodra een letter verschilt, schuif ik één plaats op en begin ik opnieuw. Hier zoek ik "aaaaab" in een tekst met alleen a’s. De eerste vijf letters kloppen. Pas bij de b gaat het fout. Op de volgende plaats controleer ik opnieuw vijf a’s en daarna de b. Er wordt dus veel werk herhaald. In het slechtste geval controleer ik op bijna elke tekstpositie het hele patroon. Daarom is de tijdscomplexiteit O(n × m): n is de tekstlengte en m de patroonlengte. Als de eerste letter al verschilt, ben ik sneller klaar met die poging.')

s=slide('KMP hoeft bekende letters niet opnieuw te controleren', '03 · De slimmere aanpak',75)
txt(s,'Patroon',.8,2.1,1.7,.4,18,MUTED); cells(s,'aaaaab',2.6,2.0)
txt(s,'Hulptabel',.8,2.95,1.7,.4,18,MUTED); cells(s,'012340',2.6,2.85,highlight=(4,))
txt(s,'Deze letters klopten al',8.0,1.85,4.2,.3,14,MUTED)
cells(s,'aaaaa',8.0,2.25,highlight=(1,2,3,4))
cells(s,'aaaa',8.65,3.0,highlight=(0,1,2,3))
txt(s,'Na vijf a’s kan ik vier a’s hergebruiken.',.8,4.0,11.8,.6,30,MINT,True)
txt(s,'De hulptabel zegt hoeveel letters bruikbaar blijven.\nDie tabel heet de prefix-tabel of LPS-tabel.',.8,4.9,11.8,1.15,25)
txt(s,'Ik ga verder in de tekst, zonder terug te gaan.',.8,6.2,11.8,.45,23,ORANGE)
note(s,'KMP lost het herhaalde werk van brute-force op. We zoeken nog steeds "aaaaab". Stel dat vijf a’s kloppen, maar de volgende tekstletter een a is in plaats van de verwachte b. Bij brute-force zou ik één plaats opschuiven en alles opnieuw controleren. KMP weet al dat de laatste vier a’s ook het begin van het patroon kunnen zijn. Die vier letters hoef ik dus niet opnieuw te controleren. Ik vergelijk dezelfde tekstletter nu met de vijfde a van het patroon. Om te weten hoeveel letters ik kan behouden, maak ik vooraf een hulptabel. Die heet de prefix-tabel of LPS-tabel. De 4 onder de vijfde a betekent dat vier letters bruikbaar blijven. Het belangrijkste idee is: onthouden wat al klopt, zodat ik minder opnieuw moet doen.')

s=slide('Mijn KMP-code heeft twee stappen', '04 · Hoe ik het programmeerde',45)
card(s,.8,2.25,3.75,'1. Tabel maken','Onthouden wat ik\nkan hergebruiken')
card(s,4.8,2.25,3.75,'2. Tekst doorzoeken','De tabel gebruiken\nals een letter verschilt')
card(s,8.8,2.25,3.75,'Resultaat bewaren','Alle gevonden plaatsen\nin een Python-lijst')
txt(s,'Brute-force: O(n × m)        KMP: O(n + m)',.8,5.35,12,.6,29,MINT,True)
txt(s,'n = tekstlengte · m = patroonlengte · KMP bewaart ook een hulptabel.',.8,6.15,12,.5,21,MUTED)
note(s,'In mijn code zijn er twee aparte functies. build_lps maakt de hulptabel. kmp_search gebruikt die tabel om de tekst te doorzoeken. De gevonden plaatsen bewaar ik in een Python-lijst. De tabel kost extra geheugen, maar helpt om vergelijkingen te vermijden. De tabel maken kost O(m), de tekst doorzoeken O(n). Samen is dat O(n + m). Bij brute-force vermenigvuldig ik de twee lengtes in de slechtste situatie; bij KMP tel ik ze op. Ook als ik al een volledig resultaat vond, helpt de tabel om verder te zoeken naar overlappende resultaten.')

s=slide('Veel gedeeltelijke matches: KMP herhaalt minder', '05 · Meting van mijn implementatie',70)
txt(s,'100.000 × a · patroon aaaaab · geen matches',.8,2.0,12,.5,25,MUTED)
txt(s,'Brute-force',.8,3.05,2.2,.5,23); box(s,3.2,3.05,6.3,.5,ORANGE); txt(s,'599.970',9.8,3.02,2.6,.5,28,ORANGE,True)
txt(s,'KMP',.8,4.05,2.2,.5,23); box(s,3.2,4.05,6.3*200004/599970,.5,MINT); txt(s,'200.004',9.8,4.02,2.6,.5,28,MINT,True)
txt(s,'Karaktervergelijkingen, inclusief prefix-tabel',3.2,4.8,9,.4,19,MUTED)
txt(s,'69,060 ms → 24,418 ms  ·  ongeveer 2,83× sneller',.8,5.5,12,.6,29,WHITE,True)
txt(s,'README-meting: 14/10/2026 · Python 3.14.4 · Linux x86_64 · mediaan van 5 runs',.8,6.3,12,.3,14,MUTED)
note(s,'Ik heb beide algoritmen op dezelfde tekst en hetzelfde patroon getest. De tekst bevat honderdduizend a’s. Het patroon is "aaaaab" en wordt nergens gevonden. Brute-force vergelijkt bijna zeshonderdduizend keer twee letters. KMP doet dat ongeveer tweehonderdduizend keer, inclusief het maken van de tabel. Dat is ongeveer drie keer minder. In deze meting duurde brute-force ongeveer 69 milliseconden en KMP ongeveer 24 milliseconden. Ik voerde iedere zoekopdracht vijf keer uit en nam de middelste tijd. Het lezen van het bestand telde niet mee. De tabel maken telde wel mee. KMP is vooral nuttig als er veel gedeeltelijk gelijke stukken zijn. Het is niet op elke tekst automatisch sneller. De exacte meetcijfers komen uit mijn README; op een andere computer kunnen de tijden verschillen.')

s=slide('Met ? kan één letter om het even wat zijn', '06 · Wildcards',40)
txt(s,'go?d  →  goed · goud · goad',.8,2.25,12,.7,36,MINT,True)
txt(s,'? vervangt precies één teken.',.8,3.5,12,.6,28)
card(s,.8,4.5,5.7,'god past niet','Er ontbreekt één teken.',ORANGE)
card(s,6.85,4.5,5.7,'goood past niet','Er staat één teken te veel.',ORANGE)
note(s,'Daarna heb ik beide algoritmen uitgebreid met wildcards. Een wildcard is hier het teken ?. Het staat voor precies één willekeurig teken. Met "go?d" vind ik dus "goed", "goud" en "goad". "god" past niet, want dat is te kort. "goood" past ook niet, want dat is te lang. Bij brute-force kan ik de ? gewoon accepteren. Bij KMP is het lastiger: de hulptabel rekent op gelijke letters, terwijl ? verschillende letters kan voorstellen. Daarom zoek ik bij mijn KMP-uitbreiding eerst de stukken zonder ?. Dat toon ik op de volgende dia.')

s=slide('Mijn oplossing: vaste stukken zoeken en combineren', '07 · KMP met wildcards',65)
cells(s,'go?d',.8,2.1,highlight=(0,1,3))
txt(s,'Zoek eerst go en d met KMP.',4.4,2.15,7.8,.5,27,MINT,True)
card(s,.8,3.35,5.7,'1. Vind de vaste stukken','Bijvoorbeeld: go op plaats 5\nen d op plaats 8.')
card(s,6.85,3.35,5.7,'2. Kijk of ze goed staan','go op 5 en 6 · ? op 7 · d op 8\nEr zit precies één teken tussen.')
txt(s,'Stukken op de juiste plaatsen + patroon past → resultaat op 5',.8,6.0,12,.6,25,ORANGE,True)
note(s,'Ik knip "go?d" in de vaste stukken "go" en "d". Ik zoek die stukken apart met KMP. Daarna kijk ik of ze op de juiste afstand van elkaar staan. Stel dat "go" begint op plaats 5. De g staat dan op 5 en de o op 6. Op plaats 7 mag om het even welk teken staan. De d moet dan op plaats 8 staan. Als dat klopt, past "go?d" vanaf plaats 5. In de code onthoud ik waar ieder vast stuk in het patroon staat. Dat heet de offset. De d staat bijvoorbeeld op patroonpositie 3. Een d op tekstpositie 8 hoort daarom bij een mogelijk begin op 8 − 3 = 5. Ik bewaar alleen beginplaatsen waarvoor alle stukken kloppen en het volledige patroon past.')

s=slide('Meer wildcards betekent niet automatisch sneller', '08 · Gevolgen voor performantie',60)
txt(s,'Elk vast stuk kan een nieuwe zoekronde door de tekst vragen.',.8,2.0,12,.5,24,MUTED)
txt(s,'Meer stukken → meer zoekwerk',.8,2.85,12,.8,38,MINT,True)
card(s,.8,4.0,5.7,'a?a?a? → 3 zoekrondes','Brute-force: 70,784 ms\nKMP-stukken: 129,479 ms',ORANGE)
card(s,6.85,4.0,5.7,'?????? → direct alle starts','Brute-force: 49,314 ms\nKMP-stukken: 1,890 ms')
txt(s,'README · 15/10/2026 · 100.000 × a · patronen van lengte 6 · mediaan van 5 runs',.8,6.55,12,.26,13,MUTED)
note(s,'Met wildcards is mijn uitbreiding niet altijd sneller dan brute-force. Bij "a?a?a?" zijn er drie vaste stukken: drie losse a’s. Mijn code kan daardoor drie keer door de tekst zoeken. Daarna moet ze de gevonden plaatsen ook nog combineren. In mijn meting duurt dat ongeveer 129 milliseconden, tegenover ongeveer 71 voor brute-force. Bij "??????" zijn er juist geen vaste stukken. Elk stuk tekst van zes tekens past dan. Mijn KMP-uitbreiding kan meteen alle passende beginplaatsen teruggeven. Dat duurt hier ongeveer 2 milliseconden, tegenover 49 voor brute-force. Het hangt dus niet alleen af van hoeveel ? er staan, maar ook van waar ze staan. De uitgebreidere formule staat op een reservedia voor vragen.')

s=slide('Hoe heb ik mijn programma gecontroleerd?', '09 · Keuzes en controles',50)
card(s,.8,2.3,3.75,'Python','Leesbare lussen\nGeen externe packages')
card(s,4.8,2.3,3.75,'Moeilijk punt','KMP aanpassen\nvoor patronen met ?',ORANGE)
card(s,8.8,2.3,3.75,'Controle','Vergelijken met een\naparte zoekmethode')
txt(s,'20.240 testgevallen: beide algoritmen gaven de juiste plaatsen.',.8,5.3,12,.7,27,MINT,True)
txt(s,'De implementatiekeuzes en tests staan uitgewerkt in de README.',.8,6.2,12,.4,21,MUTED)
note(s,'Ik koos Python omdat ik de code daarin duidelijk kan schrijven en uitleggen. Het moeilijkste onderdeel was KMP aanpassen voor patronen met ?. Dat heb ik opgelost door vaste stukken apart te zoeken. Om te controleren of alles klopt, heb ik de resultaten vergeleken met een aparte zoekmethode, op basis van reguliere expressies. Zo controleer ik niet alleen of mijn twee algoritmen het met elkaar eens zijn, maar ook of hun resultaten juist zijn. Er zijn 15.240 kleine testgevallen en 5.000 willekeurige testgevallen gecontroleerd. Ze slaagden allemaal. Ik controleerde ook overlap, een lege tekst, een te lang patroon en bijzondere tekens.')

s=slide('Wat neem ik mee uit deze opdracht?', '10 · Besluit',40)
txt(s,'Brute-force',.8,2.3,3,.5,28,ORANGE,True); txt(s,'Elke plaats proberen, vaak opnieuw beginnen',4.0,2.3,8.5,.5,25)
txt(s,'KMP',.8,3.3,3,.5,28,MINT,True); txt(s,'Onthouden wat al klopt, minder herhalen',4.0,3.3,8.5,.5,25)
txt(s,'Wildcards',.8,4.3,3,.5,28,MINT,True); txt(s,'Vaste stukken zoeken en samenbrengen',4.0,4.3,8.5,.5,25)
box(s,.8,5.65,11.75,.9,PANEL,True)
txt(s,'Individueel project: analyse, code, tests en verslag zelf uitgewerkt.',1.05,5.87,11.2,.45,22,ORANGE)
note(s,'Uit deze opdracht onthoud ik vooral het verschil in aanpak. Brute-force begint op elke plaats opnieuw. KMP onthoudt welke letters al kloppen en vermijdt daardoor veel herhaling. Voor patronen met ? zoek ik de vaste stukken apart en breng ik de resultaten samen. Dat werkt, maar kan extra zoekwerk vragen. Ik heb de opdracht alleen uitgevoerd omdat de klas een oneven aantal studenten telde. Ik werkte aan de code, de metingen en het verslag, met ondersteuning van hulpmiddelen. De gebruikte hulp kan ik bij vragen toelichten.')

s=slide('Reserve · een prefix-tabel stap voor stap', 'Vragen · buiten de 10 minuten')
cells(s,'ababaca',.8,2.15)
cells(s,'0012301',.8,3.0,highlight=(2,3,4))
txt(s,'Bij c: lengte 3 → 1 → 0, maar index blijft staan.\nBij de laatste a: opnieuw een prefix van lengte 1.',.8,4.3,12,1.3,29)
txt(s,'Na een match van aaa: val terug naar lengte 2 om overlap te vinden.',.8,6.0,12,.6,23,MINT)
note(s,'Deze dia gebruik je alleen bij vragen. Voor ababaca zijn de LPS-waarden 0,0,1,2,3,0,1. Bij index vijf staat c. Vergelijk met patroonindex drie: b, mismatch. Val terug naar LPS van index twee: één. Vergelijk met patroonindex één: b, weer mismatch. Val terug naar nul. Vergelijk met a, mismatch: de waarde blijft nul en de index gaat vooruit. Na de laatste a krijg je één. De opbouw gebruikt dus zelf eerder berekende prefixinformatie.')

s=slide('Reserve · complexiteit en telafspraken', 'Vragen · buiten de 10 minuten')
rows=[('Methode','Worst-case tijd','Extra geheugen'),('Brute-force','O(n × m)','O(k)'),('KMP zonder ?','O(n + m)','O(m + k)'),('KMP met r stukken','O(m + (r + 1) × n)','O(n + m)'),('Alleen ? bij KMP','O(n + m)','O(k)')]
for i,row in enumerate(rows):
    y=2.15+i*.66
    if i%2==0: box(s,.8,y,11.75,.61,PANEL)
    for x,w,value in zip((1,5.1,9.3),(4,4,3),row): txt(s,value,x,y+.08,w,.45,20,MINT if i==0 else WHITE,i==0)
txt(s,'k = aantal matches. Sets: gemiddelde constante tijd per bewerking.\nTeller: vaste tekst-patroonvergelijkingen + patroon-patroonvergelijkingen.',.8,5.8,12,.85,20,MUTED)
note(s,'De bounds gelden voor niet-lege patronen; een te lang patroon wordt direct afgehandeld. Het aantal vaste stukken is hoogstens min(w plus één, m min w), met w het aantal wildcards. Bij uitsluitend wildcards is de preciezere tijd O(m plus k), begrensd door O(m plus n). De teller omvat geen wildcardtests, luscontroles of setbewerkingen. Die vallen wel in de gemeten tijd. Daarom is de teller geen exacte maat voor het totaal aantal instructies.')

s=slide('Reserve · koppeling met ontwerptechnieken', 'Vragen · buiten de 10 minuten')
txt(s,'Brute-kracht',.8,2.2,3.6,.5,26,MINT,True);txt(s,'Elke passende startpositie proberen.',4.5,2.2,8,.6,25)
txt(s,'Backtracking',.8,3.1,3.6,.5,26,MINT,True);txt(s,'Een recursieve tak stoppen zodra die ongeldig is.',4.5,3.1,8,.85,24)
txt(s,'Branch-and-bound',.8,4.15,3.6,.5,25,MINT,True);txt(s,'Stoppen als een tak de beste oplossing niet kan verbeteren.',4.5,4.15,8,.85,24)
txt(s,'Let op de classificatie',.8,5.3,4,.5,25,ORANGE,True);txt(s,'Een tabel maakt KMP niet automatisch dynamisch programmeren.',4.5,5.3,8,.9,24)
note(s,'Het stoppen van de binnenste brute-forcelus bij mismatch is vroegtijdig stoppen, maar niet de recursieve backtrackingtechniek uit §7.2. De KMP-patroonindex kan terugvallen; dat is ook niet hetzelfde als een recursieve zoekboom doorlopen. De wildcardvariant splitst wel het patroon op, maar is geen klassieke recursieve verdeel-en-heersoplossing. KMP gebruikt eerder berekende informatie; leg de precieze prefixrecurrence uit voordat je het als een voorbeeld van dynamisch programmeren zou willen classificeren.')

s=slide('Reserve · bronnen en reproduceerbaarheid', 'Vragen · buiten de 10 minuten')
txt(s,'Cursus',.8,2.2,2.5,.5,25,MINT,True)
txt(s,'7-ontwerptechnieken.pdf · §7.1, cursuspagina’s 159–161\n7-ontwerptechnieken.pptx · dia’s 3–5\n07-ontwerptechnieken/ · oefeningen bij de andere technieken',3.4,2.2,9.1,1.6,21)
txt(s,'Eigen project',.8,4.15,2.5,.5,25,MINT,True)
txt(s,'README.md · keuzes, tellingen, meetmethode en bronnen\nbrute_force.py · kmp.py\ncompare_algorithms.py · compare_wildcards.py',3.4,4.15,9.1,1.5,21)
txt(s,'Tijden: historische README-metingen; resultaten en tellers opnieuw gecontroleerd op 05/11/2026.',.8,6.25,12,.5,17,MUTED)
note(s,'De meetcijfers op de dia’s komen uit de README van het project. Ze zijn gemeten op 14 en 15 oktober 2026 met Python 3.14.4 op Linux x86_64, als mediaan van vijf uitvoeringen. De matches en vergelijkingstellingen zijn opnieuw uitgevoerd en gecontroleerd op 5 november; nieuwe tijden kunnen verschillen. De originele paperverwijzingen voor KMP en de wildcardproblematiek staan in de README. Een aparte volledige projectrubriek is hier niet toegevoegd: controleer die vóór het indienen als die op het leerplatform staat.')

prs.core_properties.title = 'String matching — Bradley Velle'
prs.core_properties.subject = 'Examen Data Structures & Algorithms — brute-force, KMP en wildcards'
prs.core_properties.author = 'Bradley Velle'
prs.core_properties.keywords = 'string matching, KMP, brute-force, wildcards, ontwerptechnieken'
for reserve_slide in list(prs.slides)[11:]:
    reserve_slide._element.set('show', '0')
prs.save(OUT/'String-matching-Bradley-Velle.pptx')
elapsed=0
lines=['# Sprekersnotities — String matching','', 'De eerste 11 dia’s vormen de presentatie. Richttijd: **9 min 30 s**. Dia’s 12–15 zijn reserve voor vragen. Dezelfde notities staan in PowerPoint bij de dia’s.','', 'De opdracht is individueel uitgevoerd wegens een oneven aantal studenten. De meettijden zijn historische resultaten uit de README; presenteer ze als zodanig.','']
for i,n in enumerate(notes,1):
    start=elapsed;elapsed+=n['seconds']
    timing=f"{start//60}:{start%60:02}–{elapsed//60}:{elapsed%60:02}" if n['seconds'] else 'reserve'
    lines.extend([f"## Dia {i} — {n['title']} ({timing})",'',n['text'],''])
(OUT/'Sprekersnotities.md').write_text('\n'.join(lines),encoding='utf-8')
print(f'Gemaakt: 15 dia’s, 11 hoofddia’s, {elapsed} seconden richttijd.')
