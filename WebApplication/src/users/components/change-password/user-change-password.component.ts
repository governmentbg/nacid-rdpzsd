import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { AlertMessageDto } from "src/shared/components/alert-message/models/alert-message.dto";
import { AlertMessageService } from "src/shared/components/alert-message/services/alert-message.service";
import { UserResource } from "src/users/user.resource";
import { UserChangePasswordDto } from "./user-change-password.dto";

@Component({
    selector: 'user-change-password',
    templateUrl: './user-change-password.component.html',
    styleUrls: ['./user-change-password.styles.css']
})
export class UserChangePasswordComponent {

    userChangePasswordDto = new UserChangePasswordDto();

    constructor(
        private alertMessageService: AlertMessageService,
        private router: Router,
        private userResource: UserResource
    ) {
    }

    changePassword() {
        return this.userResource.changePassword(this.userChangePasswordDto)
            .subscribe(() => {
                const message = new AlertMessageDto('errorTexts.succesfullChangePassword', 'success', null)
                this.alertMessageService.next(message);
                this.router.navigate(['/rdpzsd']);
            });
    }

    validatePassword(password: string): boolean {
        return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$/.test(password);
    }
}