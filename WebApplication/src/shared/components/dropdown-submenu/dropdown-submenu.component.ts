import { Component, ElementRef, HostListener, Input, OnInit } from '@angular/core';

@Component({
  selector: 'dropdown-submenu',
  templateUrl: './dropdown-submenu.component.html',
  styleUrls: ['./dropdown-submenu.component.css']
})
export class DropdownSubmenuComponent {
  
	opened = false;
	@Input() text: string;

	toggle(entered: boolean) {
		this.opened = entered;
	}
}
