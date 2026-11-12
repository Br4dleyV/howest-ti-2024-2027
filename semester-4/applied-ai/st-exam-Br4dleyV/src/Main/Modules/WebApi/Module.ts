import { ServiceCollection, ServiceProvider } from '@domaincrafters/di';
import { Config } from 'Hackathon.Assistant/Infrastructure/Shared/mod.ts';
import { Module } from 'Hackathon.Assistant/Main/Shared/mod.ts';
import {
    CorsRulesMiddleware,
    GlobalExceptionHandlerMiddleware,
    HealthCheckMiddleware,
    WebApiRoutes,
} from 'Hackathon.Assistant/Infrastructure/WebApi/mod.ts';
import { OakServices, OakWebServer } from 'Hackathon.Assistant/Infrastructure/WebApi/Shared/mod.ts';
import { WebApiControllerFactory } from './WebApiControllerFactory.ts';

export class WebApi implements Module {
    add(serviceCollection: ServiceCollection, config: Config): void {
        OakServices
            .add(serviceCollection, config, new WebApiRoutes())
            .addMiddleware(serviceCollection, [
                CorsRulesMiddleware.Add(),
                GlobalExceptionHandlerMiddleware.Add(),
                HealthCheckMiddleware.Add(),
            ]);

        serviceCollection.addScoped(
            'WebApiControllerFactory',
            (serviceProvider: ServiceProvider) => {
                return Promise.resolve(new WebApiControllerFactory(serviceProvider));
            },
        );
    }

    async use(serviceProvider: ServiceProvider): Promise<void> {
        const webServer: OakWebServer =
            (await serviceProvider.getService<OakWebServer>('webserver'))
                .getOrThrow('Web server not found');

        webServer.run();
    }

    static create(): Module {
        return new WebApi();
    }
}
