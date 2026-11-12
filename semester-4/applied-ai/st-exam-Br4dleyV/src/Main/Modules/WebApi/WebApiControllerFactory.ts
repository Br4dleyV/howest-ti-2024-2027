import { ServiceProvider } from '@domaincrafters/di';
import {
    ControllerFactory,
    RouterContext,
    WebApiController,
} from 'Hackathon.Assistant/Infrastructure/WebApi/Shared/mod.ts';
import { IllegalStateException } from '@domaincrafters/std';
import {
    ScanHackathonRegistrationFormUseCase,
} from 'Hackathon.Assistant/Application/mod.ts';

import {
    ScanHackathonRegistrationFormController,
} from '../../../Infrastructure/WebApi/Controllers/ScanHackathonRegistrationFormController.ts';

export class WebApiControllerFactory implements ControllerFactory {
    private readonly _serviceProvider: ServiceProvider;

    constructor(serviceProvider: ServiceProvider) {
        this._serviceProvider = serviceProvider;
    }

    async create(ctx: RouterContext<string>): Promise<WebApiController> {
        if (!ctx.routeName) {
            throw new IllegalStateException('Route name is not defined');
        }

        switch (ctx.routeName) {
            case ScanHackathonRegistrationFormController.name:
                return await this.scanHackathonRegistrationFormController();

            default:
                throw new IllegalStateException(
                    `Controller for route ${ctx.routeName} is not defined`,
                );
        }
    }

    private async scanHackathonRegistrationFormController(): Promise<
        ScanHackathonRegistrationFormController
    > {
        const scanUseCase =
            (await this._serviceProvider.getService<ScanHackathonRegistrationFormUseCase>(
                ScanHackathonRegistrationFormUseCase.name,
            )).value;

        return new ScanHackathonRegistrationFormController(scanUseCase);
    }
}
