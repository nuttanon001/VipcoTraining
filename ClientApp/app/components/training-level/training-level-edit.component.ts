// angular
import { Component } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// class
import { ITrainingLevel } from "../../classes/training-level.class";
// component
import { AbstractEditComponent } from "../abstract/abstract-edit.component";
// service
import { TrainingLevelService } from "../../services/training-level.service";
import { TrainingLevelCommunicateService } from "../../communicates/training-level.communicate"
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "training-level-edit",
    templateUrl: "./training-level-edit.component.html",
    styleUrls: ["../../styles/edit.style.scss"],
})

export class TrainingLevelEditComponent
    extends AbstractEditComponent<ITrainingLevel, TrainingLevelService> {

    constructor(
        trainingLevelService: TrainingLevelService,
        trainingLevelServiceCom: TrainingLevelCommunicateService,
        private fb: FormBuilder,
    ) {
        super(
            trainingLevelService,
            trainingLevelServiceCom,
        );
    }

    // on get data by key
    onGetDataByKey(trainingLevel?: ITrainingLevel): void {
        if (trainingLevel) {
            this.service.getOneKeyNumber(trainingLevel.TrainingLevelId)
                .subscribe(dbTrainingLevel => {
                    this.editValue = dbTrainingLevel;
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                TrainingLevel: "",
                TrainingLevelId: 0
            };
            this.defineData()
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            TrainingLevelId: [this.editValue.TrainingLevelId],
            TrainingLevel: [this.editValue.TrainingLevel,
                [
                    Validators.required,
                    Validators.minLength(1),
                    Validators.maxLength(50),
                ]
            ],
            Detail: [this.editValue.Detail,
                [
                    Validators.maxLength(200)
                ]
            ],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate]
        });
        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
    }
}

