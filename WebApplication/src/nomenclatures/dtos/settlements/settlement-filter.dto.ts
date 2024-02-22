import { District } from "src/nomenclatures/models/settlement/district.model";
import { Municipality } from "src/nomenclatures/models/settlement/municipality.model";
import { NomenclatureCodeFilterDto } from "../nomenclature-code-filter.dto";

export class SettlementFilterDto extends NomenclatureCodeFilterDto {
  districtId: number;
  district: District;
  municipalityId: number;
  municipality: Municipality;
}