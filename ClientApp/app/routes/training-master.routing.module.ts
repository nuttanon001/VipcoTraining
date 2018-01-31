import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import { ReportTrainingMasterComponent } from "../components/training-master/report-training-master.component";
import { ReportTrainingComponent } from "../components/training-master/report-training.component";
import { ReportTraining2Component } from "../components/training-master/report.training2.component";
import { TrainingCenterComponent } from "../components/training-master/training-center.component";
import { TrainingMasterComponent } from "../components/training-master/training-master.component";
import { ReportTrainingCostComponent } from "../components/training-master/report-training-cost.component";

// service
import { AuthGuard } from "../services/auth-guard.service";

const trainingMasterRoutes: Routes = [
    {
        path: "training-master",
        canActivate: [AuthGuard],
        component: TrainingCenterComponent,
        children: [
            {
                path: "template/:condition",
                component: ReportTrainingMasterComponent,
            },
            {
                path: "report/:condition",
                component: ReportTrainingComponent,
            },
            {
                path: "employee-report/:condition",
                component: ReportTraining2Component,
            },
            {
                path: "cost-report/:condition",
                component: ReportTrainingCostComponent,
            },
            {
                path: "",
                component: TrainingMasterComponent,
            }
        ]
    }
]

@NgModule({
    imports: [
        RouterModule.forChild(trainingMasterRoutes)
    ],
    exports: [
        RouterModule
    ]
})

export class TrainingMasterRouters { }