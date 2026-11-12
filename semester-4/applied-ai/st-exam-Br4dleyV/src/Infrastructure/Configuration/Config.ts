import { z } from 'zod';
import '@std/dotenv/load';

const envSchema = z.object({
    GOOGLE_API_KEY: z.string().min(1, 'GOOGLE_API_KEY is required')
});

const env = Deno.env.toObject();
const result = envSchema.safeParse(env);

if (!result.success) {
    console.error('❌ Invalid environment variables:', result.error.format());
    Deno.exit(1);
}

export type AppConfig = z.infer<typeof envSchema>;
export const config: AppConfig = result.data;
