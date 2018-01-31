// angular
import { Component } from "@angular/core";
// class
import { ITrainingLevel } from "../../classes/training-level.class";
// component
import { AbstractViewComponent } from "../abstract/abstract-view.component";
// service
import { TrainingLevelService } from "../../services/training-level.service";
import { TrainingLevelCommunicateService } from "../../communicates/training-level.communicate"
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "training-level-view",
    templateUrl: "./training-level-view.component.html",
    styleUrls: ["../../styles/view.style.scss"],
})

export class TrainingLevelViewComponent
    extends AbstractViewComponent<ITrainingLevel, TrainingLevelService> {
    constructor(
        trainingLevelService: TrainingLevelService,
        trainingLevelComService: TrainingLevelCommunicateService,
    ) {
        super(
            trainingLevelService,
            trainingLevelComService,
        );
    }

    // on get data by key
    onGetDataByKey(trainingLevel: ITrainingLevel): void {
        // clear data
        this.displayValue = {
            TrainingLevelId: 0,
            TrainingLevel: "",
        };

        this.service.getOneKeyNumber(trainingLevel.TrainingLevelId)
            .subscribe(dbTrainingLevel => {
                this.displayValue = dbTrainingLevel;
            }, error => console.error(error));
    }
}