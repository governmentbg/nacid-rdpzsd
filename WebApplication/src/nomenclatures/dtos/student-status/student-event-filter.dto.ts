import { EducationalQualification } from "src/nomenclatures/models/institution/educational-qualification.model";
import { StudentStatus } from "src/nomenclatures/models/student-status/student-status.model";
import { NomenclatureFilterDto } from "../nomenclature-filter.dto";

export class StudentEventFilterDto extends NomenclatureFilterDto {
    studentStatusId: number;
    studentStatus: StudentStatus;
    educationalQualificationId: number;
    educationalQualification: EducationalQualification;
}