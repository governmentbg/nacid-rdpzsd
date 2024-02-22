import { Component, ElementRef, HostListener, Input } from '@angular/core';

@Component({
  selector: 'dropdown-button',
  templateUrl: './dropdown-button.component.html',
  styleUrls: ['./dropdown-button.styles.css']
})
export class DropdownButtonComponent {
  opened = false;
  @Input() text: string;
  @Input() btnClass: string;
  @Input() textClass: string;
  @Input() icon: string;
  @Input() disabled: boolean;
  @Input() toggleable = true;
  @Input() dropdownRightClass = 'dropdown-right';
  @HostListener('document:click', ['$event.target'])

  public onClick(target: any) {
    const clickedInside = this.elementRef.nativeElement.contains(target);
    if (!clickedInside) {
      this.opened = false;
    }
  }
  constructor(private elementRef: ElementRef) { }

  toggle() {
    this.opened = !this.opened;
  }
}
