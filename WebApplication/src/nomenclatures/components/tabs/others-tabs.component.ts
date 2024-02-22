import { Component } from '@angular/core';
import { UserDataService } from 'src/users/services/user-data.service';

@Component({
  selector: 'others-tabs',
  templateUrl: './others-tabs.component.html'
})
export class OthersTabsComponent {
  activeTab = "School";

  constructor(public userDataService: UserDataService) {
  }
}
