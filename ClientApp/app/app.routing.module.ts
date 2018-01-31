import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
// Service
import { AuthService  } from "./services/auth.service";
import { AuthGuard } from "./services/auth-guard.service";
// Component
import { HomeComponent } from "./components/home/home.component";
import { LoginComponent } from "./components/login/login.component";

const appRoutes: Routes = [
    {
        path: "home",
        component: HomeComponent
    },
    {
        path: "login",
        component: LoginComponent
    },
    //{
    //    path: "employee",
    //    loadChildren: "./modules/employee.module#EmployeeModule",
    //    canLoad: [AuthGuard]
    //},
    //{
    //    path: "education",
    //    loadChildren: "./modules/education.module#EducationModule",
    //    canLoad: [AuthGuard]
    //},
    //{
    //    path: "place",
    //    loadChildren: "./modules/place.module#PlaceModule",
    //    canLoad: [AuthGuard]
    //},
    //{
    //    path: "training-course",
    //    loadChildren: "./modules/training-course.module#TrainingCourseModule",
    //    canLoad: [AuthGuard]
    //},
    //{
    //    path: "training-level",
    //    loadChildren: "./modules/training-level.module#TrainingLevelModule",
    //    canLoad: [AuthGuard]
    //},
    //{
    //    path: "training-type",
    //    loadChildren: "./modules/training-type.module#TrainingTypeModule",
    //    canLoad: [AuthGuard]
    //},
    //{
    //    path: "training-program",
    //    loadChildren: "./modules/training-program.module#TrainingProgramModule",
    //    canLoad: [AuthGuard]
    //},
    //{
    //    path: "training-master",
    //    loadChildren: "./modules/training-master.module#TrainingMasterModule",
    //    canLoad: [AuthGuard]
    //},
    {
        path: "full",
        redirectTo: "/home"
    },
    {
        path: "**",
        redirectTo: "/home"
    }
];

@NgModule({
    imports: [RouterModule.forRoot(appRoutes)],
    exports: [RouterModule],
})
export class AppRoutingModule { }