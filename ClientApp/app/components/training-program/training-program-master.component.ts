import { Component, ViewContainerRef } from "@angular/core";
// component
import { AbstractMaster2ndEditionComponent } from "../abstract/abstract-master-2nd-edition.component";
// classes
import { ITrainingProgram } from "../../classes/training-program.class";
import { IBasicCourse } from "../../classes/basic-course.class";
import { IStandardCourse } from "../../classes/standard-course.class";
import { ISupplementCourse } from "../../classes/supplement-course.class";
import { IProgramHasPosition } from "../../classes/program-has-position.class";
import { IProgramHasGroup } from "../../classes/program-has-group.class";
import { LazyLoad } from "../../classes/lazyload.class";
import { LazyLoadEvent } from "primeng/primeng";
// service
import { TrainingProgramService } from "../../services/training-program.service";
import { TrainingCourseService } from "../../services/training-course.service";
import { BasicCourseService } from "../../services/basic-course.service";
import { SupplementCourseService } from "../../services/supplement-course.service";
import { StandardCourseService } from "../../services/standard-course.service";
import { ProgramHasPositionService } from "../../services/program-has-position.service";
import { ProgramHasGroupService } from "../../services/program-has-group.service";
import { PositionService } from "../../services/position.service";
import { GroupService } from "../../services/group.service";
import { DialogsService } from "../../services/dialogs.service";
// commuincate
import { TrainingProgramCommunicateService } from "../../communicates/training-program.communicate";
import { CousreDialogDataCommunicateService } from "../../communicates/course-dialog-data.communicate";
import { PositionDialogDataCommunicateService } from "../../communicates/position-dialog-data.communicate";
import { GroupDialogDataCommunicateService } from "../../communicates/group-dialog-data.communiate";
// timezone
import * as moment from "moment-timezone";

@Component({
    selector: "training-program-master",
    templateUrl: "./training-program-master.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        TrainingProgramService, TrainingCourseService,
        BasicCourseService, SupplementCourseService,
        StandardCourseService, ProgramHasPositionService,
        PositionService, DialogsService, GroupService,
        ProgramHasGroupService,
        TrainingProgramCommunicateService, CousreDialogDataCommunicateService,
        PositionDialogDataCommunicateService, GroupDialogDataCommunicateService
    ],
})

export class TrainingProgramMasterComponent
    extends AbstractMaster2ndEditionComponent<ITrainingProgram, TrainingProgramService> {
    constructor(
        trainingProgramService: TrainingProgramService,
        trainingProgramComService: TrainingProgramCommunicateService,
        dialogsService: DialogsService,
        viewContainerRef: ViewContainerRef,
    ) {
        super(
            trainingProgramService,
            trainingProgramComService,
            dialogsService,
            viewContainerRef
        );
    }

    // on get data with lazy load
    onGetAllWithLazyload(lazyload: LazyLoad): void {
        if (!this.columns) {
            this.columns = [
                { field: "TrainingProgramCode", header: "Code", style: { 'width': '15%' }},
                { field: "TrainingProgramName", header: "Name" },
                { field: "Detail", header: "Detail" },
                { field: "TrainingProgramLevelString", header: "Level", style: { 'width': '20%' }},
            ];
        }

        this.service.getAllWithLazyLoad(lazyload)
            .subscribe(restData => {
                // debug here
                // console.log("Data:", JSON.stringify(restData.Data));

                this.values = restData.Data;
                this.totalRow = restData.TotalRow;
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(trainingProgram: ITrainingProgram): ITrainingProgram {
        var zone = "Asia/Bangkok";
        if (trainingProgram !== null) {
            if (trainingProgram.CreateDate !== null) {
                trainingProgram.CreateDate = moment.tz(trainingProgram.CreateDate, zone).toDate();
            }
            if (trainingProgram.ModifyDate !== null) {
                trainingProgram.ModifyDate = moment.tz(trainingProgram.ModifyDate, zone).toDate();
            }

            if (trainingProgram.TblBasicCourse) {
                trainingProgram.TblBasicCourse.forEach((value, index) => {
                    if (value.CreateDate !== null) {
                        value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
                    }
                    if (value.ModifyDate !== null) {
                        value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
                    }
                    // can't update FromBody with same data from webapi try new object and send back update
                    if (trainingProgram.TblBasicCourse) {
                        let newData:IBasicCourse = {
                            BasicCourseId : value.BasicCourseId,
                            CreateDate : value.CreateDate,
                            Creator : value.Creator,
                            ModifyDate : value.ModifyDate,
                            Modifyer : value.Modifyer,
                            TrainingCousreId : value.TrainingCousreId,
                            TrainingProgramId : value.TrainingProgramId,
                        };
                        trainingProgram.TblBasicCourse[index] = newData;
                    }
                });
            }

            if (trainingProgram.TblStandardCourse) {
                trainingProgram.TblStandardCourse.forEach((value, index) => {
                    if (value.CreateDate !== null) {
                        value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
                    }
                    if (value.ModifyDate !== null) {
                        value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
                    }

                    if (trainingProgram.TblStandardCourse) {
                        // can't update FromBody with same data from webapi try new object and send back update
                        let newData: IStandardCourse = {
                            StandardCourseId : value.StandardCourseId,
                            CreateDate : value.CreateDate,
                            Creator : value.Creator,
                            ModifyDate : value.ModifyDate,
                            Modifyer : value.Modifyer,
                            TrainingCousreId : value.TrainingCousreId,
                            TrainingProgramId : value.TrainingProgramId,
                        };
                        trainingProgram.TblStandardCourse[index] = newData;
                    }
                });
            }

            if (trainingProgram.TblSupplementCourse) {
                trainingProgram.TblSupplementCourse.forEach((value, index) => {
                    if (value.CreateDate !== null) {
                        value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
                    }
                    if (value.ModifyDate !== null) {
                        value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
                    }
                    if (trainingProgram.TblSupplementCourse) {
                        // can't update FromBody with same data from webapi try new object and send back update
                        let newData:ISupplementCourse = {
                            SupplermentCourseId : value.SupplermentCourseId,
                            CreateDate : value.CreateDate,
                            Creator : value.Creator,
                            ModifyDate : value.ModifyDate,
                            Modifyer : value.Modifyer,
                            TrainingCousreId : value.TrainingCousreId,
                            TrainingProgramId : value.TrainingProgramId,
                        };
                        trainingProgram.TblSupplementCourse[index] = newData;
                    }
                });
            }

            if (trainingProgram.TblTrainingProgramHasGroup) {
                trainingProgram.TblTrainingProgramHasGroup.forEach((value, index) => {
                    if (value.CreateDate !== null) {
                        value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
                    }
                    if (value.ModifyDate !== null) {
                        value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
                    }
                    if (trainingProgram.TblTrainingProgramHasGroup)
                    {
                        // can't update FromBody with same data from webapi try new object and send back update
                        let newData: IProgramHasGroup = {
                            ProgramHasGroupId: value.ProgramHasGroupId,
                            TrainingProgramId: value.TrainingProgramId,
                            GroupCode: value.GroupCode,
                            CreateDate: value.CreateDate,
                            Creator: value.Creator,
                            ModifyDate: value.ModifyDate,
                            Modifyer: value.Modifyer,
                        };
                        trainingProgram.TblTrainingProgramHasGroup[index] = newData;
                    }
                });
            }

            if (trainingProgram.TblTrainingProgramHasPosition) {
                trainingProgram.TblTrainingProgramHasPosition.forEach((value, index) => {
                    if (value.CreateDate !== null) {
                        value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
                    }
                    if (value.ModifyDate !== null) {
                        value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
                    }
                    if (trainingProgram.TblTrainingProgramHasPosition) {
                        let newData: IProgramHasPosition = {
                            ProgramHasPositionId: value.ProgramHasPositionId,
                            TrainingProgramId: value.TrainingProgramId,
                            PositionCode: value.PositionCode,
                            CreateDate: value.CreateDate,
                            Creator: value.Creator,
                            ModifyDate: value.ModifyDate,
                            Modifyer: value.Modifyer,
                        };
                        trainingProgram.TblTrainingProgramHasPosition[index] = newData;
                    }
                });
            }
        }
        return trainingProgram;
    }

    // on insert data
    onInsertToDataBase(trainingProgram: ITrainingProgram): void {
        // change timezone
        trainingProgram = this.changeTimezone(trainingProgram);
        // insert data
        this.service.post(trainingProgram).subscribe(
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
    onUpdateToDataBase(trainingProgram: ITrainingProgram): void {
        // debug here
        //console.log("Befor :" + JSON.stringify(trainingProgram));
        // change timezone
        trainingProgram = this.changeTimezone(trainingProgram);
        // debug here
        //console.log("After :" + JSON.stringify(trainingProgram));
        // update data
        this.service.putKeyNumber(trainingProgram, trainingProgram.TrainingProgramId).subscribe(
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

    // on click report
    reportTrainingProgramExcel(trainingProgram?: ITrainingProgram) {
        if (trainingProgram) {
            this.service.getReportByTrainingProgramWithPosition(trainingProgram.TrainingProgramId)
                .subscribe(data => {
                    let link = document.createElement('a');
                    link.href = window.URL.createObjectURL(data);
                    link.download = "report_" + trainingProgram.TrainingProgramCode + ".xlsx";
                    link.click();
                },
                error => this.dialogsService.error("Error Message", "ไม่พบข้อมููลที่ต้องการ", this.viewContainerRef),
                () => console.log('Completed file download.'));
        }
    }

    reportTrainingProgram2Excel(trainingProgram?: ITrainingProgram) {
        if (trainingProgram) {
            this.service.getReportByTrainingProgramWithGroup(trainingProgram.TrainingProgramId)
                .subscribe(data => {
                    let link = document.createElement('a');
                    link.href = window.URL.createObjectURL(data);
                    link.download = "report_" + trainingProgram.TrainingProgramCode + ".xlsx";
                    link.click();
                },
                error => this.dialogsService.error("Error Message", "ไม่พบข้อมููลที่ต้องการ", this.viewContainerRef),
                () => console.log('Completed file download.'));
        }
    }
}