import { Component, ViewContainerRef } from "@angular/core";
// component
import { AbstractMaster2ndEditionComponent } from "../abstract/abstract-master-2nd-edition.component";
// classes
import { ITrainingMaster } from "../../classes/training-master.class";
import { LazyLoad } from "../../classes/lazyload.class";
import { LazyLoadEvent } from "primeng/primeng";
// service
import { TrainingMasterService } from "../../services/training-master.service";
import { TrainingCourseService } from "../../services/training-course.service";
import { EmployeeService } from "../../services/employee.service";
import { TrainingDetailService } from "../../services/training-detail.service";
import { LocationService } from "../../services/location.service";
import { DialogsService } from "../../services/dialogs.service";
// commuincate
import { TrainingMasterCommunicateService } from "../../communicates/training-master.communicate";
import { CousreDialogDataCommunicateService } from "../../communicates/course-dialog-data.communicate";
import { EmployeeDialogDataCommunicateService } from "../../communicates/employee-dialog-data.communicate";
// timezone
import * as moment from "moment-timezone";

@Component({
    selector: "training-master",
    templateUrl: "./training-master.component.html",
    styleUrls: ["../../styles/master.style.scss", "../../styles/master-flax.style.scss"],
    providers: [
        TrainingMasterService,
        TrainingMasterCommunicateService,
        EmployeeService,
        EmployeeDialogDataCommunicateService,
        TrainingCourseService,
        CousreDialogDataCommunicateService,
        TrainingDetailService,
        LocationService,
        DialogsService
    ],
})

export class TrainingMasterComponent
    extends AbstractMaster2ndEditionComponent<ITrainingMaster, TrainingMasterService> {

    files: FileList;

    constructor(
        trainingService: TrainingMasterService,
        trainingComService: TrainingMasterCommunicateService,
        dialogsService: DialogsService,
        viewContainerRef: ViewContainerRef,
    ) {
        super(
            trainingService,
            trainingComService,
            dialogsService,
            viewContainerRef
        );
    }

    // on get data with lazy load
    onGetAllWithLazyload(lazyload: LazyLoad): void {
        if (!this.columns) {
            this.columns = [
                { field: "TrainingCode", header: "TrainingCode", style: { 'width': '15%' } },
                { field: "TrainingName", header: "TrainingName" },
                { field: "TrainingDateString", header: "Date", style: { 'width': '20%' } },
                { field: "LecturerName", header: "Lecturer" },
            ];
        }

        this.service.getAllWithLazyLoad(lazyload)
            .subscribe(restData => {
                this.values = restData.Data;
                this.totalRow = restData.TotalRow;
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(training: ITrainingMaster): ITrainingMaster {
        var zone = "Asia/Bangkok";
        if (training) {
            if (training.CreateDate) {
                training.CreateDate = moment.tz(training.CreateDate, zone).toDate();
            }
            if (training.ModifyDate) {
                training.ModifyDate = moment.tz(training.ModifyDate, zone).toDate();
            }
            if (training.TrainingDate) {
                training.TrainingDate = moment.tz(training.TrainingDate, zone).toDate();
            }
            if (training.TrainingDateEnd) {
                training.TrainingDateEnd = moment.tz(training.TrainingDateEnd, zone).toDate();
            }
            if (training.TblTrainingDetail) {
                training.TblTrainingDetail.forEach((value, index) => {
                    if (value.CreateDate) {
                        value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
                    }
                    if (value.ModifyDate) {
                        value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
                    }

                    if (training.TblTrainingDetail)
                        training.TblTrainingDetail[index] = value;
                });
            }
        }
        return training;
    }

    // on insert data
    onInsertToDataBase(training: ITrainingMaster): void {
        // change timezone
        training = this.changeTimezone(training);
        // insert data
        this.service.post(training).subscribe(
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
    onUpdateToDataBase(training: ITrainingMaster): void {
        // change timezone
        training = this.changeTimezone(training);
        // update data
        this.service.putKeyNumber(training, training.TrainingMasterId).subscribe(
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
    reportTrainingMasterExcel(training?: ITrainingMaster) {
        if (training) {
            this.service.getReportByTrainingMaster(training.TrainingMasterId)
                .subscribe(data => {
                    let link = document.createElement('a');
                    link.href = window.URL.createObjectURL(data);
                    link.download = "report_" + training.TrainingCode + ".xlsx";
                    link.click();
                },
                error => this.dialogsService.error("Error Message", "ไม่พบข้อมููลที่ต้องการ", this.viewContainerRef),
                () => console.log('Completed file download.'));
        }
    }
}