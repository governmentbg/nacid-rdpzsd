import { Country } from "../settlement/country.model";

export class AdmissionReasonCitizenship{
    admissionReasonId: number;
    euCountry: boolean;
    eeaCountry: boolean;
    excludeCountry: boolean;

    countryId: number;
    country: Country;
}