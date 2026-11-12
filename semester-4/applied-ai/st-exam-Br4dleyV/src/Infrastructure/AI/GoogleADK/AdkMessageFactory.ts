import type { Prompt } from 'Hackathon.Assistant/Domain/Prompts/Prompt.ts';
import type { MultiModalPrompt } from 'Hackathon.Assistant/Domain/Prompts/MultiModalPrompt.ts';

type AdkContentPart = {
    text?: string;
    inlineData?: {
        data: string;
        mimeType: string;
    };
};

export type AdkContent = {
    role: string;
    parts: AdkContentPart[];
};

export class AdkMessageFactory {
    createMessage(prompt: Prompt): AdkContent {
        const parts: AdkContentPart[] = [{ text: prompt.content }];

        if (this.isMultiModalPrompt(prompt)) {
            for (const image of prompt.images) {
                parts.push({
                    inlineData: {
                        data: image.data,
                        mimeType: image.mimeType,
                    },
                });
            }
        }

        return {
            role: prompt.role,
            parts,
        };
    }

    private isMultiModalPrompt(prompt: Prompt): prompt is MultiModalPrompt {
        return 'images' in prompt;
    }
}
