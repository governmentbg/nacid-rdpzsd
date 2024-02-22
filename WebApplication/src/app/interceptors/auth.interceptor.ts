import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { accessToken } from 'src/users/dtos/login.dtos';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const bearerAccessToken = localStorage.getItem(accessToken) || sessionStorage.getItem(accessToken);

    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${bearerAccessToken}`
      }
    });

    return next
      .handle(request);
  }
}
