import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import { TrainingProgramCenterComponent } from "../components/training-program/training-program-center.component";
import { TrainingProgramMasterComponent } from "../components/training-program/training-program-master.component";
// service
import { AuthGuard } from "../services/auth-guard.service";

const trainingProgramRoutes: Routes = [
    {
        path: "training-program",
        component: TrainingProgramCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: TrainingProgramMasterComponent,
            }
        ]
    }
]

@NgModule({
    imports: [
        RouterModule.forChild(trainingProgramRoutes)
    ],
    exports: [
        RouterModule
    ]
})

export class TrainingProgramRouters { }