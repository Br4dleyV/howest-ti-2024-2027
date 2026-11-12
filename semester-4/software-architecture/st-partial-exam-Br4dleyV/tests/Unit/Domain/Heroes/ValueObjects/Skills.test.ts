import { assertEquals, assertThrows } from '@std/assert';
import {
    IntelligenceSkill,
    InvalidIntelligenceSkillException,
    InvalidSkillsException,
    InvalidSpeedSkillException,
    InvalidStrengthSkillException,
    Skills,
    SpeedSkill,
    StrengthSkill,
} from '@/Domain/Heroes/ValueObjects/mod.ts';

const createSkills = (
    strength: number,
    speed: number,
    intelligence: number,
): Skills =>
    Skills.create(
        StrengthSkill.create(strength),
        SpeedSkill.create(speed),
        IntelligenceSkill.create(intelligence),
    );

Deno.test('StrengthSkill - create - boundary values - accepts 0 and 100', () => {
    // Arrange
    const min = 0;
    const max = 100;

    // Act
    const minStrength = StrengthSkill.create(min);
    const maxStrength = StrengthSkill.create(max);

    // Assert
    assertEquals(minStrength.value, 0);
    assertEquals(maxStrength.value, 100);
});

Deno.test('StrengthSkill - create - value above max - throws exception', () => {
    // Act & Assert
    assertThrows(
        () => {
            StrengthSkill.create(101);
        },
        InvalidStrengthSkillException,
    );
});

Deno.test('SpeedSkill - create - decimal value - throws exception', () => {
    // Act & Assert
    assertThrows(
        () => {
            SpeedSkill.create(10.5);
        },
        InvalidSpeedSkillException,
    );
});

Deno.test('IntelligenceSkill - create - negative value - throws exception', () => {
    // Act & Assert
    assertThrows(
        () => {
            IntelligenceSkill.create(-1);
        },
        InvalidIntelligenceSkillException,
    );
});

Deno.test('Skills - create - valid values - returns composite value object', () => {
    // Act
    const skills = createSkills(85, 18, 9);

    // Assert
    assertEquals(skills.strength.value, 85);
    assertEquals(skills.speed.value, 18);
    assertEquals(skills.intelligence.value, 9);
});

Deno.test('Skills - create - missing strength value object - throws exception', () => {
    // Arrange
    const speed = SpeedSkill.create(10);
    const intelligence = IntelligenceSkill.create(5);

    // Act & Assert
    assertThrows(
        () => {
            Skills.create(null as unknown as StrengthSkill, speed, intelligence);
        },
        InvalidSkillsException,
    );
});

Deno.test('Skills - equals - different speed - returns false', () => {
    // Arrange
    const left = createSkills(70, 10, 7);
    const right = createSkills(70, 11, 7);

    // Act
    const areEqual = left.equals(right);

    // Assert
    assertEquals(areEqual, false);
});

Deno.test('Skills - equals - same values - returns true', () => {
    // Arrange
    const left = createSkills(70, 10, 7);
    const right = createSkills(70, 10, 7);

    // Act
    const areEqual = left.equals(right);

    // Assert
    assertEquals(areEqual, true);
});

Deno.test('Skills - power - mixed values - returns computed score', () => {
    // Arrange
    const skills = createSkills(80, 10, 5);

    // Act
    const power = skills.power;

    // Assert
    assertEquals(power, 80);
});

Deno.test('Skills - power - minimum speed and intelligence - returns zero', () => {
    // Arrange
    const skills = createSkills(90, 0, 0);

    // Act
    const power = skills.power;

    // Assert
    assertEquals(power, 0);
});
