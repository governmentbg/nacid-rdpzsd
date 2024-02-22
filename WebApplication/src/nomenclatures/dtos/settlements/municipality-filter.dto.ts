import { District } from "src/nomenclatures/models/settlement/district.model";
import { NomenclatureCodeFilterDto } from "../nomenclature-code-filter.dto";

export class MunicipalityFilterDto extends NomenclatureCodeFilterDto {
  districtId: number;
  district: District;
}