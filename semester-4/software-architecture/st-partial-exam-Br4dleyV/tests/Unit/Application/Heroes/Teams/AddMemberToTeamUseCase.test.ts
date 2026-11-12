import { assertEquals } from '@std/assert';
import type { AggregateRoot, EntityId, Repository } from '@/Domain/Shared/mod.ts';
import { Team, TeamId } from '@/Domain/Heroes/Teams/mod.ts';
import { AddMemberToTeamUseCase } from '@/Application/Heroes/Teams/mod.ts';
import { createMockTeamRepository } from '../../Shared/Repositories/MockTeamRepository.ts';
import { createMockUnitOfWork } from '@/tests/Unit/Application/Shared/mod.ts';

Deno.test('AddMemberToTeamUseCase execute - valid input adds member and saves aggregate', async () => {
    const existingTeam = Team.create(
        TeamId.create('99999999-9999-4999-8999-999999999999'),
        'Sky Defenders',
    );

    const teamRepository = createMockTeamRepository([existingTeam]);
    const repositories = new Map<string, Repository<AggregateRoot<EntityId>, EntityId>>([
        ['Team', teamRepository as unknown as Repository<AggregateRoot<EntityId>, EntityId>],
    ]);

    const unitOfWork = createMockUnitOfWork({ repositories });
    const useCase = new AddMemberToTeamUseCase(unitOfWork);

    await useCase.execute({
        teamId: existingTeam.id.toString(),
        heroId: 'aaaaaaaa-1111-4aaa-8aaa-111111111111',
        memberName: 'Nova Prime',
        alignment: 'hero',
        strength: 85,
        speed: 18,
        intelligence: 9,
    });

    assertEquals(unitOfWork.doCalled, true);
    assertEquals(unitOfWork.saveCalled, true);
    assertEquals(teamRepository.byIdCalled, true);
    assertEquals(teamRepository.saveCalled, true);
    assertEquals(existingTeam.memberCount, 1);
    assertEquals(existingTeam.members[0]?.heroId.toString(), 'aaaaaaaa-1111-4aaa-8aaa-111111111111');
});
