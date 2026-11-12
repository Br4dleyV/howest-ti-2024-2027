import { assertEquals, assertThrows } from '@std/assert';
import {
    HeroAlignement,
    InvalidHeroAlignementException,
} from '@/Domain/Heroes/ValueObjects/mod.ts';

Deno.test('HeroAlignement - create - uppercase value - normalizes to lowercase', () => {
    // Arrange
    const rawAlignment = ' HERO ';

    // Act
    const alignment = HeroAlignement.create(rawAlignment);

    // Assert
    assertEquals(alignment.value, 'hero');
});

Deno.test('HeroAlignement - create - invalid value - throws exception', () => {
    // Act & Assert
    assertThrows(
        () => {
            HeroAlignement.create('antihero');
        },
        InvalidHeroAlignementException,
    );
});

Deno.test('HeroAlignement - factories - hero villain neutral - return correct values', () => {
    // Act
    const hero = HeroAlignement.hero();
    const villain = HeroAlignement.villain();
    const neutral = HeroAlignement.neutral();

    // Assert
    assertEquals(hero.value, 'hero');
    assertEquals(villain.value, 'villain');
    assertEquals(neutral.value, 'neutral');
});

Deno.test('HeroAlignement - equals - same value - returns true', () => {
    // Arrange
    const left = HeroAlignement.create('villain');
    const right = HeroAlignement.create('VILLAIN');

    // Act
    const areEqual = left.equals(right);

    // Assert
    assertEquals(areEqual, true);
});

Deno.test('HeroAlignement - toString - valid value - returns string representation', () => {
    // Arrange
    const alignment = HeroAlignement.neutral();

    // Act
    const asString = alignment.toString();

    // Assert
    assertEquals(asString, 'neutral');
});
