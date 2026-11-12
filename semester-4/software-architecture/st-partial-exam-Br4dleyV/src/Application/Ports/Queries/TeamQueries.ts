export interface TeamMemberReadModel {
    heroId: string;
    name: string;
    alignment: 'hero' | 'villain' | 'neutral';
    power: number;
}

export interface TeamReadModel {
    id: string;
    name: string;
    members: TeamMemberReadModel[];
    memberCount: number;
    power: number;
    isReady: boolean;
}

export interface FindTeamByIdQuery {
    byId(teamId: string): Promise<TeamReadModel | null>;
}

export interface FindAllReadyTeamsQuery {
    findAll(): Promise<TeamReadModel[]>;
}
