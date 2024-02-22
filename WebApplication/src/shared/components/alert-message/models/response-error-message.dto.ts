import { AlertMessageDto, EmsAlertMessageDto } from "./alert-message.dto";

export class ResponseErrorMessageDto {
  errorMessages: AlertMessageDto[] = [];
  messages: EmsAlertMessageDto[] = [];
}