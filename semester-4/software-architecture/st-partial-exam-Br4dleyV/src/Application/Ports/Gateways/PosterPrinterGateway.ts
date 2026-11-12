export interface PosterPrinterInput {
    teamId: string;
    heroId: string;
    memberName: string;
    teamName: string;
}

export interface PosterPrinterGateway {
    print(input: PosterPrinterInput): Promise<void>;
}
