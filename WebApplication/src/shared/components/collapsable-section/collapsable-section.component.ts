import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { NgbCollapse } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'collapsable-section',
  templateUrl: './collapsable-section.component.html'
})
export class CollapsableSectionComponent {

  @ViewChild('collapse') collapseElement: NgbCollapse;
  @Input() heading: string;
  @Input() isCollapsed = true;
  @Input() disabled = false;
  @Input() icon: string;
  @Input() classHeader: string;
  @Output() isCollapsedChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  onToggle() {
    if (!this.disabled) {
      this.collapseElement.toggle();
      this.isCollapsedChange.emit(this.isCollapsed);
    }
  }
}
