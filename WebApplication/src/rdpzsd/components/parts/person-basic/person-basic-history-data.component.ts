import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PartState } from 'src/rdpzsd/enums/part-state.enum';
import { GenderType } from 'src/rdpzsd/enums/parts/gender-type.enum';
import { PartInfo } from 'src/rdpzsd/models/parts/base/part-info.model';
import { BasePersonBasic } from 'src/rdpzsd/models/parts/person-basic/base/base-person-basic.model';
import { RdpzsdAttachedFile } from 'src/shared/models/rdpzsd-attached-file.model';

@Component({
    selector: 'person-basic-history-data',
    templateUrl: './person-basic-history-data.component.html'
})
export class PersonBasicHistoryDataComponent<TPart extends BasePersonBasic<TPartInfo, TPassportCopy, TPersonImage>, TPartInfo extends PartInfo, TPassportCopy extends RdpzsdAttachedFile, TPersonImage extends RdpzsdAttachedFile> {

    genderType = GenderType;
    @Input() personBasic: TPart;
    @Input() isCollapsed = true;

    partState = PartState;
    
    constructor(
        private activeModal: NgbActiveModal
    ) {
    }

    close() {
        this.activeModal.close();
    }
}
