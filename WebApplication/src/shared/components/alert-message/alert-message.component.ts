import { Component, OnInit } from '@angular/core';
import { AlertMessageDto } from './models/alert-message.dto';
import { AlertMessageService } from './services/alert-message.service';

@Component({
  selector: 'alert-message',
  templateUrl: 'alert-message.component.html'
})
export class AlertMessageComponent implements OnInit {
  alertMessages: AlertMessageDto[] = [];

  constructor(
    private alertMessageService: AlertMessageService
  ) {
  }

  removeMessage(message: AlertMessageDto) {
    const index = this.alertMessages.indexOf(message);
    this.alertMessages.splice(index, 1);
  }

  private addMessage(message: AlertMessageDto) {
    this.alertMessages.push(message);

    window.scroll({
      top: 0,
      behavior: 'smooth'
    });

    const self = this;
    setTimeout(() => self.removeMessage(message),
      (message.timeoutSeconds ? message.timeoutSeconds : 15) * 1000);
  }

  ngOnInit() {
    this.alertMessageService
      .subscribe((alertMessage: AlertMessageDto) => {
        this.addMessage(alertMessage);
      });
  }
}
