// angular
import { Component } from "@angular/core";
// class
import { ITrainingMaster } from "../../classes/training-master.class";
import { IAttachFile } from "../../classes/attact-file.class";
// component
import { AbstractViewComponent } from "../abstract/abstract-view.component";
// service
import { TrainingMasterService } from "../../services/training-master.service";
import { TrainingCourseService } from "../../services/training-course.service";
import { TrainingDetailService } from "../../services/training-detail.service";
import { TrainingMasterCommunicateService } from "../../communicates/training-master.communicate";
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "training-view",
    templateUrl: "./training-view.component.html",
    styleUrls: ["../../styles/view.style.scss"],
})

export class TrainingViewComponent
    extends AbstractViewComponent<ITrainingMaster, TrainingMasterService> {
    scrollHeight: string;
    columns: any;
    attachFiles: Array<IAttachFile> = new Array;

    constructor(
        trainingService: TrainingMasterService,
        private trainingDetailService: TrainingDetailService,
        private trainingCourseService: TrainingCourseService,
        trainingComService: TrainingMasterCommunicateService,
    ) {
        super(
            trainingService,
            trainingComService,
        );
    }

    // on get data by key
    onGetDataByKey(training: ITrainingMaster): void {
        if (!this.scrollHeight) {
            if (window.innerWidth >= 1600) {
                this.scrollHeight = 68 + "vh";
            } else if (window.innerWidth > 1360 && window.innerWidth < 1600) {
                this.scrollHeight = 63 + "vh";
            } else {
                this.scrollHeight = 58 + "vh";
            }
        }

        if (!this.columns) {
            this.columns = [
                { field: "EmployeeTraining", header: "Code", style: { "width": "15%" } },
                { field: "EmployeeNameString", header: "Employee" },
                { field: "Score", header: "Score" },
                { field: "StatusForTrainingString", header: "Status" },
            ];
        }

        // clear data
        this.displayValue = {
            TrainingMasterId : 0
        };

        this.service.getOneKeyNumber(training.TrainingMasterId)
            .subscribe(dbTrainingMaster => {
                // debug here
                // console.log(JSON.stringify(dbTrainingMaster));
                this.displayValue = dbTrainingMaster;
                if (this.displayValue.TrainingMasterId) {
                    this.trainingDetailService.getByMasterId(this.displayValue.TrainingMasterId)
                        .subscribe(dbTrainingDetail => this.displayValue.TblTrainingDetail = dbTrainingDetail.slice());
                }
                if (this.displayValue.TrainingCousreId) {
                    this.trainingCourseService.getAttachFile(this.displayValue.TrainingCousreId)
                        .subscribe(dbAttach => this.attachFiles = dbAttach);
                }

            }, error => console.error(error));
    }

    // row track
    rowTrackBy(index: number, row: any):any { return row.id; }

    // change row class
    customRowClass(rowData: any): string {
        if (rowData.StatusForTraining) {
            return rowData.StatusForTraining === 2 ? "disabled-row" : "enabled-row";
        } else {
            return "wait-row";
        }
    }
    // open attact file
    onOpenNewLink(link: string): void {
        if (link) {
            window.open(link, "_blank");
        }
    }
}