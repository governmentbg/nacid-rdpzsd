import { Component, Input } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { NomenclatureFilterDto } from "src/nomenclatures/dtos/nomenclature-filter.dto";
import { StudentEvent } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatus } from "src/nomenclatures/models/student-status/student-status.model";
import { BaseNomenclatureResource } from "src/nomenclatures/resources/base/base-nomenclature.resource";
import { PeriodNomenclatureResource } from "src/nomenclatures/resources/period-nomenclature.resource";
import { PersonDoctoralSemester } from "src/rdpzsd/models/parts/person-doctoral/person-doctoral-semester.model";
import { PersonSemesterResource } from "src/rdpzsd/resources/parts/person-semester.resource";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { StudentStatusChangeService } from "src/shared/services/student-statuses/student-status-change.service";
import { UserDataService } from "src/users/services/user-data.service";
import { BasePersonSemesterComponent } from "../base/base-person-semester.component";
import { FileUploadService } from 'src/shared/services/file-upload.service';
import { Configuration } from "src/app/configuration/configuration";
import { MessageModalComponent } from "src/shared/components/message-modal/message-modal.component";
import { YearType } from "src/rdpzsd/enums/parts/year-type.enum";
import { AttestationType } from "src/rdpzsd/enums/parts/attestation-type.enum";

@Component({
    selector: 'tr[person-doctoral-semester]',
    templateUrl: './person-doctoral-semester.component.html',
    providers: [
        BaseNomenclatureResource,
        PersonSemesterResource
    ]
})
export class PersonDoctoralSemesterComponent extends BasePersonSemesterComponent<PersonDoctoralSemester> {

    yearType = YearType;
    attestationType = AttestationType;
    excludeYearsArray: number[] = [];

    doctoralDuration: number;
    @Input('doctoralDuration')
    set doctoralDurationSetter(doctoralDuration: number){
        this.doctoralDuration = doctoralDuration;
        this.excludeYearsArray = this.constructYearTypeByDuration(this.semester.yearType, doctoralDuration);
    }


    constructor(
        public userDataService: UserDataService,
        public studentStatusChangeService: StudentStatusChangeService,
        protected resource: PersonSemesterResource<PersonDoctoralSemester>,
        protected studentStatusResource: BaseNomenclatureResource<StudentStatus, NomenclatureFilterDto>,
        protected studentEventResource: BaseNomenclatureResource<StudentEvent, NomenclatureFilterDto>,
        public periodNomenclatureResource: PeriodNomenclatureResource,
        protected modalService: NgbModal,
        public currentUserContext: CurrentPersonContextService,
        private fileUploadService: FileUploadService,
        public configuration: Configuration
    ) {
        super(userDataService, studentStatusChangeService, resource, 'PersonDoctoralSemester', modalService, currentUserContext);
    }

    studentEventChanged(studentEvent: StudentEvent) {
        this.semester.relocatedFromPart = null;
        this.semester.relocatedFromPartId = null;
        this.semester.attestationType = null;

        this.studentStatusChangeService.studentEventChange(this.semester, studentEvent, 'studentEvent', 'studentStatus');
    }

    getRelocationFile() {
        const fileUrl = `${this.configuration.restUrl}FileStorage?key=${this.semester.semesterRelocatedFile.key}&fileName=${this.semester.semesterRelocatedFile.name}&dbId=${this.semester.semesterRelocatedFile.dbId}`;

        return this.fileUploadService.getFile(fileUrl, this.semester.semesterRelocatedFile.mimeType)
            .subscribe((blob: Blob) => {
                var url = window.URL.createObjectURL(blob);
                window.open(url);
            });
    }

    save(): any {
        if (this.semester.id) {
            return this.resource
                .update(this.semester)
                .subscribe(updatedSemester => {
                    this.semester = updatedSemester;
                    this.originalModel = null;
                    this.isEditMode = false;
                    this.currentUserContext.setIsInEdit(false);
                    this.updateSemester.emit(this.semester);
                });
        } else {
            const modal = this.modalService.open(MessageModalComponent, { backdrop: 'static', keyboard: false });
            modal.componentInstance.text = 'rdpzsd.personLot.modals.validateProtocolDate';
            modal.componentInstance.acceptButton = 'root.buttons.confirmAndSave';
            modal.componentInstance.acceptButtonClass = 'btn-sm btn-primary';

            modal.result.then((ok) => {
                if (ok) {
                    return this.resource
                        .create(this.semester)
                        .subscribe(newSemester => {
                            this.semester = newSemester;
                            this.originalModel = null;
                            this.isEditMode = false;
                            this.currentUserContext.setIsInEdit(false);
                            this.updateSemester.emit(this.semester);
                        });
                }

                return null;
            });
        }
    }

    private constructYearTypeByDuration(yearType: YearType, duration: number) {
        let excludeYearsArray: number[] = [];

        switch(duration) {
            case 1: {
                excludeYearsArray = [this.yearType.secondYear, this.yearType.thirdYear, this.yearType.fourthYear, this.yearType.fifthYear];
                break;
            }
            case 2: {
                excludeYearsArray = [this.yearType.thirdYear, this.yearType.fourthYear, this.yearType.fifthYear];
                break;
            }
            case 3: {
                excludeYearsArray = [this.yearType.fourthYear, this.yearType.fifthYear];
                break;
            }
            case 4: {
                excludeYearsArray = [this.yearType.fifthYear];
                break;
            }
            default: {
                break;
            }
        }

        switch(yearType) {
            case 2: {
                excludeYearsArray.push(this.yearType.firstYear);
                break;
            }
            case 3: {
                excludeYearsArray.push(this.yearType.firstYear, this.yearType.secondYear);
                break;
            }
            case 4: {
                excludeYearsArray.push(this.yearType.firstYear, this.yearType.secondYear, this.yearType.thirdYear);
                break;
            }
            case 4: {
                excludeYearsArray.push(this.yearType.firstYear, this.yearType.secondYear, this.yearType.thirdYear, this.yearType.fourthYear);
                break;
            }
        }

        return excludeYearsArray;
    }
}