import { Component } from '@angular/core';
import { UserAuthorizationState } from 'src/users/dtos/login.dtos';
import { UserDataService } from 'src/users/services/user-data.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.styles.css']
})
export class HeaderComponent {

  userAuthorizationState = UserAuthorizationState;

  constructor(public userDataService: UserDataService) {
  }
}
