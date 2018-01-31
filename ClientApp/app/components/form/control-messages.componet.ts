import { Component, Input } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { ValidationService } from "../../services/validation.service";

@Component({
    selector: "control-messages",
    template: `<div class="alert alert-danger" *ngIf="errorMessage !== null">{{errorMessage}}</div>`
})
export class ControlMessagesComponent {
    @Input("control") control: FormControl;
    constructor(private validation:ValidationService) { }

    get errorMessage() {
        if (this.control && this.control.errors) {
            for (let propertyName in this.control.errors) {
                if (this.control.errors.hasOwnProperty(propertyName) && this.control.touched) {
                    return this.validation.getValidatorErrorMessage(propertyName, this.control.errors[propertyName]);
                }
            }
        }
        return null;
    }
}