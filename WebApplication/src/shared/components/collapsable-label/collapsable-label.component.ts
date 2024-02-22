import { Component, Input, ViewChild } from '@angular/core';
import { NgbCollapse } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'collapsable-label',
  templateUrl: './collapsable-label.component.html',
  styleUrls: ['./collapsable-label.styles.css']
})
export class CollapsableLabelComponent {

  @ViewChild('collapse') collapseElement: NgbCollapse;
  @Input() heading: string;
  @Input() isCollapsed = true;
  @Input() disabled = false;
  @Input() icon: string;
  @Input() fontSize = 'fs-15'
  @Input() secondIcon: string;
  @Input() ngbTooltipClass: string;
  @Input() iconTooltipClass: string;

  onToggle() {
    if (!this.disabled) {
      this.collapseElement.toggle();
    }
  }
}
