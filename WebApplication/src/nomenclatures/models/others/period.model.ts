import { Semester } from "src/shared/enums/semester.enum";
import { Nomenclature } from "../base/nomenclature.model";

export class Period extends Nomenclature {
    year: number;
    semester: Semester;
}