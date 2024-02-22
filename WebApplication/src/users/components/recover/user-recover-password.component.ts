import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertMessageDto } from 'src/shared/components/alert-message/models/alert-message.dto';
import { AlertMessageService } from 'src/shared/components/alert-message/services/alert-message.service';
import { UserDataService } from 'src/users/services/user-data.service';
import { UserResource } from 'src/users/user.resource';

@Component({
  selector: 'user-recover-password',
  templateUrl: './user-recover-password.component.html',
  styleUrls: ['./user-recover-password.styles.css']
})
export class UserRecoverPasswordComponent implements OnInit {

  username: string = null;

  constructor(
    private router: Router,
    private userResource: UserResource,
    private alertMessageService: AlertMessageService,
    private userDataService: UserDataService
  ) { }

  recover() {
    return this.userResource.recoverPassword(this.username)
      .subscribe(() => {
        const message = new AlertMessageDto('errorTexts.succesfullRecoverPassword', 'success', null)
        this.alertMessageService.next(message);
        this.router.navigate(['/login']);
      });
  }

  ngOnInit() {
    this.userDataService.logout();
  }
}
