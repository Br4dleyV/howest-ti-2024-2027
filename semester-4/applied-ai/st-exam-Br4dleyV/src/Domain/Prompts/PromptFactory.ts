import { Prompt } from './Prompt.ts';
import { PromptType } from './PromptType.ts';

export interface PromptFactory {
    createPrompt(type: PromptType, data: unknown): Prompt;
}
