import { ChangeDetectorRef, Component } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { Configuration } from "src/app/configuration/configuration";
import { NomenclatureFilterDto } from "src/nomenclatures/dtos/nomenclature-filter.dto";
import { StudentEvent } from "src/nomenclatures/models/student-status/student-event.model";
import { StudentStatus } from "src/nomenclatures/models/student-status/student-status.model";
import { BaseNomenclatureResource } from "src/nomenclatures/resources/base/base-nomenclature.resource";
import { PeriodNomenclatureResource } from "src/nomenclatures/resources/period-nomenclature.resource";
import { PartHistoryDto } from "src/rdpzsd/dtos/parts/part-history.dto";
import { PersonDoctoralHistory } from "src/rdpzsd/models/parts/person-doctoral/history/person-doctoral-history.model";
import { PersonDoctoralInfo } from "src/rdpzsd/models/parts/person-doctoral/person-doctoral-info.model";
import { PersonDoctoralSemester } from "src/rdpzsd/models/parts/person-doctoral/person-doctoral-semester.model";
import { PersonDoctoral } from "src/rdpzsd/models/parts/person-doctoral/person-doctoral.model";
import { CurrentPersonContextService } from "src/rdpzsd/services/current-person-context.service";
import { UserDataService } from "src/users/services/user-data.service";
import { BaseStudentDoctoralPartComponent } from "../base/base-student-doctoral-part.component";
import { PersonDoctoralHistoryComponent } from "./person-doctoral-history.component";

@Component({
    selector: 'person-doctoral-part',
    templateUrl: './person-doctoral-part.component.html'
})
export class PersonDoctoralPartComponent extends BaseStudentDoctoralPartComponent<PersonDoctoral, PersonDoctoralInfo, PersonDoctoralSemester> {

    constructor(
        protected cdr: ChangeDetectorRef,
        public userDataService: UserDataService,
        private modalService: NgbModal,
        public currentPersonContext: CurrentPersonContextService,
        public configuration: Configuration,
        protected studentStatusResource: BaseNomenclatureResource<StudentStatus, NomenclatureFilterDto>,
        protected studentEventResource: BaseNomenclatureResource<StudentEvent, NomenclatureFilterDto>,
        public periodNomenclatureResource: PeriodNomenclatureResource
    ) {
        super(cdr, userDataService, studentStatusResource, studentEventResource, periodNomenclatureResource)
    }

    showHistory(partHistoryDto: PartHistoryDto<PersonDoctoral, PersonDoctoralHistory>) {
        const modal = this.modalService.open(PersonDoctoralHistoryComponent, { backdrop: 'static', size: 'xl' });
        modal.componentInstance.personDoctoralHistoryDto = partHistoryDto;
    }

    nextSemester(studentStatusAlias: string) {
        const newSemester = new PersonDoctoralSemester;
        newSemester.partId = this.part.id;
        newSemester.educationFeeType = this.part.semesters[0].educationFeeType;
        newSemester.educationFeeTypeId = this.part.semesters[0].educationFeeTypeId;
        newSemester.yearType = this.part.semesters[0].yearType;

        if (studentStatusAlias === this.studentStatusActive
            || (studentStatusAlias === this.studentStatusProcessGraduation && this.part.semesters[0].studentStatus?.alias === this.studentStatusActive)) {

            const studentEventAlias = studentStatusAlias === this.studentStatusProcessGraduation
                ? this.studentEventDeductedWithDefense
                : this.part.semesters[0]?.studentStatus?.alias === this.studentStatusActive
                    ? this.studentEventAttestation
                    : this.studentEventNextSemesterAfterBreak;

            return this.getStudentEventAlias(studentEventAlias)
                .subscribe(studentEvent => {
                    newSemester.protocolDate = new Date();
                    newSemester.studentStatus = studentEvent.studentStatus;
                    newSemester.studentStatusId = studentEvent.studentStatus.id;
                    newSemester.studentEvent = studentEvent;
                    newSemester.studentEventId = studentEvent.id;
                    newSemester.lastSemesterStudentStatusAlias = this.part.semesters[0].studentStatus?.alias;
                    this.currentPersonContext.setIsInEdit(true);
                    this.addSemester(newSemester);
                });
        } else {
            return this.getStudentStatusAlias(studentStatusAlias)
                .subscribe(studentStatus => {
                    newSemester.protocolDate = new Date();
                    newSemester.studentStatus = studentStatus;
                    newSemester.studentStatusId = studentStatus.id;
                    newSemester.lastSemesterStudentStatusAlias = this.part.semesters[0].studentStatus?.alias;
                    this.currentPersonContext.setIsInEdit(true);
                    this.addSemester(newSemester);
                });
        }
    }
}