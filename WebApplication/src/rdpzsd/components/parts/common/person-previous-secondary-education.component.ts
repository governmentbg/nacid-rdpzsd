import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { StudentStickerState } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { PersonSecondary } from "src/rdpzsd/models/parts/person-secondary/person-secondary.model";
import { PersonSecondaryPartResource } from "src/rdpzsd/resources/parts/person-secondary-part.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { PersonSecondaryPartComponent } from "../person-secondary/person-secondary-part.component";

@Component({
    selector: 'person-previous-secondary-education',
    templateUrl: './person-previous-secondary-education.component.html',
    providers: [
        PersonSecondaryPartResource
    ]
})
export class PersonPreviousSecondaryEducationComponent implements OnInit {

    @Input() isEditMode = false;
    @Output() personSecondaryEmitter: EventEmitter<PersonSecondary> = new EventEmitter<PersonSecondary>();

    personSecondary: PersonSecondary = null;
    loadingData = true;
    stickerState = StudentStickerState;

    constructor(
        public currentPersonContextService: CurrentPersonContextService,
        private modalService: NgbModal,
        private personSecondaryPartResource: PersonSecondaryPartResource
    ) {
    }

    showSecondaryPart() {
        const modal = this.modalService.open(PersonSecondaryPartComponent, { backdrop: 'static', keyboard: false, size: 'xl' });
        modal.componentInstance.personLotIdSetter = this.currentPersonContextService.personLotId;
        modal.componentInstance.fromModal = true;
        modal.componentInstance.hideEditActions = !this.isEditMode
            || (this.currentPersonContextService.openedFromStickers
                && this.currentPersonContextService.personStudentStickerDto?.stickerState !== this.stickerState.none
                && this.currentPersonContextService.personStudentStickerDto?.stickerState !== this.stickerState.returnedForEdit);
        modal.result.then((personSecondaryPart: PersonSecondary) => {
            this.personSecondary = personSecondaryPart;
            this.emitPersonSecondary(this.personSecondary);
        });
    }

    ngOnInit() {
        if (this.currentPersonContextService?.personLotId) {
            return this.personSecondaryPartResource.getSinglePart(this.currentPersonContextService.personLotId)
                .subscribe(e => {
                    this.personSecondary = e;
                    this.loadingData = false;
                    this.personSecondaryEmitter.emit(this.personSecondary);
                });
        } else {
            this.loadingData = false;
            this.personSecondaryEmitter.emit(null);
            return this.personSecondary = null;
        }
    }

    emitPersonSecondary(personSecondary: PersonSecondary) {
        this.personSecondaryEmitter.emit(personSecondary);
    }
}