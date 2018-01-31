// angular
import { Component } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// class
import { ITrainingType } from "../../classes/training-type.class";
import { SelectItem } from "primeng/primeng";
// component
import { AbstractEditComponent } from "../abstract/abstract-edit.component";
// service
import { TrainingTypeService } from "../../services/training-type.service";
import { TrainingTypeCommunicateService } from "../../communicates/training-type.communicate"
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "training-type-edit",
    templateUrl: "./training-type-edit.component.html",
    styleUrls: ["../../styles/edit.style.scss"],
})

export class TrainingTypeEditComponent
    extends AbstractEditComponent<ITrainingType, TrainingTypeService> {
    typeParents: Array<SelectItem>;

    constructor(
        trainingTypeService: TrainingTypeService,
        trainingTypeServiceCom: TrainingTypeCommunicateService,
        private fb: FormBuilder,
    ) {
        super(
            trainingTypeService,
            trainingTypeServiceCom,
        );
    }

    // on get data by key
    onGetDataByKey(trainingType?: ITrainingType): void {
        if (trainingType) {
            this.service.getOneKeyNumber(trainingType.TrainingTypeId)
                .subscribe(dbTrainingLevel => {
                    this.editValue = dbTrainingLevel;
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                TrainingTypeId : 0,
                TrainingTypeName : ""
            };
            this.defineData()
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
        // Array
        this.getTrainingTypeArray();
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            TrainingTypeId: [this.editValue.TrainingTypeId],
            TrainingTypeName: [this.editValue.TrainingTypeName,
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
            TrainingTypeParentId: [this.editValue.TrainingTypeParentId],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate]
        });
        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
    }

    // array combobox typeParents
    getTrainingTypeArray(): void {
        this.service.getAll()
            .subscribe(result => {
                this.typeParents = new Array;
                this.typeParents.push({ label: "-", value: undefined });
                let result2 = result.filter((value, index) => {
                    return value.TrainingTypeId !== this.editValue.TrainingTypeId;
                });
                for (let item of result2) {
                    this.typeParents.push({ label: item.TrainingTypeName, value: item.TrainingTypeId });
                }
            }, error => console.error(error));
    }
}

