import { NomenclatureFilterDto } from "../nomenclature-filter.dto";

export class AdmissionReasonFilterDto extends NomenclatureFilterDto {
  oldCode: string;
  shortName: string;
  shortNameAlt: string;
}