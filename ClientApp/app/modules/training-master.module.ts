import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
// 3rd party
import "hammerjs";
import { AngularSplitModule } from "angular-split";
// componentes
import { TrainingCenterComponent } from "../components/training-master/training-center.component";
import { TrainingMasterComponent } from "../components/training-master/training-master.component";
import { TrainingViewComponent } from "../components/training-master/training-view.component";
import { TrainingEditComponent } from "../components/training-master/training-edit.component";
import { ReportTrainingComponent } from "../components/training-master/report-training.component";
import { ReportTraining2Component } from "../components/training-master/report.training2.component";
import { ReportTrainingCostComponent } from "../components/training-master/report-training-cost.component";

import { ReportTrainingMasterComponent } from "../components/training-master/report-training-master.component";
// modules
import { CustomDialogModule } from "./customer-dialogs.module";
import { CustomFormModule } from "./customer-form.module";
import { CustomMaterialModule } from "./customer-material.module";
import { CustomPrimeNgModule } from "./customer-primeng.module";
import { DialogsModule } from "./dialogs.module";
import { TrainingMasterRouters } from "../routes/training-master.routing.module";

@NgModule({
    declarations: [
        TrainingCenterComponent,
        TrainingMasterComponent,
        TrainingViewComponent,
        TrainingEditComponent,
        ReportTrainingComponent,
        ReportTrainingMasterComponent,
        ReportTraining2Component,
        ReportTrainingCostComponent
    ],
    imports: [
        CommonModule,
        CustomMaterialModule,
        CustomPrimeNgModule,
        CustomDialogModule,
        CustomFormModule,
        AngularSplitModule,
        TrainingMasterRouters,
        DialogsModule,
        FormsModule,
        ReactiveFormsModule,
    ],
})

export class TrainingMasterModule {
}