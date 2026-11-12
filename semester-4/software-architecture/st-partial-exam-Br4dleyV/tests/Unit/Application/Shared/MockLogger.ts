import type { Logger } from '@/Application/Ports/mod.ts';

export type MockLogger = Logger & {
    debugCalls: number;
    infoCalls: number;
    warnCalls: number;
    errorCalls: number;
    debugLogs: Array<{ message: string; context?: Record<string, unknown> }>;
    infoLogs: Array<{ message: string; context?: Record<string, unknown> }>;
    warnLogs: Array<{ message: string; context?: Record<string, unknown> }>;
    errorLogs: Array<{ message: string; context?: Record<string, unknown> }>;
};

export function createMockLogger(): MockLogger {
    const mock: MockLogger = {
        debugCalls: 0,
        infoCalls: 0,
        warnCalls: 0,
        errorCalls: 0,
        debugLogs: [],
        infoLogs: [],
        warnLogs: [],
        errorLogs: [],
        debug: (message: string, context?: Record<string, unknown>) => {
            mock.debugCalls++;
            mock.debugLogs.push({ message, context });
        },
        info: (message: string, context?: Record<string, unknown>) => {
            mock.infoCalls++;
            mock.infoLogs.push({ message, context });
        },
        warn: (message: string, context?: Record<string, unknown>) => {
            mock.warnCalls++;
            mock.warnLogs.push({ message, context });
        },
        error: (message: string, context?: Record<string, unknown>) => {
            mock.errorCalls++;
            mock.errorLogs.push({ message, context });
        },
    };

    return mock;
}
