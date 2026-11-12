import type { Policy } from '@/Application/Ports/mod.ts';
import type {
    PosterPrinterGateway,
    PosterPrinterInput,
} from '@/Application/Ports/Gateways/mod.ts';
import type { MemberAddedToTeamDomainEvent } from '@/Domain/Heroes/Teams/mod.ts';

export class WhenMemberAddedToTeamThenPrintPoster
    implements Policy<MemberAddedToTeamDomainEvent> {
    constructor(private readonly posterPrinterGateway: PosterPrinterGateway) {}

    async handle(event: MemberAddedToTeamDomainEvent): Promise<void> {
        const input: PosterPrinterInput = {
            teamId: event.teamId,
            heroId: event.heroId,
            memberName: event.teamMemberName,
            teamName: event.teamName,
        };

        await this.posterPrinterGateway.print(input);
    }
}
