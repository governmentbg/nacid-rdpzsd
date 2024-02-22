import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { EntityVersion } from "src/shared/models/entity-version.model";
import { RdpzsdAttachedFile } from "src/shared/models/rdpzsd-attached-file.model";

export class BasePersonStudentDuplicateDiploma<TDuplicateDiplomaFile extends RdpzsdAttachedFile> extends EntityVersion {
    partId: number;
    duplicateStickerState: StudentStickerState;
    duplicateStickerYear: number;
    duplicateDiplomaNumber: string;
    duplicateRegistrationDiplomaNumber: string;
    duplicateDiplomaDate: Date;
    file: TDuplicateDiplomaFile;
    isValid = true;
}