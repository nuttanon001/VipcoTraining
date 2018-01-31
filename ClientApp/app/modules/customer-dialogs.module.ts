import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// components
import { CourseDialogComponent } from "../components/dialog/course-dialog-data.component";
import { EmployeeDialogComponent } from "../components/dialog/employee-dialog-data.component";
import { PositionDialogComponent } from "../components/dialog/position-dialog-data.component";
import { ProgramDialogComponent } from "../components/dialog/program-dialog-data.component";
import { GroupDialogComponent } from "../components/dialog/group-dialog-data.component";
// modules
import { CustomMaterialModule } from "./customer-material.module";
import { CustomPrimeNgModule } from "./customer-primeng.module";

@NgModule({
    declarations: [
        CourseDialogComponent,
        EmployeeDialogComponent,
        PositionDialogComponent,
        ProgramDialogComponent,
        GroupDialogComponent
    ],
    imports: [
        CommonModule,
        CustomMaterialModule,
        CustomPrimeNgModule,
        FormsModule,
        ReactiveFormsModule,
    ],
    exports: [
        CourseDialogComponent,
        EmployeeDialogComponent,
        PositionDialogComponent,
        ProgramDialogComponent,
        GroupDialogComponent
    ],
})
export class CustomDialogModule { }