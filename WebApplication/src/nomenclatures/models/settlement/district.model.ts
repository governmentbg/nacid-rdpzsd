import { NomenclatureCode } from "../base/nomenclature-code.model";

export class District extends NomenclatureCode {
  code2: string;
  secondLevelRegionCode: string;
  mainSettlementCode: string;
}