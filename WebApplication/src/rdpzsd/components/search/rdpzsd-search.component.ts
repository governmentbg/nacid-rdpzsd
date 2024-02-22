import { Component } from '@angular/core';
import { UserDataService } from 'src/users/services/user-data.service';

@Component({
    selector: 'rdpzsd-search',
    templateUrl: './rdpzsd-search.component.html'
})
export class RdpzsdSearchComponent {
    activeTab = "Students";

    constructor(public userDataService: UserDataService) {
    }

    ngAfterViewInit() {
        if (this.userDataService.isRsdUser()) {
            if (this.userDataService.userData.institution.hasMaster || this.userDataService.userData.institution.hasBachelor) {
                this.activeTab = "Students"
            } else if (this.userDataService.userData.institution.hasDoctoral) {
                this.activeTab = "Doctorals"
            }
        }
    }
}
