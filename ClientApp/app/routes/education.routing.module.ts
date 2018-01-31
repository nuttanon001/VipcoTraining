import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import { EducationMasterComponent } from "../components/education/education-master.component";
import { EducationCenterComponent } from "../components/education/education-center.component";
// service
import { AuthGuard } from "../services/auth-guard.service";

const educationRoutes: Routes = [
    {
        path: "education",
        component: EducationCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: EducationMasterComponent,
            }
        ],
    }
]

@NgModule({
    imports: [
        RouterModule.forChild(educationRoutes)
    ],
    exports: [
        RouterModule
    ]
})

export class EducationRouters { }