import { ServiceCollection, ServiceProvider } from '@domaincrafters/di';
import { InMemorySessionService } from '@google/adk';

import type { AgentFactory } from 'Hackathon.Assistant/Domain/Agents/AgentFactory.ts';
import type { PromptFactory } from 'Hackathon.Assistant/Domain/Prompts/PromptFactory.ts';
import type { Logger } from 'Hackathon.Assistant/Application/Contracts/mod.ts';
import { AdkAgentFactory } from 'Hackathon.Assistant/Infrastructure/AI/GoogleADK/AdkAgentFactory.ts';
import { AdkMessageFactory } from 'Hackathon.Assistant/Infrastructure/AI/GoogleADK/AdkMessageFactory.ts';
import { FilePromptFactory } from 'Hackathon.Assistant/Infrastructure/AI/GoogleADK/Prompts/FilePromptFactory.ts';
import { McpToolsetFactory } from 'Hackathon.Assistant/Infrastructure/AI/GoogleADK/Tools/McpToolsetFactory.ts';
import { Config } from 'Hackathon.Assistant/Infrastructure/Shared/mod.ts';
import { ConsoleLogger } from 'Hackathon.Assistant/Infrastructure/Shared/ConsoleLogger.ts';
import { Module } from 'Hackathon.Assistant/Main/Shared/mod.ts';

export class AI implements Module {
    add(serviceCollection: ServiceCollection, config: Config): void {
        const googleApiKey = config.get('GOOGLE_API_KEY', '');
        if (googleApiKey.length > 0) {
            if (!Deno.env.get('GEMINI_API_KEY')) {
                Deno.env.set('GEMINI_API_KEY', googleApiKey);
            }
            if (!Deno.env.get('GOOGLE_GENAI_API_KEY')) {
                Deno.env.set('GOOGLE_GENAI_API_KEY', googleApiKey);
            }
        }

        serviceCollection.addSingleton(
            InMemorySessionService.name,
            () => Promise.resolve(new InMemorySessionService()),
        );

        serviceCollection.addSingleton(
            AdkMessageFactory.name,
            () => Promise.resolve(new AdkMessageFactory()),
        );

        serviceCollection.addScoped(
            McpToolsetFactory.name,
            async (serviceProvider: ServiceProvider) => {
                const loggerResult = await serviceProvider.getService<Logger>(ConsoleLogger.name);

                return new McpToolsetFactory(config, loggerResult.value);
            },
        );

        serviceCollection.addScoped(
            'PromptFactory',
            () => Promise.resolve<PromptFactory>(new FilePromptFactory()),
        );

        serviceCollection.addScoped(
            'AgentFactory',
            async (serviceProvider: ServiceProvider) => {
                const sessionServiceResult = await serviceProvider.getService<InMemorySessionService>(
                    InMemorySessionService.name,
                );
                const messageFactoryResult = await serviceProvider.getService<AdkMessageFactory>(
                    AdkMessageFactory.name,
                );
                const toolsetFactoryResult = await serviceProvider.getService<McpToolsetFactory>(
                    McpToolsetFactory.name,
                );

                return new AdkAgentFactory(
                    sessionServiceResult.value,
                    messageFactoryResult.value,
                    toolsetFactoryResult.value,
                ) as AgentFactory;
            },
        );
    }

    use(_serviceProvider: ServiceProvider): Promise<void> {
        return Promise.resolve();
    }

    static create(): Module {
        return new AI();
    }
}
