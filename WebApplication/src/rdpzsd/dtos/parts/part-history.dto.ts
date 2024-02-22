export class PartHistoryDto<TPart, THistory> {
    actual: TPart;
    histories: THistory[] = [];
}