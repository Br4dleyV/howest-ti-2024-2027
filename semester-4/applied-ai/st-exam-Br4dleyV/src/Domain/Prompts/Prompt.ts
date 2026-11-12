import { PromptType } from './PromptType.ts';

export interface Prompt {
    type: PromptType;
    content: string;
    sessionUserId: string;
    role: string;
}
