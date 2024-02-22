import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { Country } from "src/nomenclatures/models/settlement/country.model";
import { LotState } from "src/rdpzsd/enums/lot-state.enum";
import { UserEmsDto } from "src/users/dtos/ems/user-ems.dto";

export class PersonApprovalSearchDto {
  lotId: number;
  uan: string;
  state: LotState;
  createDate: Date;
  createUserId: number;
  createUser: UserEmsDto;

  createInstitution: Institution;
  createInstitutionId: number;
  createSubordinate: Institution;
  createSubordinateId: number;

  fullName: string;
  fullNameAlt: string;
  birthCountry: Country;
  birthDate: Date;
  foreignerBirthSettlement: string;
}