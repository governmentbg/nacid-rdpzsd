import { InstitutionCommitState } from "src/nomenclatures/enums/institution/institution-commit-state.enum";
import { OrganizationType } from "src/nomenclatures/enums/institution/organization-type.enum";
import { OwnershipType } from "src/nomenclatures/enums/institution/ownership-type.enum";
import { NomenclatureHierarchy } from "../base/nomenclature-hierarchy.model";
import { District } from "../settlement/district.model";
import { Municipality } from "../settlement/municipality.model";
import { Settlement } from "../settlement/settlement.model";

export class Institution extends NomenclatureHierarchy {
  lotNumber: number;
  parent: Institution;
  root: Institution;
  state: InstitutionCommitState;
  uic: string;
  shortName: string;
  shortNameAlt: string;
  organizationType: OrganizationType;
  ownershipType: OwnershipType;
  settlementId: number;
  settlement: Settlement;
  municipalityId: number;
  municipality: Municipality;
  districtId: number;
  district: District;
  isResearchUniversity: boolean;
}