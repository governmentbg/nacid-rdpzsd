import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";

export class StickerReportFilterDto {
    institutionId: number;
    institution: Institution;
    stickerState = StudentStickerState.stickerForPrint;
    stickerYear: number;
}