import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// 3rd party
import "hammerjs";
import { AngularSplitModule } from "angular-split";
// componentes
import { TrainingLevelCenterComponent } from "../components/training-level/training-level-center.component";
import { TrainingLevelEditComponent } from "../components/training-level/training-level-edit.component";
import { TrainingLevleMasterComponent } from "../components/training-level/training-level-master.component";
import { TrainingLevelViewComponent } from "../components/training-level/training-level-view.component";
// modules
import { CustomFormModule } from "./customer-form.module";
import { CustomPrimeNgModule } from "./customer-primeng.module";
import { CustomMaterialModule } from "./customer-material.module";
import { DialogsModule } from "./dialogs.module";
import { TrainingLevelRouters } from "../routes/training-level.routing.module";

@NgModule({
    declarations: [
        TrainingLevelCenterComponent,
        TrainingLevleMasterComponent,
        TrainingLevelViewComponent,
        TrainingLevelEditComponent
    ],
    imports: [
        CommonModule,
        CustomMaterialModule,
        CustomPrimeNgModule,
        CustomFormModule,
        AngularSplitModule,
        TrainingLevelRouters,
        DialogsModule,
        FormsModule,
        ReactiveFormsModule,
    ],
})

export class TrainingLevelModule {

}