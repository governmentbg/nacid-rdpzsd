import { BasePersonBasic } from "../base/base-person-basic.model";
import { PassportCopyHistory } from "./passport-copy-history.model";
import { PersonBasicHistoryInfo } from "./person-basic-history-info.model";
import { PersonImageHistory } from "./person-image-history.model";

export class PersonBasicHistory extends BasePersonBasic<PersonBasicHistoryInfo, PassportCopyHistory, PersonImageHistory> {
    partId: number;
}