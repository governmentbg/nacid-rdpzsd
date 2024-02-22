import { StudentProtocolType } from "src/rdpzsd/enums/parts/student-protocol-type.enum";
import { EntityVersion } from "src/shared/models/entity-version.model";

export class BasePersonStudentProtocol extends EntityVersion {
    partId: number;
    protocolType: StudentProtocolType;
    protocolNumber: string;
    protocolDate: Date;
}