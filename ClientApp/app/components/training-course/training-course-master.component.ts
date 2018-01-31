import { Component, ViewContainerRef } from "@angular/core";
// component
import { AbstractMaster2ndEditionComponent } from "../abstract/abstract-master-2nd-edition.component";
// classes
import { ITrainingCousre } from "../../classes/training-cousre.class";
import { LazyLoad } from "../../classes/lazyload.class";
import { LazyLoadEvent } from "primeng/primeng";
// service
import { TrainingCourseService } from "../../services/training-course.service";
import { TrainingLevelService } from "../../services/training-level.service";
import { TrainingTypeService } from "../../services/training-type.service";
import { EducationService } from "../../services/education.service";
import { DialogsService } from "../../services/dialogs.service";
// commuincate
import { TrainingCousreCommunicateService } from "../../communicates/training-cousre.communicate"
// timezone
import * as moment from "moment-timezone";

@Component({
    selector: "training-course-master",
    templateUrl: "./training-course-master.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        TrainingCourseService,
        TrainingLevelService,
        TrainingTypeService,
        EducationService,
        TrainingCousreCommunicateService,
    ],
})

export class TrainingCourseMasterComponent
    extends AbstractMaster2ndEditionComponent<ITrainingCousre, TrainingCourseService> {
    constructor(
        trainingCourseService: TrainingCourseService,
        trainingCourseComService: TrainingCousreCommunicateService,
        dialogsService: DialogsService,
        viewContainerRef: ViewContainerRef,
    ) {
        super(
            trainingCourseService,
            trainingCourseComService,
            dialogsService,
            viewContainerRef
        );
    }

    // on get data with lazy load
    onGetAllWithLazyload(lazyload: LazyLoad): void {
        if (!this.columns) {
            this.columns = [
                { field: "TrainingCousreCode", header: "Code", style: { 'width': '15%' } },
                { field: "TrainingCousreName", header: "Name" },
                { field: "TrainingTypeString", header: "Type", style: { 'width': '15%' }},
                { field: "TrainingLevelString", header: "Level", style: { 'width': '10%' } },
                { field: "EducationRequirementString", header: "Education", style: { 'width': '15%' }},
            ];
        }

        this.service.getAllWithLazyLoad(lazyload)
            .subscribe(restData => {
                this.values = restData.Data;
                this.totalRow = restData.TotalRow;
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(trainingCourse: ITrainingCousre): ITrainingCousre {
        var zone = "Asia/Bangkok";
        if (trainingCourse !== null) {
            if (trainingCourse.CreateDate !== null) {
                /*
                *
                *
                *
                *
                *
                *
                May be worng date timezone come to check it
                trainingCourse.CreateDate = moment.tz(trainingCourse.CreateDate, zone).format();
                *
                *
                *
                *
                *
                */
                trainingCourse.CreateDate = moment.tz(trainingCourse.CreateDate, zone).toDate();
            }
            if (trainingCourse.ModifyDate !== null) {
                trainingCourse.ModifyDate = moment.tz(trainingCourse.ModifyDate, zone).toDate();
            }
        }
        return trainingCourse;
    }

    // on insert data
    onInsertToDataBase(trainingCourse: ITrainingCousre): void {
        let attachs:FileList|undefined = trainingCourse.AttachFile;
        // change timezone
        trainingCourse = this.changeTimezone(trainingCourse);
        // insert data
        this.service.post(trainingCourse).subscribe(
            (complete: ITrainingCousre) => {
                if (complete && attachs) {
                    this.onAttactFileToDataBase(complete.TrainingCousreId, attachs);
                }
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
    onUpdateToDataBase(trainingCourse: ITrainingCousre): void {
        let attachs: FileList | undefined = trainingCourse.AttachFile;
        // remove attach
        if (trainingCourse.RemoveAttach) {
            this.onRemoveFileFromDataBase(trainingCourse.RemoveAttach);
        }
        // change timezone
        trainingCourse = this.changeTimezone(trainingCourse);
        // update data
        this.service.putKeyNumber(trainingCourse, trainingCourse.TrainingCousreId).subscribe(
            (complete: any) => {
                if (complete && attachs) {
                    this.onAttactFileToDataBase(complete.TrainingCousreId, attachs);
                }
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

    // on attact file
    onAttactFileToDataBase(TrainCourseId:number, Attacts:FileList): void {
        this.service.postAttactFile(TrainCourseId, Attacts)
            .subscribe(complate => console.log("Upload Complate"), error => console.error(error));
    }

    // on remove file
    onRemoveFileFromDataBase(Attachs: Array<number>): void {
        Attachs.forEach((value: number) => {
            this.service.deleteAttactFile(value)
                .subscribe(complate => console.log("Delete Complate"), error => console.error(error));
        });
    }

    onTest() {
        this.service.testMethod();
    }
}