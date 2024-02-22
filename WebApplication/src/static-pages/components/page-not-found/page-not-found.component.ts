import { Component } from '@angular/core';
import { UserAuthorizationState } from 'src/users/dtos/login.dtos';
import { UserDataService } from 'src/users/services/user-data.service';

@Component({
  selector: 'page-not-found',
  templateUrl: './page-not-found.component.html',
  styleUrls: ['./page-not-found.styles.css']
})
export class PageNotFoundComponent {

  userAuthorizationState = UserAuthorizationState;

  constructor(public userDataService: UserDataService) {
  }
}
