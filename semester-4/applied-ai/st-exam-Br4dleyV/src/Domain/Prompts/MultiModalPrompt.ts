import { Prompt } from './Prompt.ts';

export interface MultiModalPrompt extends Prompt {
    images: { data: string; mimeType: string }[];
}
