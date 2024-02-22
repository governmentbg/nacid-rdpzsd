
import { Component, HostListener, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: './message-modal.component.html'
})
export class MessageModalComponent {

  @Input() warning: string;
  @Input() warningText: string;
  @Input() text: string;
  @Input() acceptButton = 'root.buttons.yes';
  @Input() acceptButtonClass = 'btn-sm btn-primary';
  @Input() declineButton = 'root.buttons.cancel';
  @Input() declineButtonClass = 'btn-sm btn-outline-primary';
  @Input() infoOnly: boolean = false;

  @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
    this.decline();
  }

  constructor(private activeModal: NgbActiveModal) {
  }

  accept() {
    this.activeModal.close(true);
  }

  decline() {
    this.activeModal.close(false);
  }
}
