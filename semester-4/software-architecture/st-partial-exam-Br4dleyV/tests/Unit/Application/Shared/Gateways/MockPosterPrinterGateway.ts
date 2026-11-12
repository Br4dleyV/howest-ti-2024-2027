/*
import type { PosterPrinterGateway, PosterPrinterInput } from '@/Application/Ports/Gateways/mod.ts';

interface MockPosterPrinterGatewayOptions {
    shouldThrowOnPrint?: boolean;
    errorMessage?: string;
}

export function createMockPosterPrinterGateway(
    options: MockPosterPrinterGatewayOptions = {},
): PosterPrinterGateway & {
    printCalled: boolean;
    printCallCount: number;
    receivedInput: PosterPrinterInput | null;
    printedInputs: PosterPrinterInput[];
} {
    const mock = {
        printCalled: false,
        printCallCount: 0,
        receivedInput: null as PosterPrinterInput | null,
        printedInputs: [] as PosterPrinterInput[],
        print: async (input: PosterPrinterInput): Promise<void> => {
            mock.printCalled = true;
            mock.printCallCount++;
            mock.receivedInput = input;
            mock.printedInputs.push(input);

            if (options.shouldThrowOnPrint) {
                throw new Error(options.errorMessage ?? 'printer failed');
            }
        },
    };

    return mock as PosterPrinterGateway & {
        printCalled: boolean;
        printCallCount: number;
        receivedInput: PosterPrinterInput | null;
        printedInputs: PosterPrinterInput[];
    };
}
*/