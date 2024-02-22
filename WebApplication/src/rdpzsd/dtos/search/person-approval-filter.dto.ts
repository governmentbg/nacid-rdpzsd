import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { ApprovalState } from "src/rdpzsd/enums/approval-state.enum";
import { FilterDto } from "src/shared/dtos/filter.dto";

export class PersonApprovalFilterDto extends FilterDto {
  createInstitutionId: number;
  createInstitution: Institution;
  createSubordinateId: number;
  createSubordinate: Institution;

  state: ApprovalState;
}