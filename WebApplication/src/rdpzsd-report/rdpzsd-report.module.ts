import { NgModule } from "@angular/core";
import { SharedModule } from "src/shared/shared.module";
import { RdpzsdReportRoutingModule } from "./rdpzsd-report.routing";
import { InstitutionStickerReportComponent } from "./sticker-reports/components/institution-sticker-report.component";
import { StickerReportTabsComponent } from "./sticker-reports/components/sticker-report-tabs.component";
import { StudentStickerReportComponent } from "./sticker-reports/components/student-sticker-report.component";

@NgModule({
    declarations: [
        StickerReportTabsComponent,
        StudentStickerReportComponent,
        InstitutionStickerReportComponent
    ],
    imports: [
        RdpzsdReportRoutingModule,
        SharedModule
    ],
    providers: []
})
export class RdpzsdReportModule { }
