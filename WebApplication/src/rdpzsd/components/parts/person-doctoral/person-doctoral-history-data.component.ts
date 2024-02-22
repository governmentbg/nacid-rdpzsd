import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { StudentEventAttestation } from 'src/nomenclatures/models/student-status/student-event.model';
import { PartState } from 'src/rdpzsd/enums/part-state.enum';
import { AttestationType } from 'src/rdpzsd/enums/parts/attestation-type.enum';
import { YearType } from 'src/rdpzsd/enums/parts/year-type.enum';
import { PartInfo } from 'src/rdpzsd/models/parts/base/part-info.model';
import { BasePersonDoctoralSemester } from 'src/rdpzsd/models/parts/person-doctoral/person-doctoral-semester.model';
import { BasePersonDoctoral } from 'src/rdpzsd/models/parts/person-doctoral/person-doctoral.model';
import { FileUploadService } from 'src/shared/services/file-upload.service';

@Component({
    selector: 'person-doctoral-history-data',
    templateUrl: './person-doctoral-history-data.component.html'
})
export class PersonDoctoralHistoryDataComponent<TPart extends BasePersonDoctoral<TPartInfo, TSemester>, TPartInfo extends PartInfo, TSemester extends BasePersonDoctoralSemester> {

    @Input() personDoctoral: TPart;
    @Input() isCollapsed = true;

    partState = PartState;
    yearType = YearType;
    attestationType = AttestationType;

    studentEventAttestation = StudentEventAttestation;

    constructor(
        private activeModal: NgbActiveModal,
        public translateService: TranslateService,
        private fileUploadService: FileUploadService
    ) {
    }

    sortSemesters() {
        return this.personDoctoral?.semesters?.sort((a, b) => a.protocolDate > b.protocolDate ? -1 : 1);
    }

    close() {
        this.activeModal.close();
    }
    getRelocationFile(semester: TSemester) {
        return this.fileUploadService.getFileToAnchorUrl(semester.semesterRelocatedFile);
    }
}
