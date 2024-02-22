import { NomenclatureCode } from "../base/nomenclature-code.model";
import { District } from "./district.model";
import { Municipality } from "./municipality.model";

export class Settlement extends NomenclatureCode {
  municipalityId: number;
  municipality: Municipality;
  districtId: number;
  district: District;
  municipalityCode: string;
  districtCode: string;
  municipalityCode2: string;
  districtCode2: string;
  typeName: string;
  settlementName: string;
  typeCode: string;
  mayoraltyCode: string;
  category: string;
  altitude: string;
  isDistrict: boolean;
}