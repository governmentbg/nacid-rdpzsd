import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { UserAuthorizationState } from 'src/users/dtos/login.dtos';
import { UserDataService } from 'src/users/services/user-data.service';

@Injectable()
export class LogoutAuthGuard implements CanActivate {

  userAuthorizationState = UserAuthorizationState

  constructor(
    public router: Router,
    private userDataService: UserDataService
  ) { }

  canActivate(): boolean {
    if (this.userDataService.currentAuthorizationState === this.userAuthorizationState.login) {
      this.router.navigate(['/']);
      return false;
    } else {
      return true;
    }
  }
}
