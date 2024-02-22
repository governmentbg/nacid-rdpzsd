import { EntityVersion } from "src/shared/models/entity-version.model";
import { AdmissionReasonEducationFeeHistory } from "./admission-reason-education-fee-history.model";
import { AdmissionReasonEducationFee } from "./admission-reason-education-fee.model";

export class AdmissionReasonHistory extends EntityVersion{
    admissionReasonId: number;
    userFullName: string;
    changeDate: Date;
    name: string;
    nameAlt: string;
    shortName: string;
    shortNameAlt: string;
    description: string;
    isActive: boolean;

    admissionReasonEducationFeeHistories: AdmissionReasonEducationFeeHistory[] = [];
}