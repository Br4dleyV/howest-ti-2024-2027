import type { UseCase } from '@/Application/Ports/mod.ts';
import { NotFoundApplicationException } from '@/Application/Shared/mod.ts';

import { FindTeamByIdQuery, TeamReadModel } from '../../Ports/Queries/mod.ts';

export interface GetTeamByIdInput {
    teamId: string;
}

export class GetTeamByIdUseCase implements UseCase<GetTeamByIdInput, TeamReadModel> {
    constructor(private readonly query: FindTeamByIdQuery) {}

    async execute(input: GetTeamByIdInput): Promise<TeamReadModel> {
        const team = await this.query.byId(input.teamId);
        if (!team) {
            throw new NotFoundApplicationException(
                `Team with id '${input.teamId}' was not found.`,
            );
        }

        return team;
    }
}
