import { NomenclatureCode } from "../base/nomenclature-code.model";
import { ResearchArea } from "../research-area.model";
import { EducationalQualification } from "./educational-qualification.model";

export class Speciality extends NomenclatureCode {
  researchAreaId: number;
  researchArea: ResearchArea;
  educationalQualificationId: number;
  educationalQualification: EducationalQualification;
  isRegulated: boolean;
}