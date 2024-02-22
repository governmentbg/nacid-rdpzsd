import { EducationFeeType } from "src/nomenclatures/models/others/education-fee-type.model";
import { StudentEvent } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatus } from "src/nomenclatures/models/student-status/student-status.model";
import { EntityVersion } from "src/shared/models/entity-version.model";
import { RdpzsdAttachedFile } from './../../../../shared/models/rdpzsd-attached-file.model';

export class BasePersonSemester extends EntityVersion {
    partId: number;
    studentStatusId: number;
    studentStatus: StudentStatus;
    studentEventId: number;
    studentEvent: StudentEvent;
    educationFeeTypeId: number;
    educationFeeType: EducationFeeType;
    note: string;
    hasScholarship: boolean;
    useHostel: boolean;
    useHolidayBase: boolean;
    participatedIntPrograms: boolean;
    relocatedFromPartId: number;
    relocatedFromPart: any;
    semesterRelocatedNumber: string;
    semesterRelocatedDate: Date;
    semesterRelocatedFile: RdpzsdAttachedFile;
    // Client only
    lastSemesterStudentStatusAlias: string;
}