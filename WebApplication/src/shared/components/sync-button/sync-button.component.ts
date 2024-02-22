import { Component, HostListener, Input } from '@angular/core';

@Component({
  selector: 'sync-button',
  templateUrl: 'sync-button.component.html'
})
export class SyncButtonComponent {
  @Input() text: string;
  @Input() icon: string;
  @Input() btnClass: string;
  @Input() titleText: string;
  @Input() showTextOnPending = true;
  @Input() disabled: boolean = false;

  @Input() click: Function;
  @Input() clickContext: object;
  @Input() clickParams: any;

  pending = false;

  @HostListener('click', ['$event'])
  onClick(event: any) {
    if (this.pending || this.disabled) {
      return;
    }

    if (event.which !== 1) {
      return;
    }

    event.preventDefault();

    // check if a function is bound to the button, else it is routing
    if (this.click) {
      const onClickBound = this.click.bind(this.clickContext);

      let result: any;
      if (this.clickParams) {
        result = onClickBound(...this.clickParams, event);
      } else {
        result = onClickBound(event);
      }

      const context = this;

      // For subscribe
      if (result && result.next && typeof (result.next) === 'function') {
        this.pending = true;
        result.complete = () => context.pending = false;
        result.error = () => context.pending = false;
      }

      // For observable
      if (result && result.subscribe && typeof (result.subscribe) === 'function') {
        this.pending = true;
        result.subscribe(
          function onNext() {
            context.pending = false;
          }, function onError() {
            context.pending = false;
          });
      }

      // For promise
      if (result && result.then && typeof (result.then) === 'function') {
        this.pending = true;
        result.then(() => context.pending = false)
          .catch(() => context.pending = false);
      }
    }
  }
}
