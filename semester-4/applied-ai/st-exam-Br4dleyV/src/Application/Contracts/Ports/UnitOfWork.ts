import type { AggregateRoot, Repository } from 'Hackathon.Assistant/Domain/Shared/mod.ts';

export interface UnitOfWork {
    do<Output>(action: () => Promise<Output>): Promise<Output>;
    getRepository<T extends Repository<AggregateRoot>>(aggregateName: string): T;
    save(aggregate: AggregateRoot): Promise<void>;
    remove(aggregate: AggregateRoot): Promise<void>;
}
