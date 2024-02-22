import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { AlertMessageDto } from '../models/alert-message.dto';

@Injectable()
export class AlertMessageService {
  messagesSubject: Subject<AlertMessageDto> = new Subject<AlertMessageDto>();

  subscribe(next: (value: AlertMessageDto) => void) {
    return this.messagesSubject.subscribe(next);
  }

  next(value: AlertMessageDto) {
    return this.messagesSubject.next(value);
  }
}
