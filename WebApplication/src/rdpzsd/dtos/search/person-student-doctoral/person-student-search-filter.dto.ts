import { Period } from "src/nomenclatures/models/others/period.model";
import { BasePersonStudentDoctoralFilterDto } from "./base-person-student-doctoral-filter.dto";

export class PersonStudentSearchFilterDto extends BasePersonStudentDoctoralFilterDto {
    periodId: number;
    period: Period;
}