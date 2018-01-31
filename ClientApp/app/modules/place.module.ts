import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// 3rd party
import "hammerjs";
import { AngularSplitModule } from "angular-split";
// componentes
import { PlaceCenterComponent } from "../components/place/place-center.component";
import { PlaceEditComponent } from "../components/place/place-edit.component";
import { PlaceMasterComponent } from "../components/place/place-master.component";
import { PlaceViewComponent } from "../components/place/place-view.component";
// modules
import { CustomFormModule } from "./customer-form.module";
import { CustomPrimeNgModule } from "./customer-primeng.module";
import { CustomMaterialModule } from "./customer-material.module";
import { DialogsModule } from "./dialogs.module";
import { PlaceRouters } from "../routes/place.routing.module";

@NgModule({
    declarations: [
        PlaceCenterComponent,
        PlaceMasterComponent,
        PlaceViewComponent,
        PlaceEditComponent
    ],
    imports: [
        CommonModule,
        CustomMaterialModule,
        CustomPrimeNgModule,
        CustomFormModule,
        AngularSplitModule,
        PlaceRouters,
        DialogsModule,
        FormsModule,
        ReactiveFormsModule,
    ],
})

export class PlaceModule { }