// angular
import { Component } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// class
import { ITrainingCousre } from "../../classes/training-cousre.class";
import { IAttachFile } from "../../classes/attact-file.class";
import { SelectItem } from "primeng/primeng";
// component
import { AbstractEditComponent } from "../abstract/abstract-edit.component";
// service
import { TrainingCourseService } from "../../services/training-course.service"
import { TrainingLevelService } from "../../services/training-level.service"
import { TrainingTypeService } from "../../services/training-type.service"
import { EducationService } from "../../services/education.service"

import { TrainingCousreCommunicateService } from "../../communicates/training-cousre.communicate"
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "training-course-edit",
    templateUrl: "./training-course-edit.component.html",
    styleUrls: ["../../styles/edit.style.scss"],
})

export class TrainingCourseEditComponent
    extends AbstractEditComponent<ITrainingCousre, TrainingCourseService> {
    trainTypes: Array<SelectItem>;
    trainLevels: Array<SelectItem>;
    educations: Array<SelectItem>;
    attachFiles: Array<IAttachFile> = new Array;

    constructor(
        trainingCourseService: TrainingCourseService,
        trainingCourseServiceCom: TrainingCousreCommunicateService,
        private trainingLevelService: TrainingLevelService,
        private trainingTypeService: TrainingTypeService,
        private educationService: EducationService,
        private fb: FormBuilder,
    ) {
        super(
            trainingCourseService,
            trainingCourseServiceCom,
        );
    }

    // on get data by key
    onGetDataByKey(trainingCourse?: ITrainingCousre): void {
        if (trainingCourse) {
            this.service.getOneKeyNumber(trainingCourse.TrainingCousreId)
                .subscribe(dbTrainingCourse => {
                    this.editValue = dbTrainingCourse;
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                TrainingCousreId: 0,
                TrainingCousreCode: "",
                TrainingCousreName: ""
            };
            this.defineData()
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
        // Array
        this.getTrainingTypeArray();
        this.getTrainingLevelArray();
        this.getEducationArray();
        this.getAttach();
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            TrainingCousreId: [this.editValue.TrainingCousreId],
            TrainingCousreCode: [this.editValue.TrainingCousreCode,
                [
                    Validators.required,
                    Validators.minLength(1),
                    Validators.maxLength(50),
                ]
            ],
            TrainingLevelId: [this.editValue.TrainingLevelId],
            TrainingTypeId: [this.editValue.TrainingTypeId],
            TrainingCousreName: [this.editValue.TrainingCousreName,
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
            EducationRequirementId: [this.editValue.EducationRequirementId],
            WorkExperienceRequirement: [this.editValue.WorkExperienceRequirement],
            MinimunScore: [this.editValue.MinimunScore],
            Remark: [this.editValue.Remark,
                [
                    Validators.maxLength(200)
                ]
            ],
            BaseCost: [this.editValue.BaseCost],
            Status: [this.editValue.Status],
            TrainingDurationHour: [this.editValue.TrainingDurationHour],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            AttachFile: [this.editValue.AttachFile],
            RemoveAttach: [this.editValue.RemoveAttach],
        });
        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
    }

    // array combobox TrainingType
    getTrainingTypeArray(): void {
        this.trainingTypeService.getAll()
            .subscribe(result => {
                this.trainTypes = new Array;
                this.trainTypes.push({ label: "-", value: undefined });
                for (let item of result) {
                    this.trainTypes.push({ label: item.TrainingTypeName, value: item.TrainingTypeId });
                }
            }, error => console.error(error));
    }
    // array combobox TrainingLevel
    getTrainingLevelArray(): void {
        this.trainingLevelService.getAll()
            .subscribe(result => {
                this.trainLevels = new Array;
                this.trainLevels.push({ label: "-", value: undefined });
                for (let item of result) {
                    this.trainLevels.push({ label: item.TrainingLevel, value: item.TrainingLevelId });
                }
            }, error => console.error(error));
    }
    // array combobox Education
    getEducationArray(): void {
        this.educationService.getAll()
            .subscribe(result => {
                this.educations = new Array;
                this.educations.push({ label: "-", value: undefined });
                for (let item of result) {
                    this.educations.push({ label: item.EducationName, value: item.EducationId });
                }
            }, error => console.error(error));
    }
    // get attact file
    getAttach(): void {
        if (this.editValue && this.editValue.TrainingCousreId > 0) {
            this.service.getAttachFile(this.editValue.TrainingCousreId)
                .subscribe(dbAttach => {
                    this.attachFiles = dbAttach.slice();
                },error => console.error(error));
        }
    }
    // has attact file
    updateResults(results: FileList): void {
        this.editValue.AttachFile = results;
        this.editValueForm.patchValue({
            AttachFile: this.editValue.AttachFile
        });
        this.onValueChanged(undefined);
        // console.log("results:", this.files); // uncomment to take a look
    }
    // delete file attach
    onDeleteAttachFile(attach: IAttachFile) {
        if (attach) {
            if (!this.editValue.RemoveAttach) {
                this.editValue.RemoveAttach = new Array;
            }
            // remove
            this.editValue.RemoveAttach.push(attach.AttactId);
            this.editValueForm.patchValue({
                RemoveAttach: this.editValue.RemoveAttach
            });
            let template: Array<IAttachFile> =
                this.attachFiles.filter((e: IAttachFile) => e.AttactId !== attach.AttactId);

            this.attachFiles = new Array();
            setTimeout(() => this.attachFiles = template.slice(), 50);

            this.onValueChanged(undefined);
        }
    }
    // open file attach
    onOpenNewLink(link: string): void {
        if (link) {
            window.open(link, "_blank");
        }
    }
}

