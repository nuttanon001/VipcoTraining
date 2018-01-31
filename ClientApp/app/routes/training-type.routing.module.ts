import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import { TrainingTypeMasterComponent } from "../components/training-type/training-type-master.component";
import { TrainingTypeCenterComponent } from "../components/training-type/training-type-center.component";
// service
import { AuthGuard } from "../services/auth-guard.service";

const trainingTypeRoutes: Routes = [
    {
        path: "training-type",
        component: TrainingTypeCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: TrainingTypeMasterComponent,
            }
        ]
    }
]

@NgModule({
    imports: [
        RouterModule.forChild(trainingTypeRoutes)
    ],
    exports: [
        RouterModule
    ]
})

export class TrainingTypeRouters { }