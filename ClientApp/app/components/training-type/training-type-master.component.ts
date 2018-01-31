import { Component, ViewContainerRef } from "@angular/core";
// component
import { AbstractMasterNodeComponent } from "../abstract/abstract-master-node.component";
// classes
import { LazyLoad } from "../../classes/lazyload.class";
import { ITrainingType } from "../../classes/training-type.class";
import { LazyLoadEvent,TreeNode } from "primeng/primeng";
// service
import { DialogsService } from "../../services/dialogs.service";
import { TrainingTypeService } from "../../services/training-type.service";
// commuincate
import { TrainingTypeCommunicateService } from "../../communicates/training-type.communicate"
// timezone
import * as moment from "moment-timezone";

@Component({
    selector: "training-type-master",
    templateUrl: "./training-type-master.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        TrainingTypeService,
        TrainingTypeCommunicateService,
    ],
})

export class TrainingTypeMasterComponent
    extends AbstractMasterNodeComponent<ITrainingType, TrainingTypeService> {
    constructor(
        trainingTypeService: TrainingTypeService,
        trainingTypeComService: TrainingTypeCommunicateService,
        dialogsService: DialogsService,
        viewContainerRef: ViewContainerRef,
    ) {
        super(
            trainingTypeService,
            trainingTypeComService,
            dialogsService,
            viewContainerRef
        );
    }

    // on get data with lazy load
    onGetAll(): void {
        this.columns = [
            //{ field: "TrainingTypeId", header: "No", style: { 'width': '25%' }},
            { field: "TrainingTypeName", header: "Name type of training" }
        ];

        this.service.getFindAllForTree("")
            .subscribe(nodeData => {
                this.nodes = nodeData;
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(trainingType: ITrainingType): ITrainingType {
        var zone = "Asia/Bangkok";
        if (trainingType !== null) {
            if (trainingType.CreateDate !== null) {
                trainingType.CreateDate = moment.tz(trainingType.CreateDate, zone).toDate();
            }
            if (trainingType.ModifyDate !== null) {
                trainingType.ModifyDate = moment.tz(trainingType.ModifyDate, zone).toDate();
            }
        }
        return trainingType;
    }

    // on insert data
    onInsertToDataBase(trainingType: ITrainingType): void {
        // change timezone
        trainingType = this.changeTimezone(trainingType);
        // insert data
        this.service.post(trainingType).subscribe(
            (complete: any) => {
                //this.selectNode.data = complete;
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
    onUpdateToDataBase(trainingType: ITrainingType): void {
        // change timezone
        trainingType = this.changeTimezone(trainingType);
        // update data
        this.service.putKeyNumber(trainingType, trainingType.TrainingTypeId).subscribe(
            (complete: any) => {
                //this.selectNode.data = complete;
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