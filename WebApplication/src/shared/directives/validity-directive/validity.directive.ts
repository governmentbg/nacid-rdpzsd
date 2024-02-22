import { AfterViewInit, Directive, EventEmitter, Input, Output, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";

@Directive()
export abstract class ValidityDirective implements AfterViewInit {

  @Input() isEditMode = false;
  @Input() isValidForm = false;
  @Output() isValidFormChange = new EventEmitter<boolean>();

  @ViewChild('form') form: NgForm;

  ngAfterViewInit() {
    this.form.valueChanges
      .subscribe(() => {
        if (this.isValidForm !== this.form.valid) {
          this.isValidForm = this.form.valid;
          this.isValidFormChange.emit(this.form.valid);
        }
      });
  }
}