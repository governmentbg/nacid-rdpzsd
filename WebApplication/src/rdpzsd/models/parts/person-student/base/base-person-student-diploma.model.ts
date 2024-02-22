import { EntityVersion } from "src/shared/models/entity-version.model";
import { RdpzsdAttachedFile } from "src/shared/models/rdpzsd-attached-file.model";

export class BasePersonStudentDiploma<TDiplomaFile extends RdpzsdAttachedFile> extends EntityVersion {
    diplomaNumber: string;
    registrationDiplomaNumber: string;
    diplomaDate: Date;
    file: TDiplomaFile;
    isValid = true;
}