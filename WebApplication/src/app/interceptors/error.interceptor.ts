import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { AlertMessageDto } from "src/shared/components/alert-message/models/alert-message.dto";
import { ResponseErrorMessageDto } from "src/shared/components/alert-message/models/response-error-message.dto";
import { AlertMessageService } from "src/shared/components/alert-message/services/alert-message.service";
import { UserDataService } from "src/users/services/user-data.service";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(
    private alertMessageService: AlertMessageService,
    private modalService: NgbModal,
    private userDataService: UserDataService
  ) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next
      .handle(request)
      .pipe(
        catchError((err) => {
          this.modalService.dismissAll();

          if (err.status === 422) {
            const error = err.error as ResponseErrorMessageDto;

            if (error.errorMessages && error.errorMessages.length > 0) {
              for (let i = 0; i <= error.errorMessages.length - 1; i++) {
                error.errorMessages[i].domainErrorCode = 'errorTexts.' + error.errorMessages[i].domainErrorCode.charAt(0).toLowerCase() + error.errorMessages[i].domainErrorCode.slice(1);
                error.errorMessages[i].type = 'danger';
                this.alertMessageService.next(error.errorMessages[i]);
              }
            }

            if (error.messages && error.messages.length > 0) {
              for (let i = 0; i <= error.messages.length - 1; i++) {
                error.messages[i].domainErrorCode = 'errorTexts.' + error.messages[i].domainErrorCode.charAt(0).toLowerCase() + error.messages[i].domainErrorCode.slice(1);
                const alertMessage = new AlertMessageDto(error.messages[i].domainErrorCode, 'danger', error.messages[i].domainErrorText, 15);
                this.alertMessageService.next(alertMessage);
              }
            }

            return throwError(() => new Error('Domain error'));
          } else if (err.status >= 500 && err.status <= 502) {
            const alertMessage = new AlertMessageDto('errorTexts.internalServerError', 'danger', null);
            this.alertMessageService.next(alertMessage);
            return throwError(() => new Error('Internal server error'));
          } else if (err.status === 503) {
            const alertMessage = new AlertMessageDto('errorTexts.serverUnavailible', 'danger', null);
            this.alertMessageService.next(alertMessage);
            return throwError(() => new Error('Server unavailible'));
          } else if (err.status === 401) {
            this.userDataService.logout();
            return throwError(() => new Error('Unautorized'));
          } else if (err.status === 400 && err.url.indexOf('/api/token') >= 0) {
            const alertMessage = new AlertMessageDto('errorTexts.invalidGrant', 'danger', null);
            this.alertMessageService.next(alertMessage);
            return throwError(() => new Error('Unautorized'));
          }
          else if (err.status === 400 && err.url.indexOf('/api/user/currentData') >= 0) {
            const alertMessage = new AlertMessageDto('errorTexts.invalidInstitution', 'danger', null);
            this.alertMessageService.next(alertMessage);
            this.userDataService.logout();
            return throwError(() => new Error('Unautorized'));
          }

          return throwError(() => new Error('Undefined error'));
        })
      )
  }
}