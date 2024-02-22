import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertMessageDto } from 'src/shared/components/alert-message/models/alert-message.dto';
import { AlertMessageService } from 'src/shared/components/alert-message/services/alert-message.service';
import { UserActivationDto } from 'src/users/dtos/user-activation.dto';
import { UserDataService } from 'src/users/services/user-data.service';
import { UserResource } from 'src/users/user.resource';

@Component({
  selector: 'user-activation',
  templateUrl: './user-activation.component.html',
  styleUrls: ['./user-activation.styles.css']
})
export class UserActivationComponent implements OnInit {

  userActivationDto = new UserActivationDto();

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private userResource: UserResource,
    private alertMessageService: AlertMessageService,
    private userDataService: UserDataService
  ) {
  }

  activate() {
    return this.userResource.activate(this.userActivationDto)
      .subscribe(() => {
        const message = new AlertMessageDto('errorTexts.sucessfulActivation', 'success', null)
        this.alertMessageService.next(message);
        this.router.navigate(['/login']);
      });
  }

  ngOnInit() {
    this.userDataService.logout();
    const code = this.route.snapshot.queryParamMap.get('code');
    this.userActivationDto.code = code;
  }
}
