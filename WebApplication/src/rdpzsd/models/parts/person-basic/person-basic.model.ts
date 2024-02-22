import { BasePersonBasic } from "./base/base-person-basic.model";
import { PassportCopy } from "./passport-copy.model";
import { PersonBasicInfo } from "./person-basic-info.model";
import { PersonImage } from "./person-image.model";

export class PersonBasic extends BasePersonBasic<PersonBasicInfo, PassportCopy, PersonImage> {
}