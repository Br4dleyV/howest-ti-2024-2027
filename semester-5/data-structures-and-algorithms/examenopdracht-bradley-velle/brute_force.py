from pathlib import Path # For file path handling
from time import perf_counter # For measuring execution time
from statistics import median # For the middle execution time

# Assignment 1: Brute-force string matching

## Search a text for a pattern using brute-force string matching
## Gives back a tuple with a list of indexes where the
## pattern was found and the number of comparisons made
def brute_force_search(text: str, pattern: str) -> tuple[list[int], int]:
    if pattern == "":  # Een leeg patroon is geen geldige zoekopdracht.
        raise ValueError("Het patroon mag niet leeg zijn.")

    matches = [] # List of indexes where matches start
    comparisons = 0 # Count of how many comparisons we make
    start = 0 # To keep track of where we are in the text

    while start + len(pattern) <= len(text):  # While there are enough characters left in the text to match the pattern
        offset = 0  # Start checking from the first character of the pattern

        while offset < len(pattern):  # Keep checking while there are pattern characters left
            # Een ? accepteert één tekstkarakter zonder de waarde te vergelijken.
            if pattern[offset] != "?":
                comparisons += 1  # Tel alleen een vergelijking met een vast karakter.
                if text[start + offset] != pattern[offset]:
                    break  # Bij een mismatch proberen we de volgende startpositie.

            offset += 1  # The characters matched, so move to the next pattern character

        if offset == len(pattern):  # If we reached the end, every pattern character matched
            matches.append(start)  # Save the starting index of this match

        start += 1  # Move one position forward in the text and try again

    return matches, comparisons  # Return the match indexes and total comparison count

#################################
###### UITVOEREN PROGRAMMA ######
#################################


def main():
    # Elke test bevat een bestandsnaam en het bijbehorende zoekpatroon.
    voorbeelden = [
        ("voorbeeld.txt", "kat"),
        ("snelle_mismatch.txt", "bbbbbb"),
        ("veel_vergelijkingen.txt", "aaaaab"),
    ]
    herhalingen = 5  # Meet iedere zoekopdracht vijf keer.

    for bestandsnaam, pattern in voorbeelden:
        # Lees het bestand voordat de tijdsmeting begint.
        bestand = Path(__file__).parent / "input_data" / bestandsnaam
        text = bestand.read_text(encoding="utf-8")
        tijden = []  # Bewaar de zoektijd van elke uitvoering.

        for run in range(herhalingen):
            starttijd = perf_counter()  # Start de tijdsmeting.
            matches, comparisons = brute_force_search(text, pattern)
            uitvoeringstijd = perf_counter() - starttijd  # Tijd in seconden.
            tijden.append(uitvoeringstijd)

        # De mediaan is de middelste tijd na het sorteren van de vijf tijden.
        mediane_tijd = median(tijden)

        print(f"Bestand: {bestandsnaam}")
        print(f"Tekstlengte: {len(text)}, patroon: {pattern!r}")
        print(f"Aantal matches: {len(matches)}")
        print(f"Startposities (eerste 10): {matches[:10]}")
        print(f"Aantal karaktervergelijkingen: {comparisons}")
        print(f"Mediane uitvoeringstijd ({herhalingen} runs): {mediane_tijd * 1000:.3f} ms")
        print()


# Voorkom dat importeren onbedoeld de benchmarks start.
if __name__ == "__main__":
    main()
