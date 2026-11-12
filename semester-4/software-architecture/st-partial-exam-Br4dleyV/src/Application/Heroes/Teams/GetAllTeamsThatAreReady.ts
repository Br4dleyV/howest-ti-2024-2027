import type { UseCase } from '@/Application/Ports/mod.ts';
import type {
    FindAllReadyTeamsQuery,
    TeamReadModel,
} from '@/Application/Ports/Queries/mod.ts';

export class GetAllReadyTeamsUseCase implements UseCase<void, TeamReadModel[]> {
    constructor(private readonly query: FindAllReadyTeamsQuery) {}

    async execute(): Promise<TeamReadModel[]> {
        return await this.query.findAll();
    }
}
