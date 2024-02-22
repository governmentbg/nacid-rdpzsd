import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UserChangePasswordDto } from "./components/change-password/user-change-password.dto";
import { LoginDto, TokenRequestResponse } from "./dtos/login.dtos";
import { UserActivationDto } from "./dtos/user-activation.dto";
import { UserDto } from "./dtos/user.dto";

@Injectable()
export class UserResource {

  constructor(
    private http: HttpClient
  ) { }

  login(loginDto: LoginDto): Observable<TokenRequestResponse> {
    return this.http.post<TokenRequestResponse>('api/token', this.urlEncodeLoginModel(loginDto))
  }

  getAuthorizedUser(): Observable<UserDto> {
    return this.http.get<UserDto>('api/user/currentData');
  }

  activate(userActivation: UserActivationDto): Observable<void> {
    return this.http.post<void>('api/ems/users/activate', userActivation);
  }

  recoverPassword(username: string): Observable<void> {
    const usernameDto = {
      username
    };

    return this.http.post<void>('api/ems/users/recoverPassword', usernameDto);
  }

  changePassword(userChangePasswordDto: UserChangePasswordDto): Observable<void> {
    return this.http.post<void>('api/ems/user/changePassword', userChangePasswordDto);
  }

  private urlEncodeLoginModel(loginDto: LoginDto): string {
    return 'username=' + loginDto.username + '&password=' + loginDto.password + '&grant_type=password';
  }
}