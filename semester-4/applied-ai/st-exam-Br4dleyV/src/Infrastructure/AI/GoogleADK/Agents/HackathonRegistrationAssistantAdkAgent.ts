import {
    type BaseTool,
    InMemorySessionService,
    isFinalResponse,
    LlmAgent,
    Runner,
    stringifyContent,
} from '@google/adk';
import type { Agent } from 'Hackathon.Assistant/Domain/Agents/Agent.ts';
import type { Prompt } from 'Hackathon.Assistant/Domain/Prompts/Prompt.ts';
import { PromptType } from 'Hackathon.Assistant/Domain/Prompts/PromptType.ts';
import { UnAuthorizedError } from 'Hackathon.Assistant/Shared/Exceptions/UnAuthorizedError.ts';
import { AdkMessageFactory } from '../AdkMessageFactory.ts';

export class HackathonRegistrationAssistantAdkAgent implements Agent {
    private readonly _llmAgent: LlmAgent;

    constructor(
        private readonly _name: string,
        private readonly _displayName: string,
        private readonly _model: string,
        private readonly _appName: string,
        private readonly _systemPrompt: string,
        private readonly _sessionService: InMemorySessionService,
        private readonly _messageFactory: AdkMessageFactory,
        tools: BaseTool[],
    ) {
        this._llmAgent = new LlmAgent({
            name: this._name,
            description: this._displayName,
            model: this._model,
            instruction: this._systemPrompt,
            tools,
        });
    }

    async ask(prompt: Prompt): Promise<void> {
        if (prompt.type !== PromptType.SCAN_HACKATHON_REGISTRATION_FORM) {
            throw new UnAuthorizedError(
                `Prompt type ${prompt.type} is not allowed for agent ${this._name}`,
            );
        }

        const sessionId = crypto.randomUUID();
        await this._sessionService.createSession({
            appName: this._appName,
            userId: prompt.sessionUserId,
            sessionId,
        });

        const runner = new Runner({
            agent: this._llmAgent,
            appName: this._appName,
            sessionService: this._sessionService,
        });

        const message = this._messageFactory.createMessage(prompt);
        const events = runner.runAsync({
            userId: prompt.sessionUserId,
            sessionId,
            newMessage: message,
        });

        let finalResponse = '';

        for await (const event of events) {
            if (isFinalResponse(event)) {
                const text = stringifyContent(event);
                if (text) {
                    finalResponse = text;
                }
            }
        }

        if (finalResponse.trim().length === 0) {
            throw new Error('The ADK agent finished without a final response.');
        }

        console.log(`[ADK:${this._name}] ${finalResponse}`);
    }
}
