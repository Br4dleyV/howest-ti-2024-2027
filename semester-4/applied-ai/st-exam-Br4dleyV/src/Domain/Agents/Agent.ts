import { Prompt } from '../Prompts/Prompt.ts';

export interface Agent {
    ask(prompt: Prompt): Promise<void>;
}
