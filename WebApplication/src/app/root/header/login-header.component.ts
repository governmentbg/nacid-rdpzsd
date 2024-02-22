import { Component, ViewChild } from '@angular/core';
import { RouterLinkActive } from '@angular/router';
import { Configuration } from 'src/app/configuration/configuration';
import { UserDataService } from 'src/users/services/user-data.service';

@Component({
  selector: 'login-header',
  templateUrl: './login-header.component.html',
  styleUrls: ['./login-header.styles.css']
})
export class LoginHeaderComponent {

  @ViewChild('stickerReportsLinkRef') stickerReportsLinkRef: RouterLinkActive = null;

  constructor(
    public userDataService: UserDataService,
    public configuration: Configuration
  ) {
  }
}
