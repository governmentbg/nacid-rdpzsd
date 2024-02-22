export class AlertMessageDto {
  domainErrorCode: string;
  type: string;
  errorText: string;
  timeoutSeconds?: number;

  constructor(domainErrorCode: string, type: string, errorText: string, timeoutSeconds: number = 15) {
    this.domainErrorCode = domainErrorCode;
    this.type = type;
    this.timeoutSeconds = timeoutSeconds;
    this.errorText = errorText;
  }
}

export class EmsAlertMessageDto {
  domainErrorCode: string;
  type: string;
  domainErrorText: string;
  timeoutSeconds?: number = 15;
}