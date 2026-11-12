import { IllegalArgumentException } from '@domaincrafters/std';
import { InMemorySessionService } from '@google/adk';
import type { AgentFactory } from 'Hackathon.Assistant/Domain/Agents/AgentFactory.ts';
import { AgentType } from 'Hackathon.Assistant/Domain/Agents/AgentType.ts';
import type { Agent } from 'Hackathon.Assistant/Domain/Agents/Agent.ts';
import { AdkMessageFactory } from './AdkMessageFactory.ts';
import { HackathonRegistrationAssistantAdkAgent } from './Agents/HackathonRegistrationAssistantAdkAgent.ts';
import { McpToolsetFactory } from './Tools/McpToolsetFactory.ts';

type AssistantSettings = {
    agentName: string;
    displayName: string;
    appName: string;
    model: string;
    persona: string;
};

const ASSISTANT_DIRECTORY = new URL(
    '../../../../agents/hackathon-registration-assistant/',
    import.meta.url,
);

export class AdkAgentFactory implements AgentFactory {
    constructor(
        private readonly _sessionService: InMemorySessionService,
        private readonly _messageFactory: AdkMessageFactory,
        private readonly _toolsetFactory: McpToolsetFactory,
    ) {
    }

    createAgent(type: AgentType): Agent {
        switch (type) {
            case AgentType.HACKATHON_REGISTRATION_ASSISTANT:
                return this.createHackathonRegistrationAssistant();

            default:
                throw new IllegalArgumentException(`Unsupported agent type: ${type}`);
        }
    }

    private createHackathonRegistrationAssistant(): Agent {
        const settings = this.loadSettings();
        const personaPrompt = this.readTextFile(
            new URL(`prompts/${settings.persona}`, ASSISTANT_DIRECTORY),
            `You are a precise hackathon registration assistant. Use tools to register only valid forms.`,
        );

        return new HackathonRegistrationAssistantAdkAgent(
            settings.agentName,
            settings.displayName,
            settings.model,
            settings.appName,
            personaPrompt,
            this._sessionService,
            this._messageFactory,
            this._toolsetFactory.createToolset(),
        );
    }

    private loadSettings(): AssistantSettings {
        const settingsRaw = Deno.readTextFileSync(new URL('settings.json', ASSISTANT_DIRECTORY));
        const settings = JSON.parse(settingsRaw) as AssistantSettings;

        if (!settings.agentName || !settings.displayName || !settings.appName || !settings.model) {
            throw new Error(
                'Invalid assistant settings: agentName, displayName, appName and model are required.',
            );
        }

        return settings;
    }

    private readTextFile(fileUrl: URL, fallback: string): string {
        const content = Deno.readTextFileSync(fileUrl).trim();

        if (content.length === 0) {
            return fallback;
        }

        return content;
    }
}
