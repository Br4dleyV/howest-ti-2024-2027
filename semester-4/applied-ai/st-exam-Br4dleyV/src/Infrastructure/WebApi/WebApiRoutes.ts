import { Router, RouterBuilder, Routes } from 'Hackathon.Assistant/Infrastructure/WebApi/Shared/mod.ts';
import {
    ScanHackathonRegistrationFormController,
} from './Controllers/ScanHackathonRegistrationFormController.ts';

export class WebApiRoutes implements Routes {
    map(routerBuilder: RouterBuilder): Router {
        return routerBuilder
            .mapPost(
                ScanHackathonRegistrationFormController.name,
                '/api/assistant/scan-hackathon-registration-form',
            )
            .build();
    }
}
