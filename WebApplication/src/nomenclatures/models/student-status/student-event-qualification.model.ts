import { EntityVersion } from "src/shared/models/entity-version.model";
import { EducationalQualification } from "../institution/educational-qualification.model";

export class StudentEventQualification extends EntityVersion {
    studentEventId: number;
    educationalQualificationId: number
    educationalQualification: EducationalQualification
}