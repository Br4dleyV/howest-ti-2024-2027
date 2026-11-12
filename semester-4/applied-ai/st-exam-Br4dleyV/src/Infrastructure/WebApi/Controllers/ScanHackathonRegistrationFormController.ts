import { encodeBase64 } from '@std/encoding/base64';
import type { RouterContext } from '@oak/oak';
import type {
    ScanHackathonRegistrationFormInputData,
    UseCase,
} from 'Hackathon.Assistant/Application/Contracts/mod.ts';
import { WebApiController } from '../Shared/mod.ts';

export class ScanHackathonRegistrationFormController implements WebApiController {
    constructor(
        private readonly _scanUseCase: UseCase<ScanHackathonRegistrationFormInputData, void>,
    ) {
    }


    async handle(ctx: RouterContext<string>): Promise<void> {
        const form = await ctx.request.body.formData();
        const fileEntry = form.get('registrationFormImage');
        const maxFileSize = 5 * 1024 * 1024;

        if (!(fileEntry instanceof File)) {
            ctx.response.status = 400;
            ctx.response.body = {
                error: 'Image file is required (field name: registrationFormImage)',
            };
            return;
        }

        if (fileEntry.size > maxFileSize) {
            ctx.response.status = 400;
            ctx.response.body = { error: 'Image file exceeds 5MB limit' };
            return;
        }

        console.log(`Received file: ${fileEntry.name}, size: ${fileEntry.size} bytes`);

        const fileBuffer = await fileEntry.arrayBuffer();
        const base64Image = encodeBase64(fileBuffer);
        const mimeType = fileEntry.type || 'image/jpeg';
        const sessionUserId =
            ctx.request.headers.get('x-session-user-id') || crypto.randomUUID();

        console.log(`Base64 image size: ${base64Image.length} characters`);

        await this._scanUseCase.execute({
            base64Image,
            mimeType,
            sessionUserId,
        });


        ctx.response.status = 200;
        ctx.response.body = {
            message: 'Hackathon registration form scanned successfully',
        };
    }
}
