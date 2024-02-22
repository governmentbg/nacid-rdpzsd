import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[uanValidation]'
})
export class UanDirective {
  regexStr = '^(?![oOiIjJ])[a-zA-Z0-9]+$';

  constructor() { }

  @HostListener('keypress', ['$event'])
  onKeyPress(event: any){
    return new RegExp(this.regexStr).test(event.key);
  }

  @HostListener('paste', ['$event'])
  blockPaste(event: ClipboardEvent){
    this.validateFields(event);
  }

  validateFields(event: ClipboardEvent){
    const pastedData = event.clipboardData.getData('text/plain');
    if(!new RegExp(this.regexStr).test(pastedData)){
      event.preventDefault();
    }
  }
}
