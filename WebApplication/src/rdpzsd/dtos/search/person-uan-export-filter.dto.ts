import { Country } from "src/nomenclatures/models/settlement/country.model";
import { PersonLotNewFilterType } from "src/rdpzsd/enums/search/person-lot-new-filter-type.enum";
import { FilterDto } from "src/shared/dtos/filter.dto";

export class PersonUanExportFilterDto extends FilterDto {
    filterType = PersonLotNewFilterType.identificationNumber;
  
    searchType: string;
    uan: string;
    uin: string;
    foreignerNumber: string;
  
    birthCountryId: number;
    birthCountry: Country;
    birthDate: Date;
  }