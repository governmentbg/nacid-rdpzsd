import { AfterViewInit, Directive, ElementRef, Input } from '@angular/core';
import { NgModel } from '@angular/forms';

@Directive({
  selector: '[setColor]'
})
export class LabelColorDirective implements AfterViewInit{
  @Input() element: NgModel = null;

  constructor(private el: ElementRef) {}

  ngAfterViewInit(){
    this.element.valueChanges.subscribe(()=> {
      if(this.element.invalid){
        this.el.nativeElement.style.color = 'rgba(220, 20, 60)';
      }
      else if(this.element.valid){
        this.el.nativeElement.style.color = 'rgb(33, 37, 41)';
      }
    })
  }
}
