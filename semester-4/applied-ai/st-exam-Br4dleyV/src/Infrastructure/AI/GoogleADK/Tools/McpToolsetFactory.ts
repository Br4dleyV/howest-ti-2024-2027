import { type BaseTool, FunctionTool } from '@google/adk';
import { z } from 'zod';
import type { Logger } from 'Hackathon.Assistant/Application/Contracts/mod.ts';
import type { Config } from 'Hackathon.Assistant/Infrastructure/Shared/mod.ts';

type GroupMember = {
    firstName: string;
    lastName: string;
};

type GroupPayload = {
    name: string;
    goal: string;
    members: GroupMember[];
};

export class McpToolsetFactory {
    private readonly _baseUrl: string;

    constructor(
        private readonly _config: Config,
        private readonly _logger: Logger,
    ) {
        this._baseUrl = this._config.get('HACKATHON_REGISTRATION_BASE_URL', 'http://localhost:40520');
    }

    createToolset(): BaseTool[] {
        const memberSchema = z.object({
            firstName: z.string().min(1),
            lastName: z.string().min(1),
        });

        const createGroupTool = new FunctionTool({
            name: 'create_hackathon_group',
            description:
                'Creates a new hackathon group registration in the hackathon-registration API.',
            parameters: z.object({
                name: z.string().min(1).describe('The group name.'),
                goal: z.string().min(1).describe('The group goal.'),
                members: z.array(memberSchema)
                    .min(2)
                    .max(5)
                    .describe('List of members with firstName and lastName.'),
            }),
            execute: async (args: GroupPayload) => {
                this._logger.info('Calling create_hackathon_group tool', {
                    name: args.name,
                    memberCount: args.members.length,
                });

                const created = await this.request('POST', '/groups', args);

                return {
                    status: 'success',
                    createdGroup: created,
                };
            },
        });

        const listGroupsTool = new FunctionTool({
            name: 'list_hackathon_groups',
            description: 'Lists already registered hackathon groups.',
            parameters: z.object({}),
            execute: async () => {
                const groups = await this.request('GET', '/groups');
                return {
                    status: 'success',
                    groups,
                };
            },
        });

        const listGoalsTool = new FunctionTool({
            name: 'list_hackathon_goals',
            description: 'Lists known/allowed hackathon goals from the registration API.',
            parameters: z.object({}),
            execute: async () => {
                const goals = await this.request('GET', '/groups/goals');
                return {
                    status: 'success',
                    goals,
                };
            },
        });

        return [createGroupTool, listGroupsTool, listGoalsTool];
    }

    private async request(method: string, path: string, body?: unknown): Promise<unknown> {
        const url = `${this._baseUrl}${path}`;
        const response = await fetch(url, {
            method,
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
            body: body ? JSON.stringify(body) : undefined,
        });

        const rawResponse = await response.text();

        if (!response.ok) {
            throw new Error(
                `Hackathon API request failed (${method} ${path}) with status ${response.status}: ${rawResponse}`,
            );
        }

        if (rawResponse.trim().length === 0) {
            return {};
        }

        try {
            return JSON.parse(rawResponse);
        } catch {
            return { raw: rawResponse };
        }
    }
}
