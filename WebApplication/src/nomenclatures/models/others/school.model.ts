import { SchoolOwnershipType } from "src/nomenclatures/enums/school/school-ownership-type.enum";
import { SchoolState } from "src/nomenclatures/enums/school/school-state.enum";
import { SchoolType } from "src/nomenclatures/enums/school/school-type.enum";
import { Nomenclature } from "../base/nomenclature.model";
import { District } from "../settlement/district.model";
import { Municipality } from "../settlement/municipality.model";
import { Settlement } from "../settlement/settlement.model";

export class School extends Nomenclature {
    state: SchoolState;
    type: SchoolType;
    ownershipType: SchoolOwnershipType;
    settlementId: number;
    settlement: Settlement;
    municipalityId: number;
    municipality: Municipality;
    districtId: number;
    district: District;
    migrationId: number;
    parentId: number;
    parent: School;
}