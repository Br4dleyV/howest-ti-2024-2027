import { Prompt } from '../Prompts/Prompt.ts';
import { PromptType } from '../Prompts/PromptType.ts';
import { AgentType } from './AgentType.ts';
import { Tool } from '../Tools/Tool.ts';
import { Agent } from './Agent.ts';
import { UnAuthorizedError } from '../../Shared/Exceptions/UnAuthorizedError.ts';

export abstract class GeneralAgent implements Agent {
    constructor(
        private readonly _name: string,
        private readonly _displayName: string,
        private readonly _type: AgentType,
        private readonly _systemPrompt: string,
        private readonly _allowedPromptTypes: PromptType[] = [],
        private readonly _tools: Tool[] = [],
    ) {
    }

    public ask(_prompt: Prompt): Promise<void> {
        if (!this.isPromptAllowed(_prompt.type)) {
            throw new UnAuthorizedError(
                `Prompt type ${_prompt.type} is not allowed for agent ${this.type}`,
            );
        }

        return this._ask(_prompt);
    }

    protected abstract _ask(prompt: Prompt): Promise<void>;

    protected get systemPrompt(): Readonly<string> {
        return this._systemPrompt;
    }

    protected get tools(): Readonly<Tool[]> {
        return this._tools;
    }

    public get name(): Readonly<string> {
        return this._name;
    }

    public get displayName(): Readonly<string> {
        return this._displayName;
    }

    public get type(): Readonly<AgentType> {
        return this._type;
    }

    public isPromptAllowed(type: PromptType): boolean {
        return this._allowedPromptTypes.includes(type);
    }
}
