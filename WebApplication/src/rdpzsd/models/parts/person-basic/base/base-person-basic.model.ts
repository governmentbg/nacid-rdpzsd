import { Country } from "src/nomenclatures/models/settlement/country.model";
import { District } from "src/nomenclatures/models/settlement/district.model";
import { Municipality } from "src/nomenclatures/models/settlement/municipality.model";
import { Settlement } from "src/nomenclatures/models/settlement/settlement.model";
import { GenderType } from "src/rdpzsd/enums/parts/gender-type.enum";
import { RdpzsdAttachedFile } from "src/shared/models/rdpzsd-attached-file.model";
import { PartInfo } from "../../base/part-info.model";
import { Part } from "../../base/part.model";

export class BasePersonBasic<TPartInfo extends PartInfo, TPassportCopy extends RdpzsdAttachedFile, TPersonImage extends RdpzsdAttachedFile> extends Part<TPartInfo>
{
    firstName: string;
    middleName: string;
    lastName: string;
    otherNames: string;
    fullName: string;
    firstNameAlt: string
    middleNameAlt: string;
    lastNameAlt: string;
    otherNamesAlt: string;
    fullNameAlt: string;

    uin: string;
    foreignerNumber: string;
    idnNumber: string;

    email: string;
    phoneNumber: string;
    postCode: string;

    gender: GenderType;
    birthDate: Date;

    birthCountryId: number;
    birthCountry: Country;
    birthDistrictId: number;
    birthDistrict: District;
    birthMunicipalityId: number;
    birthMunicipality: Municipality;
    birthSettlementId: number;
    birthSettlement: Settlement;
    foreignerBirthSettlement: string;

    citizenshipId: number;
    citizenship: Country;
    secondCitizenshipId: number;
    secondCitizenship: Country;

    residenceCountryId: number;
    residenceCountry: Country;
    residenceDistrictId: number;
    residenceDistrict: District;
    residenceMunicipalityId: number;
    residenceMunicipality: Municipality;
    residenceSettlementId: number;
    residenceSettlement: Settlement;
    residenceAddress: string;

    passportCopy: TPassportCopy;
    personImage: TPersonImage;
}