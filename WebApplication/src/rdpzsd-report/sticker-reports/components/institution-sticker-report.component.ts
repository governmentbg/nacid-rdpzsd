import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import * as saveAs from 'file-saver';
import { Institution } from 'src/nomenclatures/models/institution/institution.model';
import { RdpzsdReportResource } from 'src/rdpzsd-report/resources/rdpzsd-report.resource';
import { StudentStickerStateReport } from 'src/rdpzsd/enums/parts/student-sticker-state.enum';
import { Level } from 'src/shared/enums/level.enum';
import { UserDataService } from 'src/users/services/user-data.service';
import { StickerReportFilterDto } from '../dtos/sticker-report-filter.dto';

@Component({
    selector: 'institution-sticker-report',
    templateUrl: './institution-sticker-report.component.html',
    providers: [RdpzsdReportResource]
})
export class InstitutionStickerReportComponent {

    filter = this.initializeFilter();
    currentYear = new Date().getFullYear();
    studentStickerStateReport = StudentStickerStateReport;

    level = Level;

    constructor(
        public userDataService: UserDataService,
        public translateService: TranslateService,
        private resource: RdpzsdReportResource<StickerReportFilterDto>
    ) {
        resource.init('InstitutionSticker');
    }

    initializeFilter() {
        var newFilter = new StickerReportFilterDto();

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

    exportReportExcel() {
        return this.resource.exportReportExcel(this.filter)
            .subscribe((blob: Blob) => {
                saveAs(blob, 'СтикериВУ.xlsx');
            });
    }

    clear() {
        this.filter = this.initializeFilter();
    }
}
