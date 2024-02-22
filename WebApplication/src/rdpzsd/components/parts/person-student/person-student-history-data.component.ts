import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { StudentEventIndividualPlanTwoSemesters, StudentEventIndividualPlanTwoYears } from 'src/nomenclatures/models/student-status/student-event.model';
import { StudentStatusGraduated } from 'src/nomenclatures/models/student-status/student-status.model';
import { PartState } from 'src/rdpzsd/enums/part-state.enum';
import { CourseType } from 'src/rdpzsd/enums/parts/course.enum';
import { StudentProtocolType } from 'src/rdpzsd/enums/parts/student-protocol-type.enum';
import { PartInfo } from 'src/rdpzsd/models/parts/base/part-info.model';
import { BasePersonStudentDiploma } from 'src/rdpzsd/models/parts/person-student/base/base-person-student-diploma.model';
import { BasePersonStudentDuplicateDiploma } from 'src/rdpzsd/models/parts/person-student/base/base-person-student-duplicate-diploma.model';
import { BasePersonStudentProtocol } from 'src/rdpzsd/models/parts/person-student/base/base-person-student-protocol.model';
import { BasePersonStudentSemester } from 'src/rdpzsd/models/parts/person-student/person-student-semester.model';
import { BasePersonStudent } from 'src/rdpzsd/models/parts/person-student/person-student.model';
import { Semester } from 'src/shared/enums/semester.enum';
import { RdpzsdAttachedFile } from 'src/shared/models/rdpzsd-attached-file.model';
import { FileUploadService } from 'src/shared/services/file-upload.service';

@Component({
    selector: 'person-student-history-data',
    templateUrl: './person-student-history-data.component.html'
})
export class PersonStudentHistoryDataComponent<TPart extends BasePersonStudent<TPartInfo, TSemester, TDiploma, TDiplomaFile, TDuplicateDiploma, TDuplicateDiplomaFile, TProtocol>, TPartInfo extends PartInfo, TSemester extends BasePersonStudentSemester, TDiploma extends BasePersonStudentDiploma<TDiplomaFile>, TDiplomaFile extends RdpzsdAttachedFile, TDuplicateDiploma extends BasePersonStudentDuplicateDiploma<TDuplicateDiplomaFile>, TDuplicateDiplomaFile extends RdpzsdAttachedFile, TProtocol extends BasePersonStudentProtocol> {

    @Input() personStudent: TPart;
    @Input() isCollapsed = true;

    partState = PartState;
    courseType = CourseType;
    semesterType = Semester;
    studentProtocolType = StudentProtocolType;

    studentStatusGraduated = StudentStatusGraduated;

    studentEventIndividualPlanTwoYears = StudentEventIndividualPlanTwoYears;
    studentEventIndividualPlanTwoSemesters = StudentEventIndividualPlanTwoSemesters;

    constructor(
        private activeModal: NgbActiveModal,
        public translateService: TranslateService,
        private fileUploadService: FileUploadService
    ) {
    }

    sortSemesters() {
        return this.personStudent?.semesters?.sort((a, b) => (a.periodId > b.periodId) || (a.periodId == b.periodId && a.studentStatusId > b.studentStatusId) ? -1 : 1);
    }

    sortDuplicateDiplomas() {
        return this.personStudent?.duplicateDiplomas?.sort((a, b) => (a.duplicateDiplomaDate < b.duplicateDiplomaDate) ? -1 : 1);
    }

    close() {
        this.activeModal.close();
    }

    getRelocationFile(semester: TSemester) {
        return this.fileUploadService.getFileToAnchorUrl(semester.semesterRelocatedFile);
    }
}
