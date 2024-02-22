import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { StickerReportTabsComponent } from "./sticker-reports/components/sticker-report-tabs.component";

const routes: Routes = [
    {
        path: 'stickerReports',
        component: StickerReportTabsComponent,
        data: {
            title: 'Справка стикери'
        }
    }
]

@NgModule({
    imports: [CommonModule, RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RdpzsdReportRoutingModule { }