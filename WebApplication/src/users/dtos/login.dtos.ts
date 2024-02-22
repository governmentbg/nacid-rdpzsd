export const accessToken = 'access_token';

export class LoginDto {
  username: string;
  password: string;
  rememberMe = false;
}

export class TokenRequestResponse {
  access_token: string;
}

export enum LoginError {
  none = 1,
  wrongCredentials = 2,
  locked = 3
}

export enum UserAuthorizationState {
  login = 1,
  logout = 2,
  loading = 3
}
