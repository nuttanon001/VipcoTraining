import { NgModule } from "@angular/core";
import {
    MdButtonModule,
    MdCheckboxModule,
    MdProgressBarModule,
    MdTooltipModule,
    MdSidenavModule,
    MdInputModule,
    MdIconModule,
    MdMenuModule,
    MdDialogModule,
    MdTabsModule,
    MdCardModule,
    MdSortModule,
    MdPaginatorModule,
} from "@angular/material";

@NgModule({
    exports: [
        MdButtonModule,
        MdCheckboxModule,
        MdProgressBarModule,
        MdTooltipModule,
        MdSidenavModule,
        MdMenuModule,
        MdInputModule,
        MdIconModule,
        MdDialogModule,
        MdTabsModule,
        MdCardModule,
        MdSortModule,
        MdPaginatorModule,
    ],
})
export class CustomMaterialModule { }