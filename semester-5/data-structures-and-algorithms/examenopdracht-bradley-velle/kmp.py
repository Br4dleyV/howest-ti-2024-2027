"""Knuth-Morris-Pratt: zoek alle matches zonder terug te gaan in de tekst."""


def build_lps(pattern: str) -> tuple[list[int], int]:  # Ontvang een patroon; geef een lijst en teller terug.
    """Maak de prefix-tabel en tel de vergelijkingen tussen patroonkarakters.

    lps[i] is de lengte van het langste echte voorstuk dat ook een eindstuk
    is van pattern[:i + 1]. 'Echt' betekent korter dan dat volledige stuk.
    """
    lps = [0] * len(pattern)  # Begin met nul voor iedere patroonpositie.
    comparisons = 0  # Vergelijkingen tijdens het maken van de tabel.
    length = 0  # Lengte van het voorstuk dat momenteel overeenkomt.
    index = 1  # Het eerste karakter heeft geen korter, niet-leeg voorstuk.

    while index < len(pattern):  # Verwerk ieder volgend patroonkarakter.
        comparisons += 1  # Tel de karaktervergelijking die nu volgt.
        if pattern[index] == pattern[length]:  # Vergelijk het huidige karakter met het karakter na het bekende voorstuk.
            length += 1  # Het overeenkomstige voorstuk wordt één langer.
            lps[index] = length  # Bewaar deze lengte in de tabel.
            index += 1  # Ga naar de volgende patroonpositie.
        elif length > 0:  # Er is een mismatch, maar misschien past een korter voorstuk nog.
            # Bijvoorbeeld: bij het stuk 'ababa' kan 'aba' nog bruikbaar zijn.
            # Laat index gelijk: vergelijk hetzelfde karakter opnieuw met het kortere voorstuk.
            length = lps[length - 1]  # Probeer een korter bekend voorstuk.
        else:  # Ook het eerste patroonkarakter komt niet overeen.
            index += 1  # Geen overeenkomst; de tabelwaarde blijft nul.

    return lps, comparisons  # Geef de tabel en het aantal vergelijkingen voor de tabelopbouw terug.


def kmp_search(text: str, pattern: str) -> tuple[list[int], int, int]:  # Ontvang tekst en patroon; geef drie resultaten terug.
    """Geef matchindices, zoekvergelijkingen en tabelvergelijkingen terug.

    Zoekt hoofdlettergevoelig en vindt ook overlappende matches.
    Zonder wildcards: O(n + m) tijd en O(m + k) extra geheugen.
    Met wildcards: zoek de vaste stukken met KMP en combineer hun posities.
    """
    if pattern == "":  # Gebruik dezelfde afspraak als bij brute-force.
        raise ValueError("Het patroon mag niet leeg zijn.")  # Stop met een duidelijke foutmelding.
    if len(pattern) > len(text):  # Het patroon past nergens in de tekst.
        return [], 0, 0  # Geen matches; zoeken en tabelopbouw zijn niet nodig.

    if "?" in pattern:  # Gebruik voor wildcards de uitbreiding hieronder.
        return kmp_wildcard_search(text, pattern)

    lps, preprocessing_comparisons = build_lps(pattern)  # Maak de tabel en bewaar de voorbereidingsteller.
    matches = []  # Startposities van de gevonden matches.
    comparisons = 0  # Vergelijkingen tussen tekst en patroon.
    text_index = 0  # Huidige positie in de tekst.
    pattern_index = 0  # Aantal patroonkarakters dat al overeenkomt.

    while text_index < len(text):  # Zoek verder zolang er tekstkarakters over zijn.
        comparisons += 1  # Tel iedere vergelijking, inclusief mismatches.
        if text[text_index] == pattern[pattern_index]:  # Controleer of de huidige karakters gelijk zijn.
            text_index += 1  # Ga vooruit in de tekst.
            pattern_index += 1  # Ga vooruit in het patroon.

            if pattern_index == len(pattern):  # Het hele patroon matcht.
                matches.append(text_index - len(pattern))  # Trek de patroonlengte af om de startpositie te vinden.
                # Bij 'aaa' in 'aaaaa' blijven na een match twee a's bruikbaar.
                # Daardoor vinden we ook de overlappende matches op indices 1 en 2.
                pattern_index = lps[pattern_index - 1]  # Bewaar overlap.
        elif pattern_index > 0:  # Mismatch nadat al een deel van het patroon overeenkwam.
            # De tekstpositie blijft gelijk; hergebruik een bekend voorstuk.
            pattern_index = lps[pattern_index - 1]  # Probeer een korter voorstuk zonder de tekst opnieuw te doorlopen.
        else:  # Mismatch bij het eerste patroonkarakter; er is niets om te hergebruiken.
            text_index += 1  # Geen bruikbaar voorstuk; ga verder in de tekst.

    return matches, comparisons, preprocessing_comparisons  # Houd zoekwerk en voorbereidingswerk apart.


def kmp_wildcard_search(text: str, pattern: str) -> tuple[list[int], int, int]:
    """Zoek vaste stukken met gewone KMP; combineer hun mogelijke startposities.

    Bij r vaste stukken: O(m + (r + 1) * n) tijd en O(n + m) extra geheugen.
    Deze functie wordt door kmp_search aangeroepen na de invoercontroles.
    """
    blocks = []  # Bewaar van ieder vast stuk de positie in het patroon en de tekst.
    index = 0  # Loop van links naar rechts door het patroon.

    while index < len(pattern):
        if pattern[index] == "?":  # Een wildcard hoort niet bij een vast stuk.
            index += 1  # Ga naar het volgende patroonkarakter.
        else:
            block_start = index  # Onthoud waar dit vaste stuk begint.
            while index < len(pattern) and pattern[index] != "?":
                index += 1  # Zoek het einde van dit stuk zonder wildcards.
            blocks.append((block_start, pattern[block_start:index]))

    if not blocks:  # Een patroon met alleen ? past op elke positie waar het past.
        return list(range(len(text) - len(pattern) + 1)), 0, 0

    candidates = None  # De mogelijke startposities van het volledige patroon.
    comparisons = 0  # Tel het zoekwerk van alle vaste stukken samen.
    preprocessing_comparisons = 0  # Tel ook alle prefix-tabellen mee.

    for offset, block in blocks:
        # Dit stuk bevat geen ?, dus kmp_search gebruikt hier gewone KMP.
        block_matches, search_count, table_count = kmp_search(text, block)
        comparisons += search_count
        preprocessing_comparisons += table_count

        # Voor 'go?d' ligt 'd' op patroonpositie 3. Een d op tekstpositie 8
        # geeft dus startpositie 8 - 3 = 5 voor het volledige patroon.
        starts = set()
        for position in block_matches:
            start = position - offset
            if 0 <= start <= len(text) - len(pattern):  # Het hele patroon moet passen.
                starts.add(start)

        if candidates is None:  # Het eerste vaste stuk levert de eerste kandidaten.
            candidates = starts
        else:
            candidates.intersection_update(starts)  # Bewaar posities waar beide stukken passen.

        if not candidates:  # Zonder kandidaten kunnen latere stukken geen match meer opleveren.
            break

    # Doorloop de posities op volgorde, zodat ook de uitvoer gesorteerd is.
    # Een set sorteren is niet nodig: deze stap kost lineaire tijd.
    matches = [start for start in range(len(text) - len(pattern) + 1)
               if start in candidates]
    return matches, comparisons, preprocessing_comparisons


if __name__ == "__main__":  # Start alleen bij rechtstreeks uitvoeren, niet bij importeren.
    # Het vergelijkingsprogramma meet beide algoritmen op dezelfde bestanden.
    from compare_algorithms import main  # Haal de functie op die de vergelijking uitvoert.

    main()  # Start de metingen en toon de resultaten.
