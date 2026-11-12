import { Optional, type Optional as OptionalType } from '@domaincrafters/std';
import { Team, TeamId, type TeamRepository } from '@/Domain/Heroes/Teams/mod.ts';

export type MockTeamRepository = TeamRepository & {
    byIdCalled: boolean;
    byIdCallCount: number;
    saveCalled: boolean;
    saveCallCount: number;
    removeCalled: boolean;
    removeCallCount: number;
    receivedById: TeamId[];
    savedTeams: Team[];
    removedTeams: Team[];
};

export function createMockTeamRepository(initialTeams: Team[] = []): MockTeamRepository {
    const teams = [...initialTeams];

    const mock: MockTeamRepository = {
        byIdCalled: false,
        byIdCallCount: 0,
        saveCalled: false,
        saveCallCount: 0,
        removeCalled: false,
        removeCallCount: 0,
        receivedById: [],
        savedTeams: [],
        removedTeams: [],
        byId: async (id: TeamId): Promise<OptionalType<Team>> => {
            mock.byIdCalled = true;
            mock.byIdCallCount++;
            mock.receivedById.push(id);

            const team = teams.find((candidate) => candidate.id.equals(id));
            if (team) {
                return Optional.of(team) as OptionalType<Team>;
            }

            return Optional.empty() as OptionalType<Team>;
        },
        save: async (aggregate: Team): Promise<void> => {
            mock.saveCalled = true;
            mock.saveCallCount++;
            mock.savedTeams.push(aggregate);

            const index = teams.findIndex((team) => team.id.equals(aggregate.id));
            if (index >= 0) {
                teams[index] = aggregate;
                return;
            }

            teams.push(aggregate);
        },
        remove: async (aggregate: Team): Promise<void> => {
            mock.removeCalled = true;
            mock.removeCallCount++;
            mock.removedTeams.push(aggregate);

            const index = teams.findIndex((team) => team.id.equals(aggregate.id));
            if (index >= 0) {
                teams.splice(index, 1);
            }
        },
    };

    return mock;
}
