import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, Observer, Subject, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { accessToken, TokenRequestResponse, UserAuthorizationState } from "../dtos/login.dtos";
import { pathsWithoutTokenRedirect } from "../dtos/paths-without-token-redirect.dto";
import { UserDto, UserType } from "../dtos/user.dto";
import { UserResource } from "../user.resource";

export const emsUser = 'emsUser';
export const rsdUser = 'rsdUser';
export const emsEdit = 'registerRsd#edit';
export const emsStickers = 'registerRsd#stickers';

@Injectable()
export class UserDataService {

  userData: UserDto = new UserDto();
  currentAuthorizationState: UserAuthorizationState = UserAuthorizationState.loading;

  toggleSidebarSubject: Subject<void> = new Subject<void>();

  constructor(
    private router: Router,
    private userResource: UserResource
  ) {
  }

  isEmsUser() {
    return this.userData && this.userData.userType && this.userData.userType == UserType.ems;
  }

  isEmsEditUser() {
    return this.userData && this.userData.userType && this.userData.userType == UserType.ems && this.userData.permissions.indexOf(emsEdit) != -1;
  }

  isEmsStickersUser() {
    return this.userData && this.userData.userType && this.userData.userType == UserType.ems && this.userData.permissions.indexOf(emsStickers) != -1;
  }

  isRsdUser() {
    return this.userData && this.userData.userType && this.userData.userType == UserType.rsd;
  }

  login(data: TokenRequestResponse, rememberMe: boolean) {
    if (rememberMe) {
      localStorage.setItem(accessToken, data.access_token);
    } else {
      sessionStorage.setItem(accessToken, data.access_token);
    }

    return this.getUserData().subscribe(() => {
      if (this.isEmsEditUser()) {
        this.router.navigate(['/rdpzsdApproval']);
      } else if (this.isEmsStickersUser()) {
        this.router.navigate(['/rdpzsdStickers']);
      } else {
        this.router.navigate(['/']);
      }
    });
  }

  logout() {
    if (this.currentAuthorizationState !== UserAuthorizationState.logout) {
      this.currentAuthorizationState = UserAuthorizationState.logout;
      this.userData = new UserDto();
      localStorage.clear();
      sessionStorage.clear();
      const currentRoute = window.location.pathname;
      if (pathsWithoutTokenRedirect.indexOf(currentRoute) === -1) {
        this.router.navigate(['login']);
      }
    }
  }

  getUserData() {
    this.currentAuthorizationState = UserAuthorizationState.loading;
    return new Observable((observer: Observer<any>) => {
      return this.userResource.getAuthorizedUser()
        .pipe(
          catchError(() => {
            this.logout();
            observer.next(null);
            observer.complete();
            return throwError(() => new Error('Unautorized'));
          })
        )
        .subscribe(user => {
          this.userData = user;
          this.currentAuthorizationState = UserAuthorizationState.login;
          observer.next(this.userData);
          observer.complete();
        });
    });
  }
}