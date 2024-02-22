import { BasePersonSemester } from "../base/base-person-semester.model";
import { BasePersonStudentDoctoral } from "../base/base-person-student-doctoral.model";
import { PartInfo } from "../base/part-info.model";
import { PersonStudentPeRecognitionDocument } from "../person-student/person-student-pe-recognition-document.model";
import { PersonStudent } from "../person-student/person-student.model";
import { PersonDoctoralInfo } from "./person-doctoral-info.model";
import { PersonDoctoralSemester } from "./person-doctoral-semester.model";

export class BasePersonDoctoral<TPartInfo extends PartInfo, TSemester extends BasePersonSemester> extends BasePersonStudentDoctoral<TPartInfo, TSemester> {
    startDate: Date;
    endDate: Date;
}

export class PersonDoctoral extends BasePersonDoctoral<PersonDoctoralInfo, PersonDoctoralSemester> {
    pePart: PersonStudent;
    lotId: number;
}