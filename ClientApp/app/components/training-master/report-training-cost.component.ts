// angular
import { Component, OnInit, OnDestroy, ViewContainerRef, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup, FormControl } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
// classes
import { ITrainingCousre } from "../../classes/training-cousre.class";
import { ITrainingFilter } from "../../classes/training-filter.class";
import { TrainingCost } from "../../classes/training-cost.class";
// service
import { DialogsService } from "../../services/dialogs.service";
import { TrainingMasterService } from "../../services/training-master.service";
import { TrainingCourseService } from "../../services/training-course.service";
// communicate
import { CousreDialogDataCommunicateService } from "../../communicates/course-dialog-data.communicate";
// rxjs
import { Subscription } from "rxjs/Subscription";
// primeng
import { SelectItem } from "primeng/primeng";
// timezone
import * as moment from "moment-timezone";

@Component({
    selector: "training-cost",
    templateUrl: "./report-training-cost.component.html",
    styleUrls: ["../../styles/master.style.scss", "../../styles/master-flax.style.scss"],
    providers: [
        TrainingCourseService,
        TrainingMasterService,
        CousreDialogDataCommunicateService,
    ]
})
/** report-training-cost component*/
export class ReportTrainingCostComponent implements OnInit
{
    reportForm: FormGroup;
    reportParamater: ITrainingFilter;
    scrollHeight: string;
    courseName: string;

    trainingCost: TrainingCost;
    trainingCosts: Array<TrainingCost>;

    columns: Array<any>;
    showCourse: boolean;
    /** report-training-cost ctor */
    constructor(
        private service: TrainingMasterService,
        private servierTrainingCourse: TrainingCourseService,
        private serviceDialogCourse: CousreDialogDataCommunicateService,
        private dialogsService: DialogsService,
        private route: ActivatedRoute,
        private fb: FormBuilder,
        private viewContainerRef: ViewContainerRef) { }

    /** Called by Angular after report-training-cost component initialized */
    ngOnInit(): void {
        if (window.innerWidth >= 1600) {
            this.scrollHeight = 68 + "vh";
        } else if (window.innerWidth > 1360 && window.innerWidth < 1600) {
            this.scrollHeight = 62 + "vh";
        } else {
            this.scrollHeight = 59 + "vh";
        }

        this.route.params.subscribe((params: any) => {
            let key: string = params.condition; // params.["condition"];
            this.columns = new Array;
            this.trainingCosts = new Array;

            if (key) {
                this.defineData();
            }
        }, error => console.error(error));
    }

    // define
    defineData(): void {
        this.showCourse = false;

        this.columns = [
            { field: "TrainingName", header: "Course" },
            { field: "TrainingDate", header: "Date", style: { "width": "15%" } },
            { field: "PeopleString", header: "People", style: { "width": "10%" } },
            { field: "CostString", header: "Cost", style: { "width": "10%" } },
            { field: "CostPerManString", header: "Per", style: { "width": "10%" } },
            { field: "Remark", header: "Remark", style: { "width": "10%" } },
        ];


        this.buildForm();

        this.serviceDialogCourse.ToParent$.subscribe(
            (selectedCouse) => {
                if (selectedCouse) {
                    console.log(selectedCouse);

                    this.reportForm.patchValue({
                        TrainingId: selectedCouse.TrainingCousreId,
                        courseName: selectedCouse.TrainingCousreCode + " : " + selectedCouse.TrainingCousreName,
                    });
                }
                this.showCourse = !this.showCourse;
            });
    }

    // build form
    buildForm(): void {
        this.reportParamater = {
            TrainingId: 0,
            EmployeeCode: "",
            GetTypeProgram: 1,
            GroupCode: "",
            LocateID: "",
            AfterDate: undefined,
            EndDate: undefined,

        };

        this.reportForm = this.fb.group({
            courseName: [this.courseName],
            TrainingId: [this.reportParamater.TrainingId],
            AfterDate: [this.reportParamater.AfterDate],
            EndDate:[this.reportParamater.EndDate]
        });

        this.reportForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
    }

    // on value of form change
    onValueChanged(data?: any): void {
        if (!this.reportForm) { return; }

        this.reportParamater = this.reportForm.value;
        if (!this.reportParamater.AfterDate || !this.reportParamater.EndDate) { return; }

        let zone: string = "Asia/Bangkok";
        if (this.reportParamater) {
            if (this.reportParamater.AfterDate) {
                //debug here
                console.log("Befor: ", this.reportParamater.AfterDate)

                this.reportParamater.AfterDate = moment.tz(this.reportParamater.AfterDate, zone).toDate();
                //debug here
                console.log("After: ", this.reportParamater.AfterDate)
            }

            if (this.reportParamater.EndDate) {
                this.reportParamater.EndDate = moment.tz(this.reportParamater.EndDate, zone).toDate();
            }
        }

        this.onGetDataTrainingCostFromWebApi();
    }

    // get data training course from webapi
    onGetDataTrainingCostFromWebApi(): void {

        this.service.getTrainingCostFromHistory(this.reportParamater)
            .subscribe(dbDataBase => this.trainingCosts = dbDataBase);
    }

    // row Track by
    rowTrackBy(index: number, row: any): any { return row.id; }

    // open course dialogs
    openDialog(condition: string): void {
        if (condition === "Course") {
            this.serviceDialogCourse.toChild("open");
            this.showCourse = !this.showCourse;
        }
    }

    // reset
    resetFilter(): void {
        this.trainingCosts = new Array;
        this.buildForm();
    }

    // on click report
    reportExcel() {
        if (this.reportParamater) {
            if (!this.reportParamater.AfterDate || !this.reportParamater.EndDate) { return; }

            this.service.getTrainingCostFromHistoryExcel(this.reportParamater)
                .subscribe(data => {
                    let link = document.createElement('a');
                    link.href = window.URL.createObjectURL(data);
                    link.download = "Report_Cost.xlsx";
                    link.click();
                },
                error => this.dialogsService.error("Error Message", "ไม่พบข้อมููลที่ต้องการ", this.viewContainerRef),
                () => console.log('Completed file download.'));
        }
    }
}