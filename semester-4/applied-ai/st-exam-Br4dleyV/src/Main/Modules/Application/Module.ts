import { ServiceCollection, ServiceProvider } from '@domaincrafters/di';

import { Config } from 'Hackathon.Assistant/Infrastructure/Shared/mod.ts';

import type { AgentFactory } from 'Hackathon.Assistant/Domain/Agents/AgentFactory.ts';
import type { PromptFactory } from 'Hackathon.Assistant/Domain/Prompts/PromptFactory.ts';
import { ScanHackathonRegistrationFormUseCase } from 'Hackathon.Assistant/Application/mod.ts';
import { Module } from 'Hackathon.Assistant/Main/Shared/mod.ts';

export class Application implements Module {
    add(serviceCollection: ServiceCollection, _config: Config): void {
        serviceCollection.addScoped(
            ScanHackathonRegistrationFormUseCase.name,
            async (serviceProvider: ServiceProvider) => {
                const agentFactoryResult = await serviceProvider.getService<AgentFactory>('AgentFactory');
                const promptFactoryResult = await serviceProvider.getService<PromptFactory>('PromptFactory');

                return new ScanHackathonRegistrationFormUseCase(
                    agentFactoryResult.value,
                    promptFactoryResult.value,
                );
            },
        );
    }

    async use(_serviceProvider: ServiceProvider): Promise<void> {
        // No-op
    }

    static create(): Module {
        return new Application();
    }
}
