import { Component, ViewContainerRef } from "@angular/core";
// component
import { AbstractMasterComponent } from "../abstract/abstract-master.component";
// classes
import { ITrainingLevel } from "../../classes/training-level.class";
import { LazyLoad } from "../../classes/lazyload.class";
import { LazyLoadEvent } from "primeng/primeng";
// service
import { DialogsService } from "../../services/dialogs.service";
import { TrainingLevelService } from "../../services/training-level.service";
// commuincate
import { TrainingLevelCommunicateService } from "../../communicates/training-level.communicate"
// timezone
import * as moment from "moment-timezone";

@Component({
    selector: "training-level-master",
    templateUrl: "./training-level-master.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        TrainingLevelService,
        TrainingLevelCommunicateService,
    ],
})

export class TrainingLevleMasterComponent
    extends AbstractMasterComponent<ITrainingLevel, TrainingLevelService> {
    constructor(
        trainingLevelService: TrainingLevelService,
        trainingLevelComService: TrainingLevelCommunicateService,
        dialogsService: DialogsService,
        viewContainerRef: ViewContainerRef,
    ) {
        super(
            trainingLevelService,
            trainingLevelComService,
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
    changeTimezone(trainingLevel: ITrainingLevel): ITrainingLevel {
        var zone = "Asia/Bangkok";
        if (trainingLevel !== null) {
            if (trainingLevel.CreateDate !== null) {
                trainingLevel.CreateDate = moment.tz(trainingLevel.CreateDate, zone).toDate();
            }
            if (trainingLevel.ModifyDate !== null) {
                trainingLevel.ModifyDate = moment.tz(trainingLevel.ModifyDate, zone).toDate();
            }
        }
        return trainingLevel;
    }

    // on insert data
    onInsertToDataBase(trainingLevel: ITrainingLevel): void {
        // change timezone
        trainingLevel = this.changeTimezone(trainingLevel);
        // insert data
        this.service.post(trainingLevel).subscribe(
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
    onUpdateToDataBase(trainingLevel: ITrainingLevel): void {
        // change timezone
        trainingLevel = this.changeTimezone(trainingLevel);
        // update data
        this.service.putKeyNumber(trainingLevel, trainingLevel.TrainingLevelId).subscribe(
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