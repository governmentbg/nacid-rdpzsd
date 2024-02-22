import { EducationalForm } from "src/nomenclatures/models/institution/educational-form.model";
import { EducationalQualification } from "src/nomenclatures/models/institution/educational-qualification.model";
import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { ResearchArea } from "src/nomenclatures/models/research-area.model";
import { FilterDto } from "src/shared/dtos/filter.dto";

export class InstitutionSpecialityFilterDto extends FilterDto {
  institutionRootId: number;
  institutionRoot: Institution;
  institutionId: number;
  institution: Institution;
  educationalQualificationAlias: string;
  educationalFormId: number;
  educationalForm: EducationalForm;
  educationalQualificationId: number;
  educationalQualification: EducationalQualification;
  duration: number;
  researchAreaId: number;
  researchArea: ResearchArea;
  isRegulated: boolean;
  isForCadets: boolean;
  specialityName: string;
  specialityCode: string;
  getInstitutionSpecialitiesByPermissions = false;
  showJointSpecialitiesOnly: boolean;
  specialityIdNumber: string;
  // For paginator
  currentPage = 1;
}