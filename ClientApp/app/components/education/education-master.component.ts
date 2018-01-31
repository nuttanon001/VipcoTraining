import { Component, ViewContainerRef } from "@angular/core";
// component
import { AbstractMasterComponent } from "../abstract/abstract-master.component";
// classes
import { IEducation } from "../../classes/education.class";
import { LazyLoad } from "../../classes/lazyload.class";
import { LazyLoadEvent } from "primeng/primeng";
// service
import { DialogsService } from "../../services/dialogs.service";
import { EducationService } from "../../services/education.service";
// commuincate
import { EducationCommunicateService } from "../../communicates/education.communicate"
// timezone
import * as moment from "moment-timezone";

@Component({
    selector: "education-master",
    templateUrl: "./education-master.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        EducationService,
        EducationCommunicateService,
    ],
})

export class EducationMasterComponent
    extends AbstractMasterComponent<IEducation, EducationService> {
    constructor(
        educationService: EducationService,
        educationServiceCom: EducationCommunicateService,
        dialogsService: DialogsService,
        viewContainerRef: ViewContainerRef,
    ) {
        super(
            educationService,
            educationServiceCom,
            dialogsService,
            viewContainerRef
        );
    }

    // on get data with lazy load
    onGetAllWithLazyload(lazyload: LazyLoad): void {
        this.service.getAllWithLazyLoad(lazyload)
            .subscribe(restData => {
                this.values = restData.Data;
                this.totalRow = restData.TotalRow;
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(education: IEducation): IEducation {
        var zone = "Asia/Bangkok";
        if (education !== null) {
            if (education.CreateDate !== null) {
                education.CreateDate = moment.tz(education.CreateDate, zone).toDate();
            }
            if (education.ModifyDate !== null) {
                education.ModifyDate = moment.tz(education.ModifyDate, zone).toDate();
            }
        }
        return education;
    }

    // on insert data
    onInsertToDataBase(education: IEducation): void {
        // change timezone
        education = this.changeTimezone(education);
        // insert data
        this.service.post(education).subscribe(
            (complete: any) => {
                this.displayValue = complete;
                this.onSaveComplete();
            },
            (error: any) => {
                console.error(error);
                this.editValue.Creator = undefined;
                this.canSave = true;
                this.dialogsService.error("Failed !", "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef)
            }
        );
    }

    // on update data
    onUpdateToDataBase(education: IEducation): void {
        // change timezone
        education = this.changeTimezone(education);
        // update data
        this.service.putKeyNumber(education, education.EducationId).subscribe(
            (complete: any) => {
                this.displayValue = complete;
                this.onSaveComplete();
            },
            (error: any) => {
                console.error(error);
                this.canSave = true;
                this.dialogsService.error("Failed !", "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef)
            }
        );
    }
}