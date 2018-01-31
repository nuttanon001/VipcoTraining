import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from "./components/login/login.component";

// module
import { CustomFormModule } from "./modules/customer-form.module";
import { CustomMaterialModule } from "./modules/customer-material.module";
import { DialogsModule } from "./modules/dialogs.module";
// service
import { DialogsService } from "./services/dialogs.service";
import { ValidationService } from "./services/validation.service";
import { AuthService } from "./services/auth.service";
import { AuthGuard } from "./services/auth-guard.service";
// import { DialogsModule,CustomMaterialModule } from "./modules/module.index";
// module
import { TrainingCourseModule } from "./modules/training-course.module";
import { EducationModule } from "./modules/education.module";
import { PlaceModule } from "./modules/place.module";
import { TrainingLevelModule } from "./modules/training-level.module";
import { TrainingMasterModule } from "./modules/training-master.module";
import { TrainingProgramModule } from "./modules/training-program.module";
import { TrainingTypeModule } from "./modules/training-type.module";
import { TestModule } from "./modules/test.module";
// router
import { AppRoutingModule } from "./app.routing.module";
@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        NavMenuComponent,
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        // Modules
        DialogsModule,
        CustomFormModule,
        CustomMaterialModule,
        // Modules Component
        EducationModule,
        PlaceModule,
        TrainingCourseModule,
        TrainingLevelModule,
        TrainingMasterModule,
        TrainingProgramModule,
        TrainingTypeModule,
        // Test
        // TestModule,
        // Routing
        AppRoutingModule,
    ],
    providers: [
        DialogsService,
        ValidationService,
        AuthGuard,
        AuthService
    ]
})
export class AppModuleShared {
}
