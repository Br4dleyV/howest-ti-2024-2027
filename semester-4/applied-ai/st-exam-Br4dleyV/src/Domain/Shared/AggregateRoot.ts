/*
 * Copyright (c) 2024 Matthias Blomme and Dimitri Casier
 *
 * This software is released under the MIT License.
 * https://opensource.org/licenses/MIT
 */

import { Entity } from './Entity.ts';
import type { EntityId } from './EntityId.ts';
import type { DomainEvent } from './DomainEvents/DomainEvent.ts';

export abstract class AggregateRoot extends Entity {
    private _domainEvents: DomainEvent[] = [];

    /**
     * Creates an instance of AggregateRoot.
     *
     * @param id - The unique identifier for the aggregate root.
     */
    constructor(id: EntityId) {
        super(id);
    }

    /**
     * Record a domain event that occurred in this aggregate.
     * Events are stored internally and can be retrieved using pullDomainEvents().
     *
     * @param event - The domain event to record
     */
    protected raise(event: DomainEvent): void {
        this._domainEvents.push(event);
    }

    /**
     * Retrieve all domain events and clear the internal event list.
     * This follows the "pull" pattern where the application layer retrieves events after persistence.
     *
     * @returns Array of domain events that occurred in this aggregate
     */
    pullDomainEvents(): DomainEvent[] {
        const events = [...this._domainEvents];
        this._domainEvents = [];
        console.log('pulled events:', events);

        return events;
    }

    /**
     * Get a read-only view of domain events without clearing them.
     * Useful for testing or inspection without side effects.
     *
     * @returns Read-only array of domain events
     */
    get domainEvents(): ReadonlyArray<DomainEvent> {
        return this._domainEvents;
    }

    /**
     * Check if this aggregate has any pending domain events.
     *
     * @returns true if there are pending events, false otherwise
     */
    hasDomainEvents(): boolean {
        return this._domainEvents.length > 0;
    }
}
