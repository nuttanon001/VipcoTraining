import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// 3rd party
import "hammerjs";
import { AngularSplitModule } from "angular-split";
// componentes
import { TrainingCourseCenterComponent } from "../components/training-course/training-course-center.component";
import { TrainingCourseMasterComponent } from "../components/training-course/training-course-master.component";
import { TrainingCourseViewComponent } from "../components/training-course/training-course-view.component";
import { TrainingCourseEditComponent } from "../components/training-course/training-course-edit.component";
import { AttactFileComponent } from "../components/training-master/attact.file.component";
// modules
import { CustomFormModule } from "./customer-form.module";
import { CustomPrimeNgModule } from "./customer-primeng.module";
import { CustomMaterialModule } from "./customer-material.module";
import { DialogsModule } from "./dialogs.module";
import { TrainingCourseRouters } from "../routes/training-course.routing.module";

@NgModule({
    declarations: [
        TrainingCourseCenterComponent,
        TrainingCourseMasterComponent,
        TrainingCourseViewComponent,
        TrainingCourseEditComponent,
        AttactFileComponent,
    ],
    imports: [
        FormsModule,
        CommonModule,
        ReactiveFormsModule,
        AngularSplitModule,
        TrainingCourseRouters,
        CustomMaterialModule,
        CustomPrimeNgModule,
        DialogsModule,
        CustomFormModule,
    ],
})

export class TrainingCourseModule {
}