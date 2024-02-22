import { BasePersonStudentDuplicateDiploma } from "./base/base-person-student-duplicate-diploma.model";
import { PersonStudentDuplicateDiplomaFile } from "./person-student-duplicate-diploma-file.model";

export class PersonStudentDuplicateDiploma extends BasePersonStudentDuplicateDiploma<PersonStudentDuplicateDiplomaFile> {

    // Client only
    isEditMode = false;
    originalModel: PersonStudentDuplicateDiploma = null;
}