import { BasePersonStudent } from "../person-student.model";
import { PersonStudentDiplomaFileHistory } from "./person-student-diploma-file-history.model";
import { PersonStudentDiplomaHistory } from "./person-student-diploma-history.model";
import { PersonStudentDuplicateDiplomaFileHistory } from "./person-student-duplicate-diploma-file-history.model";
import { PersonStudentDuplicateDiplomaHistory } from "./person-student-duplicate-diploma-history.model";
import { PersonStudentHistoryInfo } from "./person-student-history-info.model";
import { PersonStudentProtocolHistory } from "./person-student-protocol-history.model";
import { PersonStudentSemesterHistory } from "./person-student-semester-history.model";

export class PersonStudentHistory extends BasePersonStudent<PersonStudentHistoryInfo, PersonStudentSemesterHistory, PersonStudentDiplomaHistory, PersonStudentDiplomaFileHistory, PersonStudentDuplicateDiplomaHistory, PersonStudentDuplicateDiplomaFileHistory, PersonStudentProtocolHistory> {
    partId: number;
}