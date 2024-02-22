import { PersonStudentPeRecognitionDocumentHistory } from "../../person-student/history/person-student-pe-recognition-document-history.model";
import { BasePersonDoctoral } from "../person-doctoral.model";
import { PersonDoctoralHistoryInfo } from "./person-doctoral-history-info.model";
import { PersonDoctoralSemesterHistory } from "./person-doctoral-semester-history.model";

export class PersonDoctoralHistory extends BasePersonDoctoral<PersonDoctoralHistoryInfo, PersonDoctoralSemesterHistory> {
    partId: number;
}