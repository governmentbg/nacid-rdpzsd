import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserDataService } from 'src/users/services/user-data.service';

@Component({
    selector: 'sticker-report-tabs',
    templateUrl: './sticker-report-tabs.component.html'
})
export class StickerReportTabsComponent implements OnInit {
    activeTab = "StudentSticker";

    constructor(
        public userDataService: UserDataService,
        private router: Router
    ) {
    }

    ngOnInit() {
        if (!this.userDataService.userData?.institution?.hasMaster
            && !this.userDataService.userData?.institution?.hasBachelor
            && !this.userDataService.isEmsStickersUser()) {
            this.router.navigate(['/']);
        }
    }
}
