import { ToolId } from './ToolId.ts';

export interface Tool {
    id: ToolId;
    name: string;
    description: string;
}
