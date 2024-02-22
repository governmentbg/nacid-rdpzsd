import { BasePersonSecondary } from "./base/base-person-secondary.model";
import { PersonSecondaryInfo } from "./person-secondary-info.model";
import { PersonSecondaryRecognitionDocument } from "./person-secondary-recognition-document.model";

export class PersonSecondary extends BasePersonSecondary<PersonSecondaryInfo, PersonSecondaryRecognitionDocument> {
    fromRso: boolean;
    rsoIntId: number;
}