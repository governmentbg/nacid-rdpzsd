import { Directive, forwardRef, HostListener, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[numbersOnly]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => NumbersOnlyDirective),
      multi: true
    }
  ]
})
export class NumbersOnlyDirective implements Validator {
  regexStr = '^[0-9]+$';
  @Input() enableEmptyValidation: boolean = false;
  @Input() min: number = null;
  @Input() max: number = null;

  validate(control: AbstractControl): { [key: string]: any } | null {
    if (this.enableEmptyValidation && !control.value) {
      return null;
    } else if (!control.value || (this.min && control.value < this.min) || (this.max && control.value > this.max)) {
      return { minMax: 'value is not in range' };
    } else {
      return null;
    }
  }

  constructor() { }

  @HostListener('keypress', ['$event'])
  onKeyPress(event: any) {
    return new RegExp(this.regexStr).test(event.key);
  }

  @HostListener('paste', ['$event'])
  blockPaste(event: ClipboardEvent) {
    this.validateFields(event);
  }

  validateFields(event: ClipboardEvent) {
    const pastedData = event.clipboardData.getData('text/plain');
    if (!new RegExp(this.regexStr).test(pastedData)) {
      event.preventDefault();
    }
  }
}
