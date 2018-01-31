import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// 3rd party
import "hammerjs";
import { AngularSplitModule } from "angular-split";
// componentes
import { TrainingProgramCenterComponent } from "../components/training-program/training-program-center.component";
import { TrainingProgramMasterComponent } from "../components/training-program/training-program-master.component";
import { TrainingProgramViewComponent } from "../components/training-program/training-program-view.component";
import { TrainingProgramEditComponent } from "../components/training-program/training-program-edit.component";
// modules
import { DialogsModule } from "./dialogs.module";
import { CustomDialogModule } from "./customer-dialogs.module";
import { CustomFormModule } from "./customer-form.module";
import { CustomMaterialModule } from "./customer-material.module";
import { CustomPrimeNgModule } from "./customer-primeng.module";
import { TrainingProgramRouters } from "../routes/training-program.routing.module";

@NgModule({
    declarations: [
        TrainingProgramCenterComponent,
        TrainingProgramMasterComponent,
        TrainingProgramViewComponent,
        TrainingProgramEditComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        CustomFormModule,
        CustomMaterialModule,
        CustomPrimeNgModule,
        CustomDialogModule,
        AngularSplitModule,
        TrainingProgramRouters,
        DialogsModule,
    ],
})

export class TrainingProgramModule {
}