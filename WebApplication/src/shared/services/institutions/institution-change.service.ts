import { Injectable } from '@angular/core';
import { Institution } from 'src/nomenclatures/models/institution/institution.model';

@Injectable()
export class InstitutionChangeService {

  institutionChange(entity: any, institution: Institution, institutionName: string, subordinateName: string = null, institutionSpecialityName: string = null) {
    if (institution) {
      entity[institutionName] = institution;
      entity[`${institutionName}Id`] = institution.id;
    } else {
      entity[institutionName] = null;
      entity[`${institutionName}Id`] = null;
    }

    if (subordinateName) {
      entity[subordinateName] = null;
      entity[`${subordinateName}Id`] = null;
    }

    if (institutionSpecialityName) {
      entity[institutionSpecialityName] = null;
      entity[`${institutionSpecialityName}Id`] = null;
    }
  }

  subordinateChange(entity: any, subordinate: Institution, subordinateName: string, institutionName: string = null, institutionSpecialityName: string = null) {
    if (subordinate) {
      entity[subordinateName] = subordinate;
      entity[`${subordinateName}Id`] = subordinate.id;

      if (institutionName) {
        entity[institutionName] = subordinate.parent;
        entity[`${institutionName}Id`] = subordinate.parentId;
      }
    } else {
      entity[subordinateName] = null;
      entity[`${subordinateName}Id`] = null;
    }

    if (institutionSpecialityName) {
      entity[institutionSpecialityName] = null;
      entity[`${institutionSpecialityName}Id`] = null;
    }
  }
}
