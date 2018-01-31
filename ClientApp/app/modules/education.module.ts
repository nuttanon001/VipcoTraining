import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// 3rd party
import "hammerjs";
import { AngularSplitModule } from "angular-split";
// componentes
import { EducationCenterComponent } from "../components/education/education-center.component";
import { EducationEditComponent } from "../components/education/education-edit.component";
import { EducationMasterComponent } from "../components/education/education-master.component";
import { EducationViewComponent } from "../components/education/education-view.component";
// modules
import { CustomFormModule } from "./customer-form.module";
import { CustomPrimeNgModule } from "./customer-primeng.module";
import { CustomMaterialModule } from "./customer-material.module";
import { DialogsModule } from "./dialogs.module";
import { EducationRouters } from "../routes/education.routing.module";

@NgModule({
    declarations: [
        EducationCenterComponent,
        EducationMasterComponent,
        EducationViewComponent,
        EducationEditComponent
    ],
    imports: [
        //Angular
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        //3rd
        AngularSplitModule,
        //Custom
        CustomMaterialModule,
        CustomPrimeNgModule,
        CustomFormModule,
        DialogsModule,
        EducationRouters,
    ],
})

export class EducationModule {}