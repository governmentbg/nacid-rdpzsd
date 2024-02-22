import { Injectable } from '@angular/core';
import { Country } from 'src/nomenclatures/models/settlement/country.model';
import { District } from 'src/nomenclatures/models/settlement/district.model';
import { Municipality } from 'src/nomenclatures/models/settlement/municipality.model';
import { Settlement } from 'src/nomenclatures/models/settlement/settlement.model';

@Injectable()
export class SettlementChangeService {

  countryChange(entity: any, country: Country, countryName: string, districtName: string = null, municipalityName: string = null, settlementName: string = null) {
    entity.residenceAddress = null;

    if (country) {
      entity[countryName] = country;
      entity[`${countryName}Id`] = country.id;
    } else {
      entity[countryName] = null;
      entity[`${countryName}Id`] = null;
    }

    if (districtName) {
      entity[districtName] = null;
      entity[`${districtName}Id`] = null;
    }

    if (municipalityName) {
      entity[municipalityName] = null;
      entity[`${municipalityName}Id`] = null;
    }

    if (settlementName) {
      entity[settlementName] = null;
      entity[`${settlementName}Id`] = null;
    }
  }

  districtChange(entity: any, district: District, districtName: string, municipalityName: string = null, settlementName: string = null) {
    if (district) {
      entity[districtName] = district;
      entity[`${districtName}Id`] = district.id;
    } else {
      entity[districtName] = null;
      entity[`${districtName}Id`] = null;
    }

    if (municipalityName) {
      entity[municipalityName] = null;
      entity[`${municipalityName}Id`] = null;
    }

    if (settlementName) {
      entity[settlementName] = null;
      entity[`${settlementName}Id`] = null;
    }
  }

  municipalityChange(entity: any, municipality: Municipality, municipalityName: string, districtName: string = null, settlementName: string = null) {
    if (municipality) {
      entity[municipalityName] = municipality;
      entity[`${municipalityName}Id`] = municipality.id;

      if (districtName) {
        entity[districtName] = municipality.district;
        entity[`${districtName}Id`] = municipality.districtId;
      }
    } else {
      entity[municipalityName] = null;
      entity[`${municipalityName}Id`] = null;
    }

    if (settlementName) {
      entity[settlementName] = null;
      entity[`${settlementName}Id`] = null;
    }
  }

  settlementChange(entity: any, settlement: Settlement, settlementName: string, districtName: string = null, municipalityName: string = null) {
    if (settlement) {
      if (municipalityName) {
        entity[municipalityName] = settlement.municipality;
        entity[`${municipalityName}Id`] = settlement.municipalityId;
      }

      if (districtName) {
        entity[districtName] = settlement.district;
        entity[`${districtName}Id`] = settlement.districtId;
      }

      entity[settlementName] = settlement;
      entity[`${settlementName}Id`] = settlement.id;
    } else {
      entity[settlementName] = null;
      entity[`${settlementName}Id`] = null;
    }
  }
}
