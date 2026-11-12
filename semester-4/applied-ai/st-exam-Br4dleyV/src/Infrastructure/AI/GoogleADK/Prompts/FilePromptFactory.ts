import type { PromptFactory } from 'Hackathon.Assistant/Domain/Prompts/PromptFactory.ts';
import type { Prompt } from 'Hackathon.Assistant/Domain/Prompts/Prompt.ts';
import { PromptType } from 'Hackathon.Assistant/Domain/Prompts/PromptType.ts';
import type { MultiModalPrompt } from 'Hackathon.Assistant/Domain/Prompts/MultiModalPrompt.ts';
import { IllegalArgumentException } from '@domaincrafters/std';

type ScanPromptFactoryInputData = {
    sessionUserId: string;
    image: {
        data: string;
        mimeType: string;
    };
};

const ASSISTANT_PROMPTS_DIRECTORY = new URL(
    '../../../../../agents/hackathon-registration-assistant/prompts/',
    import.meta.url,
);

export class FilePromptFactory implements PromptFactory {
    createPrompt(type: PromptType, data: unknown): Prompt {
        switch (type) {
            case PromptType.SCAN_HACKATHON_REGISTRATION_FORM:
                return this.createScanPrompt(data);

            default:
                throw new IllegalArgumentException(
                    `Prompt type ${type} is not supported by FilePromptFactory.`,
                );
        }
    }

    private createScanPrompt(data: unknown): MultiModalPrompt {
        const scanData: ScanPromptFactoryInputData = data as ScanPromptFactoryInputData;

        if (!scanData?.image?.data || !scanData?.image?.mimeType || !scanData?.sessionUserId) {
            throw new IllegalArgumentException(
                'Scan prompt data must contain sessionUserId, image.data and image.mimeType.',
            );
        }

        const instructions = this.readPromptFile(
            new URL('scan-registration-form.md', ASSISTANT_PROMPTS_DIRECTORY),
            `Analyze the attached handwritten image and decide if it is a hackathon registration form.
If valid, extract group name, goal and members (firstName/lastName), then use tools to register the group.
Do not invent missing fields and keep the final message concise.`,
        );

        return {
            type: PromptType.SCAN_HACKATHON_REGISTRATION_FORM,
            role: 'user',
            sessionUserId: scanData.sessionUserId,
            content: instructions,
            images: [
                {
                    data: scanData.image.data,
                    mimeType: scanData.image.mimeType,
                },
            ],
        };
    }

    private readPromptFile(fileUrl: URL, fallback: string): string {
        const content = Deno.readTextFileSync(fileUrl).trim();

        if (content.length === 0) {
            return fallback;
        }

        return content;
    }
}
