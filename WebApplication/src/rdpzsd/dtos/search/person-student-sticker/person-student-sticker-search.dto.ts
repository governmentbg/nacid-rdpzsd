import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { StudentEvent } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatus } from "src/nomenclatures/models/student-status/student-status.model";
import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { PersonStudentStickerNote } from "src/rdpzsd/models/parts/person-student/person-student-sticker-note.model";
import { PersonSearchDto } from "../person-search.dto";

export class PersonStudentStickerSearchDto extends PersonSearchDto {
    partId: number;
    duplicateDiplomaId: number;
    isDuplicate: boolean;
    stickerYear: number;
    stickerState: StudentStickerState;
    subordinate: Institution;
    speciality: string;
    specialityAlt: string;
    studentStatus: StudentStatus;
    studentEvent: StudentEvent;
    stickerNotes: PersonStudentStickerNote[] = [];

    // Client only
    isMarked = false;
}
