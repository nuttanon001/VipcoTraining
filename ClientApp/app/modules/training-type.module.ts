import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// 3rd party
import "hammerjs";
import { AngularSplitModule } from "angular-split";
// componentes
import { TrainingTypeCenterComponent } from "../components/training-type/training-type-center.component"
import { TrainingTypeEditComponent } from "../components/training-type/training-type-edit.component"
import { TrainingTypeMasterComponent } from "../components/training-type/training-type-master.component"
import { TrainingTypeViewComponent } from "../components/training-type/training-type-view.component"
// modules
import { CustomFormModule } from "./customer-form.module";
import { CustomPrimeNgModule } from "./customer-primeng.module";
import { CustomMaterialModule } from "./customer-material.module";
import { DialogsModule } from "./dialogs.module";

import { TrainingTypeRouters } from "../routes/training-type.routing.module";

@NgModule({
    declarations: [
        TrainingTypeCenterComponent,
        TrainingTypeMasterComponent,
        TrainingTypeViewComponent,
        TrainingTypeEditComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AngularSplitModule,
        TrainingTypeRouters,
        DialogsModule,
        CustomMaterialModule,
        CustomPrimeNgModule,
        CustomFormModule,
    ],
})

export class TrainingTypeModule {

}