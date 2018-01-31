import { NgModule } from "@angular/core";
import {
    DataTableModule,
    DialogModule,
    SharedModule,
    CalendarModule,
    DropdownModule,
    InputMaskModule,
    TreeModule,
    TreeTableModule,
    AccordionModule,
} from "primeng/primeng";

@NgModule({
    exports: [
        DataTableModule,
        DialogModule,
        SharedModule,
        CalendarModule,
        DropdownModule,
        InputMaskModule,
        TreeTableModule,
        TreeModule,
        AccordionModule,
    ],
})
export class CustomPrimeNgModule { }