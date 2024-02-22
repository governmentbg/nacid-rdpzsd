import { NgModel } from "@angular/forms";
import { TranslateService } from "@ngx-translate/core";
import { GenderType } from "src/rdpzsd/enums/parts/gender-type.enum";
import { PersonBasic } from "src/rdpzsd/models/parts/person-basic/person-basic.model";
import { UserDataService } from "src/users/services/user-data.service";

export abstract class BasePersonBasicComponent {

  genderType = GenderType;

  constructor(
    public translateService: TranslateService,
    public userDataService: UserDataService
  ) {
  }

  changeUin(uin: string, personBasic: PersonBasic, uinModel: NgModel) {
    if (uinModel.value && uinModel.valid) {
      this.setBirthDateGenderByUin(uin, personBasic);
    } else {
      personBasic.birthDate = null;
      personBasic.gender = null;
    }
  }

  public setBirthDateGenderByUin(uin: string, personBasic: PersonBasic) {
    const arrDate = uin.match(/.{1,2}/g);
    const genderNumber = parseInt(uin.charAt(8)) % 2;

    if (parseInt(arrDate[1]) > 40) {
      arrDate[0] = '20' + arrDate[0];
      arrDate[1] = (parseInt(arrDate[1]) - 40).toString();
    } else if (parseInt(arrDate[1]) > 20) {
      arrDate[0] = '18' + arrDate[0];
      arrDate[1] = (parseInt(arrDate[1]) - 20).toString();
    } else {
      arrDate[0] = '19' + arrDate[0];
    }

    personBasic.birthDate = new Date(parseInt(arrDate[0]), parseInt(arrDate[1]) - 1, parseInt(arrDate[2]), 12, 0, 0);
    if (genderNumber === 0) {
      personBasic.gender = this.genderType.male;
    } else {
      personBasic.gender = this.genderType.female;
    }
  }

}
