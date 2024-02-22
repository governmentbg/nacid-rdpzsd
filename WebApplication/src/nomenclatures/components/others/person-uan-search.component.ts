import { Component, OnInit } from '@angular/core';
import { UserDataService } from 'src/users/services/user-data.service';

@Component({
  selector: 'person-uan-search',
  templateUrl: './person-uan-search.component.html',
})
export class PersonUanSearchComponent {
  activeTab = "Students";

  constructor(public userDataService: UserDataService) {
  }
}
