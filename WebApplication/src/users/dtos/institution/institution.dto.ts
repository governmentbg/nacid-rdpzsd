import { OrganizationType } from "src/nomenclatures/enums/institution/organization-type.enum";
import { BaseInstitutionDto } from "./base/base-institution.dto";
import { ChildInstitutionDto } from "./child-institution.dto";

export class InstitutionDto extends BaseInstitutionDto {
  logo: string;

  hasBachelor: boolean;
  hasMaster: boolean;
  hasDoctoral: boolean;
  organizationType: OrganizationType;

  childInstitutions: ChildInstitutionDto[] = [];
}