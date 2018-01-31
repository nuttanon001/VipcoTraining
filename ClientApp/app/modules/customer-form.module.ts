import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ControlMessagesComponent } from "../components/form/control-messages.componet";

@NgModule({
    declarations: [
        ControlMessagesComponent
    ],
    imports: [
        CommonModule
    ],
    exports: [
        ControlMessagesComponent
    ],
})
export class CustomFormModule { }