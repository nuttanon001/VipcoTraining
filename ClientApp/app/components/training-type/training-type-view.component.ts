// angular
import { Component } from "@angular/core";
// class
import { ITrainingType } from "../../classes/training-type.class";
// component
import { AbstractViewComponent } from "../abstract/abstract-view.component";
// service
import { TrainingTypeService } from "../../services/training-type.service";
import { TrainingTypeCommunicateService } from "../../communicates/training-type.communicate"
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "training-type-view",
    templateUrl: "./training-type-view.component.html",
    styleUrls: ["../../styles/view.style.scss"],
})

export class TrainingTypeViewComponent
    extends AbstractViewComponent<ITrainingType, TrainingTypeService> {

    constructor(
        trainingTypeService: TrainingTypeService,
        trainingTypeComService: TrainingTypeCommunicateService,
    ) {
        super(
            trainingTypeService,
            trainingTypeComService,
        );
    }

    // on get data by key
    onGetDataByKey(trainingType: ITrainingType): void {
        // clear data
        this.displayValue = {
            TrainingTypeId: 0,
            TrainingTypeName: ""
        };

        this.service.getOneKeyNumber(trainingType.TrainingTypeId)
            .subscribe(dbTrainingLevel => {
                this.displayValue = dbTrainingLevel;
            }, error => console.error(error));
    }
}