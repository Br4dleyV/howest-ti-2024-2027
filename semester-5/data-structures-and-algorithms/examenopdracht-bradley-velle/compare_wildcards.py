"""Deel 3: meet het effect van het aantal en de posities van ?-wildcards."""

from compare_algorithms import benchmark  # Gebruik dezelfde meetmethode als bij deel 2.


# De lange voorbeelden gebruiken steeds dezelfde tekstlengte en patroonlengte.
# De twee patronen met vijf wildcards laten ook het effect van hun positie zien.
VOORBEELDEN = [
    ("wildcards.txt", "go?d"),  # Zowel goed als goud moet gevonden worden.
    ("veel_vergelijkingen.txt", "aaaaab"),  # Geen wildcards: gewone KMP.
    ("veel_vergelijkingen.txt", "aaaa?b"),  # Eén wildcard, twee vaste stukken.
    ("veel_vergelijkingen.txt", "aaa??b"),  # Twee wildcards, twee vaste stukken.
    ("veel_vergelijkingen.txt", "a?a?a?"),  # Drie wildcards, drie vaste stukken.
    ("veel_vergelijkingen.txt", "a????b"),  # Vier wildcards, twee vaste stukken.
    ("veel_vergelijkingen.txt", "a?????"),  # Vijf wildcards, veel matches.
    ("veel_vergelijkingen.txt", "?????b"),  # Ook vijf wildcards, maar geen matches.
    ("veel_vergelijkingen.txt", "??????"),  # Alleen wildcards: alle startposities matchen.
]


def main():  # Meet ieder voorbeeld vijf keer voor beide algoritmen.
    herhalingen = 5  # Gebruik hetzelfde aantal in de meting en het uitvoerlabel.
    for row in benchmark(repeats=herhalingen, voorbeelden=VOORBEELDEN):
        pattern = row["pattern"]  # Haal het patroon uit het meetresultaat.
        search = row["search_comparisons"]  # Alleen tekst-patroonvergelijkingen.
        table = row["preprocessing_comparisons"]  # Vergelijkingen voor de KMP-tabellen.
        print(f"{row['file']} — {row['algorithm']}")
        print(f"Tekstlengte: {row['n']}, patroonlengte: {len(pattern)}")
        print(f"Patroon: {pattern!r}, wildcards: {pattern.count('?')}")
        print(f"Matches: {row['matches']}, eerste indices: {row['indices']}")
        print(f"Zoekvergelijkingen: {search}, tabel: {table}, totaal: {search + table}")
        print(f"Mediane tijd ({herhalingen} runs, inclusief voorbereiding): {row['median_ms']:.3f} ms")
        print()  # Scheid de voorbeelden met een lege regel.


if __name__ == "__main__":  # Voer de metingen niet uit bij het importeren.
    main()
