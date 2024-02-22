import { SchoolOwnershipType } from "src/nomenclatures/enums/school/school-ownership-type.enum";
import { SchoolState } from "src/nomenclatures/enums/school/school-state.enum";
import { SchoolType } from "src/nomenclatures/enums/school/school-type.enum";
import { District } from "src/nomenclatures/models/settlement/district.model";
import { Municipality } from "src/nomenclatures/models/settlement/municipality.model";
import { Settlement } from "src/nomenclatures/models/settlement/settlement.model";
import { NomenclatureFilterDto } from "../nomenclature-filter.dto";

export class SchoolFilterDto extends NomenclatureFilterDto {
    state: SchoolState;
    type: SchoolType;
    ownershipType: SchoolOwnershipType;
    settlementId: number;
    settlement: Settlement;
    municipalityId: number;
    municipality: Municipality;
    districtId: number;
    district: District;
}