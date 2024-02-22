import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { UserAuthorizationState } from 'src/users/dtos/login.dtos';
import { UserDataService } from 'src/users/services/user-data.service';

@Component({
  selector: 'user-side-bar',
  templateUrl: './user-side-bar.component.html',
  styleUrls: ['./user-side-bar.styles.css']
})
export class UserSideBarComponent {

  @ViewChild('toggleUserSideBarButton') toggleUserSideBarButton: ElementRef;
  userAutorizationState = UserAuthorizationState;

  @HostListener('document:click', ['$event'])
  clickout(event: any) {
    if (this.userDataService.currentAuthorizationState === this.userAutorizationState.login && this.toggleUserSideBarButton) {
      const userSideBarIsExpanded = JSON.parse(this.toggleUserSideBarButton.nativeElement.getAttribute('aria-expanded'));
      if (!this.elementRef.nativeElement.contains(event.target) && userSideBarIsExpanded) {
        this.toggleUserSideBarButton.nativeElement.click();
      }
    }
  }

  constructor(
    public userDataService: UserDataService,
    private elementRef: ElementRef
  ) {
    this.userDataService
      .toggleSidebarSubject
      .subscribe(() => {
        this.toggleUserSideBarButton.nativeElement.click();
      });
  }
}
