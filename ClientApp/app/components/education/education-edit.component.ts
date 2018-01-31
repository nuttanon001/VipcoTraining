// angular
import { Component } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// class
import { IEducation } from "../../classes/education.class";
// component
import { AbstractEditComponent } from "../abstract/abstract-edit.component";
// service
import { EducationService } from "../../services/education.service";
import { EducationCommunicateService } from "../../communicates/education.communicate"
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "education-edit",
    templateUrl: "./education-edit.component.html",
    styleUrls: ["../../styles/edit.style.scss"],
})

export class EducationEditComponent
    extends AbstractEditComponent<IEducation, EducationService> {

    constructor(
        educationService: EducationService,
        educationServiceCom: EducationCommunicateService,
        private fb: FormBuilder,
    ) {
        super(
            educationService,
            educationServiceCom,
        );
    }

    // on get data by key
    onGetDataByKey(education?: IEducation): void {
        if (education) {
            this.service.getOneKeyNumber(education.EducationId)
                .subscribe(dbEducation => {
                    this.editValue = dbEducation;
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                EducationId: 0,
                EducationName: ""
            }
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
            EducationId: [this.editValue.EducationId],
            EducationName: [this.editValue.EducationName,
                [
                    Validators.required,
                    Validators.minLength(1),
                    Validators.maxLength(200),
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

