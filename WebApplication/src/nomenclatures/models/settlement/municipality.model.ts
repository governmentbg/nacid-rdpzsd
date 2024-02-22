import { NomenclatureCode } from "../base/nomenclature-code.model";
import { District } from "./district.model";

export class Municipality extends NomenclatureCode {
  districtId: number;
  district: District;
  code2: string;
  mainSettlementCode: string;
  category: string;
}