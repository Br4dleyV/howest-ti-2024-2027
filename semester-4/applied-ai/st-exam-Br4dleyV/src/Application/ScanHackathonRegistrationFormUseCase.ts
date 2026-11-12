import {
    type AgentFactory,
} from 'Hackathon.Assistant/Domain/Agents/AgentFactory.ts';
import {
    AgentType,
} from 'Hackathon.Assistant/Domain/Agents/AgentType.ts';
import {
    type PromptFactory,
} from 'Hackathon.Assistant/Domain/Prompts/PromptFactory.ts';
import {
    PromptType,
} from 'Hackathon.Assistant/Domain/Prompts/PromptType.ts';
import type {
    ScanHackathonRegistrationFormInputData,
    UseCase,
} from './Contracts/mod.ts';

export class ScanHackathonRegistrationFormUseCase
    implements UseCase<ScanHackathonRegistrationFormInputData, void> {
    constructor(
        private readonly _agentFactory: AgentFactory,
        private readonly _promptFactory: PromptFactory,
    ) {
    }

    async execute(input: ScanHackathonRegistrationFormInputData): Promise<void> {
        const prompt = this._promptFactory.createPrompt(
            PromptType.SCAN_HACKATHON_REGISTRATION_FORM,
            {
                sessionUserId: input.sessionUserId,
                image: {
                    data: input.base64Image,
                    mimeType: input.mimeType,
                },
            },
        );

        const agent = this._agentFactory.createAgent(
            AgentType.HACKATHON_REGISTRATION_ASSISTANT,
        );

        await agent.ask(prompt);
    }
}
