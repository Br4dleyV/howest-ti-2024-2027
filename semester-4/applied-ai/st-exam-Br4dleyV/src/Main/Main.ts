import * as mod from 'Hackathon.Assistant/Main/Modules/mod.ts';
import { DIAwareMain } from 'Hackathon.Assistant/Main/Shared/mod.ts';

const modules = [
    mod.Tools.create(),
    mod.AI.create(),
    mod.Application.create(),
    mod.WebApi.create()
];

export class Main extends DIAwareMain {
    static async init(): Promise<Main> {
        return await new Main().load(modules);
    }
}

if (import.meta.main) {
    await Main.init();
}
