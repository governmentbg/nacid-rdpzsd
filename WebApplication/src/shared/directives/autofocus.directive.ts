import { Directive, ElementRef, Input, OnInit } from '@angular/core';

@Directive({
  selector: '[autofocus]'
})
export class AutofocusDirective implements OnInit {

  @Input() autofocus = false;
  @Input() autofocusDelay = 50;

  constructor(private el: ElementRef) {
  }

  ngOnInit() {
    if (this.autofocus) {
      window.setTimeout(() => {
        this.el.nativeElement.focus();
      }, this.autofocusDelay);
    }
  }
}