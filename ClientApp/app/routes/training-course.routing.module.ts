import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import { TrainingCourseMasterComponent } from "../components/training-course/training-course-master.component";
import { TrainingCourseCenterComponent } from "../components/training-course/training-course-center.component";
// service
import { AuthGuard } from "../services/auth-guard.service";

const trainingCourseRoutes: Routes = [
    {
        path: "training-course",
        component: TrainingCourseCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: TrainingCourseMasterComponent,
            }
        ]
    }
]

@NgModule({
    imports: [
        RouterModule.forChild(trainingCourseRoutes)
    ],
    exports: [
        RouterModule
    ]
})

export class TrainingCourseRouters { }