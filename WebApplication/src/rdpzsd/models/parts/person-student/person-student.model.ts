import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { RdpzsdAttachedFile } from "src/shared/models/rdpzsd-attached-file.model";
import { BasePersonSemester } from "../base/base-person-semester.model";
import { BasePersonStudentDoctoral } from "../base/base-person-student-doctoral.model";
import { PartInfo } from "../base/part-info.model";
import { BasePersonStudentDiploma } from "./base/base-person-student-diploma.model";
import { BasePersonStudentDuplicateDiploma } from "./base/base-person-student-duplicate-diploma.model";
import { BasePersonStudentProtocol } from "./base/base-person-student-protocol.model";
import { PersonStudentDiplomaFile } from "./person-student-diploma-file.model";
import { PersonStudentDiploma } from "./person-student-diploma.model";
import { PersonStudentDuplicateDiplomaFile } from "./person-student-duplicate-diploma-file.model";
import { PersonStudentDuplicateDiploma } from "./person-student-duplicate-diploma.model";
import { PersonStudentInfo } from "./person-student-info.model";
import { PersonStudentProtocol } from "./person-student-protocol.model";
import { PersonStudentSemester } from "./person-student-semester.model";
import { PersonStudentStickerNote } from "./person-student-sticker-note.model";

export class BasePersonStudent<TPartInfo extends PartInfo, TSemester extends BasePersonSemester, TDiploma extends BasePersonStudentDiploma<TDiplomaFile>, TDiplomaFile extends RdpzsdAttachedFile, TDuplicateDiploma extends BasePersonStudentDuplicateDiploma<TDuplicateDiplomaFile>, TDuplicateDiplomaFile extends RdpzsdAttachedFile, TProtocol extends BasePersonStudentProtocol> extends BasePersonStudentDoctoral<TPartInfo, TSemester> {
    facultyNumber: string;

    stickerState: StudentStickerState;
    stickerYear: number;

    protocols: TProtocol[] = [];
    diploma: TDiploma;
    duplicateDiplomas: TDuplicateDiploma[] = [];
}

export class PersonStudent extends BasePersonStudent<PersonStudentInfo, PersonStudentSemester, PersonStudentDiploma, PersonStudentDiplomaFile, PersonStudentDuplicateDiploma, PersonStudentDuplicateDiplomaFile, PersonStudentProtocol> {
    pePart: PersonStudent;
    lotId: number;
    stickerNotes: PersonStudentStickerNote[] = [];
}