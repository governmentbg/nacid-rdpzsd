import { BasePersonStudentProtocol } from "./base/base-person-student-protocol.model";

export class PersonStudentProtocol extends BasePersonStudentProtocol {

    // Client only
    isEditMode = false;
    originalModel: PersonStudentProtocol = null;
}