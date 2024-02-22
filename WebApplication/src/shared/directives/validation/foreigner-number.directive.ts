import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

export function ForeignerNumberValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const isWhitespace = (control.value || '').trim().length === 0;
    if (isWhitespace) {
      return { whitespace: 'value is only whitespace' };
    } else {
      if (isNaN(control.value) || control.value.length !== 10) {
        return { foreignerNumber: 'value is not foreigner number' };
      } else {
        var weights = [21, 19, 17, 13, 11, 9, 7, 3, 1];
        var checkSum = control.value.charAt(9);
        var mod = 10;
        var sum = 0;

        for (let i = 0; i <= control.value.length - 2; i++) {
          sum += (control.value[i] * weights[i]);
        }

        var validCheckSum = sum % mod;

        if (validCheckSum >= 10) {
          validCheckSum = 0;
        }

        return checkSum == validCheckSum ? null : { foreignerNumber: 'value is not foreigner number' };
      }
    }
  };
}

@Directive({
  selector: '[foreignerNumberValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => ForeignerNumberDirective),
      multi: true
    }
  ]
})

export class ForeignerNumberDirective implements Validator {

  @Input() enableEmptyValidation = false;

  private valFn = ForeignerNumberValidator();
  validate(control: AbstractControl): { [key: string]: any } | null {
    if (!this.enableEmptyValidation) {
      return this.valFn(control);
    } else {
      return control.value ? this.valFn(control) : null;
    }
  }
}
