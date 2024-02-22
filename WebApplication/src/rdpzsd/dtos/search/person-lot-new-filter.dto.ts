import { Country } from "src/nomenclatures/models/settlement/country.model";
import { FilterDto } from "src/shared/dtos/filter.dto";
import { PersonLotNewFilterType } from "../../enums/search/person-lot-new-filter-type.enum";

export class PersonLotNewFilterDto extends FilterDto {
    filterType = PersonLotNewFilterType.identificationNumber;

    showNotMapped = true;

    uan: string;
    uin: string;
    foreignerNumber: string;

    birthCountryId: number;
    birthCountry: Country;
    birthDate: Date;
}