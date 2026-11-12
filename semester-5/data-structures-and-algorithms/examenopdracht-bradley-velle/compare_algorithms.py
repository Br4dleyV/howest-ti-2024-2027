"""Vergelijk brute-force en KMP op dezelfde input, met vijf tijdsmetingen."""

from pathlib import Path  # Maak bestandspaden die ook vanuit een andere werkmap kloppen.
from statistics import median  # Bereken de middelste tijd na het sorteren van de tijden.
from time import perf_counter  # Meet de verstreken tijd in seconden.

from brute_force import brute_force_search  # Gebruik de zoekfunctie uit deel 1.
from kmp import kmp_search  # Gebruik de zoekfunctie uit deel 2.


def benchmark(repeats=5, voorbeelden=None):  # Een eigen lijst voorbeelden is optioneel.
    if repeats < 1:  # Zonder uitvoeringen kunnen we geen mediane tijd berekenen.
        raise ValueError("Meet minstens één keer.")  # Stop als het aantal herhalingen ongeldig is.

    if voorbeelden is None:  # Gebruik zonder eigen input de voorbeelden van deel 2.
        voorbeelden = [  # Elke tuple bevat een bestandsnaam en het zoekpatroon.
            ("voorbeeld.txt", "kat"),  # Korte tekst met twee matches.
            ("snelle_mismatch.txt", "bbbbbb"),  # Het eerste karakter verschilt meteen.
            ("veel_vergelijkingen.txt", "aaaaab"),  # De eerste vijf karakters matchen, het zesde verschilt.
        ]
    results = []  # Bewaar de resultaten van alle voorbeelden en algoritmen.

    for filename, pattern in voorbeelden:  # Pak telkens de bestandsnaam en het patroon uit de tuple.
        # Lees eenmaal; bestandsinvoer valt buiten de tijdsmeting.
        path = Path(__file__).parent / "input_data" / filename  # Zoek naast dit script in input_data.
        text = path.read_text(encoding="utf-8")  # Lees de hele tekst in een string.
        previous_matches = None  # Voor dit bestand is nog geen algoritme uitgevoerd.

        # search bevat de functie zelf: we kunnen daarmee eerst brute-force en daarna KMP aanroepen.
        for name, search in [("Brute-force", brute_force_search), ("KMP", kmp_search)]:
            durations = []  # Begin voor ieder algoritme met een lege lijst tijden.

            for run in range(repeats):  # Herhaal dezelfde zoekopdracht het gekozen aantal keren.
                started = perf_counter()  # Bewaar de tijd vlak voor het zoeken.
                result = search(text, pattern)  # Voer het algoritme uit; KMP maakt hier ook zijn tabel.
                durations.append(perf_counter() - started)  # Bewaar de verstreken zoektijd.

            matches = result[0]  # Het eerste resultaat bevat bij beide algoritmen de matchposities.
            # Bij KMP zijn de brute-force matches al bekend; controleer dat ze gelijk zijn.
            if previous_matches is not None and matches != previous_matches:
                raise AssertionError(f"Algoritmen geven andere matches voor {filename}")  # Stop bij een verschil.
            previous_matches = matches  # Bewaar de matches voor de volgende vergelijking.

            results.append({  # Bewaar deze meting als een dictionary met benoemde gegevens.
                "file": filename,  # Naam van het inputbestand.
                "n": len(text),  # Aantal karakters in de tekst.
                "pattern": pattern,  # Het gebruikte zoekpatroon.
                "algorithm": name,  # Naam van het gemeten algoritme.
                "matches": len(matches),  # Aantal gevonden matches.
                "indices": matches[:10],  # Alleen de eerste tien posities voor compacte uitvoer.
                "search_comparisons": result[1],  # Vergelijkingen tussen tekst en patroon.
                "preprocessing_comparisons": result[2] if name == "KMP" else 0,  # Alleen KMP heeft een tabel.
                "median_ms": median(durations) * 1000,  # Zet de mediane tijd van seconden naar milliseconden om.
            })

    return results  # Geef alle metingen terug zodat main ze kan printen.


def main():  # Voer de vergelijking uit en toon de resultaten.
    herhalingen = 5  # Gebruik hetzelfde aantal in de meting en het uitvoerlabel.
    for row in benchmark(herhalingen):  # Toon één resultaat per bestand en algoritme.
        search = row["search_comparisons"]  # Haal het aantal zoekvergelijkingen op.
        preprocessing = row["preprocessing_comparisons"]  # Haal het aantal tabelvergelijkingen op.
        # Toon bestandsnaam, algoritme, inputgegevens en gevonden posities.
        print(f"{row['file']} — {row['algorithm']}")
        print(f"Tekstlengte: {row['n']}, patroon: {row['pattern']!r}")
        print(f"Matches: {row['matches']}, eerste indices: {row['indices']}")
        print(f"Zoekvergelijkingen: {search}")  # Toon alleen het werk tijdens het zoeken.
        print(f"Tabelvergelijkingen: {preprocessing}, totaal: {search + preprocessing}")  # Tel zoeken en voorbereiding op.
        print(f"Mediane tijd ({herhalingen} runs, inclusief voorbereiding): {row['median_ms']:.3f} ms")  # Rond af op drie decimalen.
        print()  # Plaats een lege regel tussen de resultaten.


if __name__ == "__main__":  # Start alleen bij rechtstreeks uitvoeren, niet bij importeren.
    main()  # Start de vergelijking.
