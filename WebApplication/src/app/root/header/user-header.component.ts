import { Component } from '@angular/core';
import { UserDataService } from 'src/users/services/user-data.service';

@Component({
    selector: 'user-header',
    templateUrl: './user-header.component.html'
})
export class UserHeaderComponent {

    constructor(public userDataService: UserDataService) {
    }

    toggleUserSidebar() {
        this.userDataService
            .toggleSidebarSubject
            .next();
    }
}
