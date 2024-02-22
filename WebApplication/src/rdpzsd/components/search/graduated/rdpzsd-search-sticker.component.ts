import { Component, HostListener } from "@angular/core";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { TranslateService } from "@ngx-translate/core";
import { catchError, Observable, Observer, throwError } from "rxjs";
import { Doctor } from "src/nomenclatures/models/institution/educational-qualification.model";
import { InstitutionSpeciality } from "src/nomenclatures/models/institution/institution-speciality.model";
import { Institution } from "src/nomenclatures/models/institution/institution.model";
import { StudentEventGraduatedWithDiploma, StudentEventGraduatedWithoutDiploma } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatusProcessGraduation } from "src/nomenclatures/models/student-status/student-status.model";
import { StickerDto } from "src/rdpzsd/dtos/parts/person-student-sticker/sticker.dto";
import { PersonStudentStickerFilterDto } from "src/rdpzsd/dtos/search/person-student-sticker/person-student-sticker-filter.dto";
import { PersonStudentStickerSearchDto } from "src/rdpzsd/dtos/search/person-student-sticker/person-student-sticker-search.dto";
import { StudentStickerState, StudentStickerStateMon, StudentStickerStateRsd } from "src/rdpzsd/enums/parts/student-sticker-state.enum";
import { PersonStudentStickerNote } from "src/rdpzsd/models/parts/person-student/person-student-sticker-note.model";
import { PersonStudentStickerResource } from "src/rdpzsd/resources/parts/person-student-graduation/person-student-sticker.resource";
import { PersonStudentStickerSearchResource } from "src/rdpzsd/resources/search/person-student-sticker-search.resource";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { MigrationNoEmail, MigrationNoPhoneNumber } from "src/shared/constraints/global-constraints";
import { SearchResultDto } from "src/shared/dtos/search-result.dto";
import { Level } from "src/shared/enums/level.enum";
import { UserDataService } from "src/users/services/user-data.service";
import { PersonStudentStickerNotesModalComponent } from "../../parts/person-student/graduation/stickers/person-student-sticker-notes.modal";
import { PersonStudentStickerModalComponent } from "../../parts/person-student/graduation/stickers/person-student-sticker.modal";

@Component({
    selector: 'rdpzsd-search-sticker',
    templateUrl: './rdpzsd-search-sticker.component.html'
})
export class RdpzsdSearchStickerComponent {

    studentStatusProcessGraduation = StudentStatusProcessGraduation;
    studentEventGraduatedWithoutDiploma = StudentEventGraduatedWithoutDiploma;
    studentEventGraduatedWithDiploma = StudentEventGraduatedWithDiploma;

    migrationNoPhoneNumber = MigrationNoPhoneNumber;
    migrationNoEmail = MigrationNoEmail;

    level = Level;
    studentStickerState = StudentStickerState;
    studentStickerStateMon = StudentStickerStateMon;
    studentStickerStateRsd = StudentStickerStateRsd;
    filter: PersonStudentStickerFilterDto = this.initializeFilter();
    searchResult: SearchResultDto<PersonStudentStickerSearchDto> = new SearchResultDto<PersonStudentStickerSearchDto>();
    loadingData = false;
    loadingDataCount = false;
    dataCount: number = null;
    doctor = Doctor;

    markedEnabled = false;

    @HostListener('document:keydown.enter', ['$event'])
    handleKeyboardEvent() {
        this.getData(false, false);
    }

    constructor(
        private resource: PersonStudentStickerSearchResource,
        private router: Router,
        public userDataService: UserDataService,
        public translateService: TranslateService,
        private modalService: NgbModal,
        private stickerResource: PersonStudentStickerResource
    ) {
    }

    initializeFilter() {
        var newFilter = new PersonStudentStickerFilterDto();

        if (this.userDataService.isRsdUser()) {
            newFilter.institution = new Institution();
            newFilter.institutionId = this.userDataService.userData.institution.id;
            newFilter.institution.name = this.userDataService.userData.institution.name;
            newFilter.institution.nameAlt = this.userDataService.userData.institution.nameAlt;
            newFilter.institution.shortName = this.userDataService.userData.institution.shortName;
            newFilter.institution.shortNameAlt = this.userDataService.userData.institution.shortNameAlt;
            newFilter.institution.organizationType = this.userDataService.userData.institution.organizationType;
        }

        return newFilter;
    }

    openStickerNotesModal(stickerNotes: PersonStudentStickerNote[]) {
        const modal = this.modalService.open(PersonStudentStickerNotesModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.stickerNotes = stickerNotes;
    }

    returnForEditModal(partId: number, index: number) {
        const modal = this.modalService.open(PersonStudentStickerModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
        modal.componentInstance.title = 'rdpzsd.personStudent.stickers.modalReturnForEditTitle';
        modal.componentInstance.requiredNote = true;
        modal.componentInstance.requiredYear = false;
        modal.componentInstance.showYear = false;

        modal.result.then((stickerDto: StickerDto) => {
            if (stickerDto) {
                this.stickerResource
                    .returnForEdit(partId, stickerDto)
                    .subscribe(() => {
                        this.searchResult.result.splice(index, 1);
                    });
            } else {
                return null;
            }
        });
    }

    startMarking() {
        this.markedEnabled = true;
        this.searchResult?.result?.forEach(e => e.isMarked = false);
    }

    cancelMarking() {
        this.markedEnabled = false;
        this.searchResult?.result?.forEach(e => e.isMarked = false);
    }

    markedRecieved() {
        return this.markedRecievedModal()
            .subscribe(reloadData => {
                if (reloadData) {
                    this.getData(false, true);
                }
            });
    }

    markedForPrint() {
        return this.markedForPrintModal()
            .subscribe(reloadData => {
                if (reloadData) {
                    this.getData(false, true);
                }
            });
    }

    markedRecievedModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            modal.componentInstance.text = 'rdpzsd.personStudent.stickers.modalMarkedRecieved';

            modal.result.then((ok) => {
                if (ok) {
                    const markedRecievedIds = this.searchResult.result?.filter(e => e.isMarked
                        && e.stickerState === this.studentStickerState.stickerForPrint)
                        .map(e => e.partId);

                    this.stickerResource
                        .markedRecieved(markedRecievedIds)
                        .pipe(
                            catchError(() => {
                                observer.complete();
                                return throwError(() => new Error('Marked recieved stickers error'));
                            })
                        )
                        .subscribe(() => {
                            observer.next(true);
                            observer.complete();
                        });
                } else {
                    observer.complete();
                }
            });
        });
    }

    markedForPrintModal() {
        return new Observable((observer: Observer<any>) => {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', size: 'lg', keyboard: false });
            modal.componentInstance.text = 'rdpzsd.personStudent.stickers.modalMarkedForPrint';

            modal.result.then((ok) => {
                if (ok) {
                    const markedForPrintIds = this.searchResult.result?.filter(e => e.isMarked
                        && (e.stickerState === this.studentStickerState.sendForSticker || e.stickerState === this.studentStickerState.reissueSticker))
                        .map(e => e.partId);

                    this.stickerResource
                        .markedForPrint(markedForPrintIds)
                        .pipe(
                            catchError(() => {
                                observer.complete();
                                return throwError(() => new Error('Marked for print stickers error'));
                            })
                        )
                        .subscribe(() => {
                            observer.next(true);
                            observer.complete();
                        });
                } else {
                    observer.complete();
                }
            });
        });
    }

    forPrintModal(partId: number, index: number) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
        modal.componentInstance.text = 'rdpzsd.personStudent.stickers.confirmForPrintModal';
        modal.componentInstance.acceptButton = 'root.buttons.yesSure'

        modal.result.then((ok) => {
            if (ok) {
                return this.stickerResource
                    .forPrint(partId)
                    .subscribe(personStudent => {
                        this.searchResult.result[index] = personStudent;
                    });
            } else {
                return null;
            }
        });
    }

    forPrintDuplicateModal(duplicateDiplomaId: number, index: number) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
        modal.componentInstance.text = 'rdpzsd.personStudent.stickers.confirmForPrintModal';

        modal.result.then((ok) => {
            if (ok) {
                return this.stickerResource
                    .forPrintDuplicate(duplicateDiplomaId)
                    .subscribe(studentStickerState => {
                        this.searchResult.result[index].stickerState = studentStickerState;
                    });
            } else {
                return null;
            }
        });
    }

    recievedModal(partId: number, index: number) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
        modal.componentInstance.text = 'rdpzsd.personStudent.stickers.confirmRecievedModal';
        modal.componentInstance.acceptButton = 'root.buttons.yesSure'

        modal.result.then((ok) => {
            if (ok) {
                return this.stickerResource
                    .recieved(partId)
                    .subscribe(() => {
                        this.searchResult.result.splice(index, 1);
                    });
            } else {
                return null;
            }
        });
    }

    recievedDuplicateModal(duplicateDiplomaId: number, index: number) {
        const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
        modal.componentInstance.text = 'rdpzsd.personStudent.stickers.confirmRecievedModal';
        modal.componentInstance.acceptButton = 'root.buttons.yesSure'

        modal.result.then((ok) => {
            if (ok) {
                return this.stickerResource
                    .recievedDuplicate(duplicateDiplomaId)
                    .subscribe(() => {
                        this.searchResult.result.splice(index, 1);
                    });
            } else {
                return null;
            }
        });
    }

    clear() {
        this.filter = this.initializeFilter();
        return this.getData(false, false);
    }

    getData(loadMore: boolean, triggerLoadingDataIndicator = true) {
        if (triggerLoadingDataIndicator) {
            this.loadingData = true;
        }

        if (loadMore) {
            this.filter.limit = this.filter.limit + this.searchResult.result.length;
        } else {
            this.filter.limit = 15;
        }

        this.dataCount = null;
        this.cancelMarking();

        return this.resource
            .getAll(this.filter)
            .subscribe(e => {
                this.searchResult = e;
                this.loadingData = false;
            });
    }

    getDataCount() {
        this.loadingDataCount = true;

        return this.resource
            .getAllCount(this.filter)
            .subscribe(e => {
                this.dataCount = e;
                this.loadingDataCount = false;
            });
    }

    institutionSpecialityChanged(institutionSpeciality: InstitutionSpeciality) {
        this.filter.institutionSpeciality = institutionSpeciality;
        this.filter.institutionSpecialityId = institutionSpeciality?.id;
        this.filter.educationalForm = institutionSpeciality?.educationalForm;
        this.filter.educationalFormId = institutionSpeciality?.educationalFormId;
        this.filter.educationalQualification = institutionSpeciality?.speciality?.educationalQualification;
        this.filter.educationalQualificationId = institutionSpeciality?.speciality?.educationalQualificationId;
    }

    hasSendForStickerOrReissue(checkMarked: boolean) {
        return this.searchResult?.result?.some(e =>
            (e.stickerState === this.studentStickerState.sendForSticker || e.stickerState === this.studentStickerState.reissueSticker)
            && (!checkMarked || e.isMarked)
        );
    }

    hasForPrint(checkMarked: boolean) {
        return this.searchResult?.result?.some(e => e.stickerState === this.studentStickerState.stickerForPrint
            && (!checkMarked || e.isMarked)
        );
    }

    markUnmarkAll(event: any) {
        if (this.filter?.stickerState === this.studentStickerState.stickerForPrint) {
            this.searchResult?.result?.forEach(e => {
                if (e.stickerState === this.studentStickerState.stickerForPrint) {
                    e.isMarked = event.target.checked;
                }
            });
        } else {
            this.searchResult?.result?.forEach(e => {
                if (e.stickerState === this.studentStickerState.sendForSticker || e.stickerState === this.studentStickerState.reissueSticker) {
                    e.isMarked = event.target.checked;
                }
            });
        }
    }

    getStudentLot(personStudent: PersonStudentStickerSearchDto) {
        this.router.navigate(['/personLot', personStudent.uan, 'Student'], {
            state: { personStudentStickerDto: personStudent },
            queryParams: { fromStickers: true }
        });
    }

    ngOnInit() {
        if (!this.userDataService.isEmsStickersUser()
            && !this.userDataService.userData?.institution?.hasMaster
            && !this.userDataService.userData?.institution?.hasBachelor) {
            this.router.navigate(['/rdpzsd']);
        }

        return this.getData(false, true);
    }
}