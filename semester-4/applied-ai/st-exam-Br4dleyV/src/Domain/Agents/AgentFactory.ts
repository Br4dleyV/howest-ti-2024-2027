import { Agent } from './Agent.ts';
import { AgentType } from './AgentType.ts';

export interface AgentFactory {
    createAgent(type: AgentType): Agent;
}
