/*
 * Copyright (c) 2024 Matthias Blomme and Dimitri Casier
 *
 * This software is released under the MIT License.
 * https://opensource.org/licenses/MIT
 */

import type { Optional } from 'jsr:@domaincrafters/std@1';
import { AggregateRoot, EntityId } from './mod.ts';

/**
 * Defines a generic repository interface for managing domain entities within Domain Driven Development.
 *
 * The `Repository` interface provides the standard operations for accessing and manipulating
 * domain entities. It abstracts the data storage mechanism, allowing for flexibility and
 * ease of testing.
 *
 * @typeParam E - The type of the AggregateRoot managed by the repository.
 */
export interface Repository<E extends AggregateRoot> {
    /**
     * Retrieves an AggregateRoot by its unique identifier.
     *
     * @param id - The unique identifier of the AggregateRoot to retrieve.
     * @returns A promise that resolves to an Optional containing the AggregateRoot if found, or empty if not found.
     */
    byId(id: EntityId): Promise<Optional<E>>;

    /**
     * Persists the given AggregateRoot to the repository.
     *
     * @param AggregateRoot - The AggregateRoot to save.
     * @returns A promise that resolves when the AggregateRoot has been successfully saved.
     */
    save(AggregateRoot: E): Promise<void>;

    /**
     * Removes the given AggregateRoot from the repository.
     *
     * @param AggregateRoot - The AggregateRoot to remove.
     * @returns A promise that resolves when the AggregateRoot has been successfully removed.
     */
    remove(AggregateRoot: E): Promise<void>;
}
