import { type Repository } from '@/Domain/Shared/mod.ts';
import { Team } from './Team.ts';
import { TeamId } from './TeamId.ts';

export interface TeamRepository extends Repository<Team, TeamId> {}
