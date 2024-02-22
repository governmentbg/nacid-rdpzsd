import { BasePersonSecondary } from "../base/base-person-secondary.model";
import { PersonSecondaryHistoryInfo } from "./person-secondary-history-info.model";
import { PersonSecondaryRecognitionDocumentHistory } from "./person-secondary-recognition-document-history.model";

export class PersonSecondaryHistory extends BasePersonSecondary<PersonSecondaryHistoryInfo, PersonSecondaryRecognitionDocumentHistory> {
    partId: number;
}