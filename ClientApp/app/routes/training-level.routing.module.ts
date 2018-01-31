import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import { TrainingLevelCenterComponent } from "../components/training-level/training-level-center.component";
import { TrainingLevleMasterComponent } from "../components/training-level/training-level-master.component";
// service
import { AuthGuard } from "../services/auth-guard.service";
const trainingLevelRoutes: Routes = [
    {
        path: "training-level",
        component: TrainingLevelCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: TrainingLevleMasterComponent,
            }
        ]
    }
]

@NgModule({
    imports: [
        RouterModule.forChild(trainingLevelRoutes)
    ],
    exports: [
        RouterModule
    ]
})

export class TrainingLevelRouters { }