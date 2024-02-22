import { OrganizationType } from "src/nomenclatures/enums/institution/organization-type.enum";
import { OwnershipType } from "src/nomenclatures/enums/institution/ownership-type.enum";
import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { District } from "src/nomenclatures/models/settlement/district.model";
import { Municipality } from "src/nomenclatures/models/settlement/municipality.model";
import { Settlement } from "src/nomenclatures/models/settlement/settlement.model";
import { NomenclatureHierarchyFilterDto } from "../nomenclature-hierarchy-filter.dto";

export class InstitutionFilterDto extends NomenclatureHierarchyFilterDto<Institution> {
  uic: string;
  lotNumber: number;
  organizationType: OrganizationType;
  organizationTypes: OrganizationType[] = [];
  ownershipType: OwnershipType;
  districtId: number;
  district: District;
  municipalityId: number;
  municipality: Municipality;
  settlementId: number;
  settlement: Settlement;
}