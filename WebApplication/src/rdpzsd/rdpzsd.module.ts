import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { RdpzsdRoutingModule } from "./rdpzsd.routing";
import { RdpzsdSearchComponent } from "./components/search/rdpzsd-search.component";
import { RdpzsdSearchNewComponent } from "./components/search/rdpzsd-search-new.component";
import { PersonLotSearchResource } from "./resources/search/person-lot-search.resource";
import { PersonLotResource } from "./resources/person-lot.resource";
import { PersonLotNewComponent } from "./components/lot/new/person-lot-new.component";
import { PersonLotComponent } from "./components/lot/person-lot.component";
import { PersonBasicPartComponent } from "./components/parts/person-basic/person-basic-part.component";
import { PersonBasicComponent } from "./components/parts/person-basic/person-basic.component";
import { PersonBasicAdditionalTabsComponent } from "./components/parts/person-basic/person-basic-additional-tabs.component";
import { PersonLotInfoComponent } from "./components/lot/person-lot-info.component";
import { PartActionsComponent } from "./components/parts/part-actions/part-actions.component";
import { PersonLotActionsComponent } from "./components/lot/person-lot-actions.component";
import { PersonBasicHistoryComponent } from "./components/parts/person-basic/person-basic-history.component";
import { PersonBasicHistoryDataComponent } from "./components/parts/person-basic/person-basic-history-data.component";
import { LotActionsComponent } from "./components/lot/lot-actions/lot-actions.component";
import { RdpzsdSearchApprovalComponent } from "./components/search/rdpzsd-search-approval.component";
import { PersonLotCancelApprovalComponent } from "./components/lot/person-lot-cancel-approval.component";
import { RdpzsdSearchDoctoralsComponent } from "./components/search/rdpzsd-search-doctorals.component";
import { RdpzsdSearchStudentsComponent } from "./components/search/rdpzsd-search-students.component";
import { PersonStudentPartsComponent } from "./components/parts/person-student/person-student-parts.component";
import { PersonDoctoralPartsComponent } from "./components/parts/person-doctoral/person-doctoral-parts.component";
import { PersonDoctoralPartComponent } from "./components/parts/person-doctoral/person-doctoral-part.component";
import { PersonStudentPartComponent } from "./components/parts/person-student/person-student-part.component";
import { PersonSemesterSubheaderComponent } from "./components/parts/common/person-semester-subheader.component";
import { PersonStudentComponent } from "./components/parts/person-student/person-student.component";
import { PersonDoctoralComponent } from "./components/parts/person-doctoral/person-doctoral.component";
import { PersonSecondaryPartComponent } from "./components/parts/person-secondary/person-secondary-part.component";
import { PersonSecondaryComponent } from "./components/parts/person-secondary/person-secondary.component";
import { PersonSecondaryHistoryComponent } from "./components/parts/person-secondary/person-secondary-history.component";
import { PersonSecondaryHistoryDataComponent } from './components/parts/person-secondary/person-secondary-history-data.component';
import { PersonPreviousEducationComponent } from "./components/parts/common/person-previous-education.component";
import { PersonPreviousSecondaryEducationComponent } from "./components/parts/common/person-previous-secondary-education.component";
import { PersonStudentSemesterComponent } from "./components/parts/person-student/person-student-semester.component";
import { PersonStudentHistoryComponent } from "./components/parts/person-student/person-student-history.component";
import { PersonStudentHistoryDataComponent } from "./components/parts/person-student/person-student-history-data.component";
import { PersonDoctoralSemesterComponent } from "./components/parts/person-doctoral/person-doctoral-semester.component";
import { PersonDoctoralHistoryDataComponent } from "./components/parts/person-doctoral/person-doctoral-history-data.component";
import { PersonDoctoralHistoryComponent } from "./components/parts/person-doctoral/person-doctoral-history.component";
import { RdpzsdSearchGraduatedComponent } from "./components/search/graduated/rdpzsd-search-graduated.component";
import { PersonStudentProtocolComponent } from "./components/parts/person-student/graduation/person-student-protocol.component";
import { PersonStudentStickerSearchResource } from "./resources/search/person-student-sticker-search.resource";
import { RdpzsdSearchStickerComponent } from "./components/search/graduated/rdpzsd-search-sticker.component";
import { PersonStudentStickerValidationInfoComponent } from "./components/parts/person-student/graduation/stickers/person-student-sticker-validation-info.component";
import { PersonStudentStickerResource } from "./resources/parts/person-student-graduation/person-student-sticker.resource";
import { PersonStudentStickerErrorModalComponent } from "./components/parts/person-student/graduation/stickers/person-student-sticker-error.modal";
import { PersonStudentStickerModalComponent } from "./components/parts/person-student/graduation/stickers/person-student-sticker.modal";
import { PersonStudentStickerNotesModalComponent } from "./components/parts/person-student/graduation/stickers/person-student-sticker-notes.modal";
import { PersonStudentStickerActionsComponent } from "./components/parts/person-student/graduation/stickers/person-student-sticker-actions.component";
import { PersonStudentDiplomaComponent } from "./components/parts/person-student/graduation/person-student-diploma.component";
import { PersonStudentDuplicateDiplomaComponent } from "./components/parts/person-student/graduation/person-student-duplicate-diploma.component";

@NgModule({
    declarations: [
        RdpzsdSearchComponent,
        RdpzsdSearchStudentsComponent,
        RdpzsdSearchDoctoralsComponent,
        RdpzsdSearchNewComponent,
        RdpzsdSearchApprovalComponent,
        RdpzsdSearchGraduatedComponent,
        RdpzsdSearchStickerComponent,
        PersonLotNewComponent,
        PersonLotComponent,
        PersonLotInfoComponent,
        PersonLotCancelApprovalComponent,
        LotActionsComponent,
        PersonLotActionsComponent,
        PersonBasicPartComponent,
        PersonBasicComponent,
        PersonBasicAdditionalTabsComponent,
        PersonBasicHistoryComponent,
        PersonBasicHistoryDataComponent,
        PartActionsComponent,
        PersonStudentPartsComponent,
        PersonDoctoralPartsComponent,
        PersonStudentPartComponent,
        PersonDoctoralPartComponent,
        PersonStudentComponent,
        PersonDoctoralComponent,
        PersonSemesterSubheaderComponent,
        PersonSemesterSubheaderComponent,
        PersonSecondaryPartComponent,
        PersonSecondaryComponent,
        PersonSecondaryHistoryComponent,
        PersonSecondaryHistoryDataComponent,
        PersonPreviousEducationComponent,
        PersonPreviousSecondaryEducationComponent,
        PersonStudentSemesterComponent,
        PersonStudentHistoryComponent,
        PersonStudentHistoryDataComponent,
        PersonDoctoralSemesterComponent,
        PersonDoctoralHistoryComponent,
        PersonDoctoralHistoryDataComponent,
        PersonStudentProtocolComponent,
        PersonStudentStickerValidationInfoComponent,
        PersonStudentStickerErrorModalComponent,
        PersonStudentStickerModalComponent,
        PersonStudentStickerNotesModalComponent,
        PersonStudentStickerActionsComponent,
        PersonStudentDiplomaComponent,
        PersonStudentDuplicateDiplomaComponent
    ],
    imports: [
        RdpzsdRoutingModule,
        SharedModule
    ],
    providers: [
        PersonLotResource,
        PersonLotSearchResource,
        PersonStudentStickerSearchResource,
        PersonStudentStickerResource
    ]
})
export class RdpzsdModule { }
