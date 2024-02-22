import { Component } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { UserDataService } from 'src/users/services/user-data.service';
import { LoginDto, LoginError, TokenRequestResponse } from '../../dtos/login.dtos';
import { UserResource } from '../../user.resource';

@Component({
  selector: 'login-component',
  templateUrl: './login.component.html',
  styleUrls: ['./login.styles.css']
})
export class LoginComponent {

  loginError = LoginError;
  loginDto = new LoginDto();
  currentLoginError = LoginError.none;

  constructor(
    private userResource: UserResource,
    private userDataService: UserDataService
  ) {
  }

  login() {
    return this.userResource.login(this.loginDto)
      .pipe(
        catchError(() => {
          this.currentLoginError = this.loginError.wrongCredentials;
          return throwError(() => new Error('Error'));
        })
      )
      .subscribe((res: TokenRequestResponse) => { this.userDataService.login(res, this.loginDto.rememberMe) });
  }
}
