import { School } from "src/nomenclatures/models/others/school.model";
import { Country } from "src/nomenclatures/models/settlement/country.model";
import { Settlement } from "src/nomenclatures/models/settlement/settlement.model";
import { PartInfo } from "../../base/part-info.model";
import { Part } from "../../base/part.model";
import { RdpzsdAttachedFile } from './../../../../../shared/models/rdpzsd-attached-file.model';

export class BasePersonSecondary<TPartInfo extends PartInfo, TRecognitionDocument extends RdpzsdAttachedFile> extends Part<TPartInfo>
{
  graduationYear: number;

  countryId: number;
  country: Country;

  schoolId: number;
  school: School;

  foreignSchoolName: string;
  profession: string;
  diplomaNumber: string;
  diplomaDate: Date;

  recognitionNumber: string;
  recognitionDate: Date;
  personSecondaryRecognitionDocument: TRecognitionDocument;

  missingSchoolFromRegister: boolean;
  missingSchoolName: string;
  missingSchoolSettlementId: number;
  missingSchoolSettlement: Settlement;
}