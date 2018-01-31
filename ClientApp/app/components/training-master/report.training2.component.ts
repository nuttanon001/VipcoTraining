// angular
import { Component, OnInit, OnDestroy, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormGroup, FormControl } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
// class
import { ITrainingCousre } from "../../classes/training-cousre.class";
import { ITrainingMaster } from "../../classes/training-master.class";
import { ITrainingFilter } from "../../classes/training-filter.class";
import { ITrainingReport } from "../../classes/training-report.class";
import { IAttachFile } from "../../classes/attact-file.class";

// service
import { DialogsService } from "../../services/dialogs.service";
import { TrainingMasterService } from "../../services/training-master.service";
import { TrainingCourseService } from "../../services/training-course.service";
import { EmployeeService } from "../../services/employee.service";
import { LocationService } from "../../services/location.service";
// commuction
import { CousreDialogDataCommunicateService } from "../../communicates/course-dialog-data.communicate";
import { EmployeeDialogDataCommunicateService } from "../../communicates/employee-dialog-data.communicate";
// rxjs
import { Subscription } from "rxjs/Subscription";
// primeng
import { SelectItem } from "primeng/primeng";
// timezone
import * as moment from "moment-timezone";
@Component({
    selector: "report-training2",
    templateUrl: "./report.training2.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        TrainingCourseService,
        TrainingMasterService,
        EmployeeService,
        LocationService,
        CousreDialogDataCommunicateService,
        EmployeeDialogDataCommunicateService
    ]
})
/** report.training2 component*/
export class ReportTraining2Component implements OnInit, OnDestroy
{
    reportForm: FormGroup;
    //subscription: Subscription;
    reportParamater: ITrainingFilter;
    scrollHeight: string;
    courseName: string;
    employeeName: string;

    trainingReport: ITrainingReport;
    trainingReports: Array<ITrainingReport>;
    attachFiles: Array<IAttachFile>;

    columns: Array<any>;
    showCourse: boolean;
    showEmployee: boolean;
    /** report.training2 ctor */
    constructor(
        private service: TrainingMasterService,
        private servierTrainingCourse: TrainingCourseService,
        private serviceDialogCourse: CousreDialogDataCommunicateService,
        private serviceDialogEmployee: EmployeeDialogDataCommunicateService,
        private dialogsService: DialogsService,
        private route: ActivatedRoute,
        private fb: FormBuilder,
        private viewContainerRef: ViewContainerRef) { }

    /** Called by Angular after report.training2 component initialized */
    ngOnInit(): void {
        if (window.innerWidth >= 1600) {
            this.scrollHeight = 70 + "vh";
        } else if (window.innerWidth > 1360 && window.innerWidth < 1600) {
            this.scrollHeight = 65 + "vh";
        } else {
            this.scrollHeight = 60 + "vh";
        }

        this.route.params.subscribe((params: any) => {
            let key: string = params.condition; // params.["condition"];
            this.columns = new Array;
            this.trainingReports = new Array;
            this.attachFiles = new Array;

            if (key) {
                this.defineData();
            }
        }, error => console.error(error));
    }

    // on hook component
    ngOnDestroy(): void {
        // prevent memory leak when component destroyed
        //this.subscription.unsubscribe();
    }

    // define
    defineData(): void {
        this.showCourse = false;
        this.showEmployee = false;

        this.columns = [
            // { field: "CourseCode", header: "Code", style: { "width": "10%" }  },
            { field: "CourseName", header: "Course" },
            { field: "EmpCode", header: "EmpCode", style: { "width": "7%" } },
            { field: "NameThai", header: "Employee" },
            { field: "GroupName", header: "GroupName" },
            { field: "SectionName", header: "SectionName" },
            { field: "CourseDate", header: "Date", style: { "width": "10%" } },
            { field: "PassString", header: "Status", style: { "width": "7%" } },
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

        this.serviceDialogEmployee.ToParent$.subscribe(
            (selectEmployee) => {
                if (selectEmployee) {
                    this.reportForm.patchValue({
                        EmployeeCode: selectEmployee.EmpCode,
                        employeeName: "คุณ " + selectEmployee.NameThai,
                    });
                }
                this.showEmployee = !this.showEmployee;
            });
    }

    // build form
    buildForm(): void {
        this.reportParamater = {
            TrainingId: 0,
            EmployeeCode : "",
            GetTypeProgram: 1,
            GroupCode: "",
            LocateID: "",
            AfterDate: undefined
        };

        this.reportForm = this.fb.group({
            courseName: [this.courseName],
            employeeName: [this.employeeName],
            GetTypeProgram: [this.reportParamater.GetTypeProgram],
            GroupCode: [this.reportParamater.GroupCode],
            EmployeeCode: [this.reportParamater.EmployeeCode],
            LocateID: [this.reportParamater.LocateID],
            TrainingId: [this.reportParamater.TrainingId],
            AfterDate: [this.reportParamater.AfterDate]
        });

        this.reportForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
    }

    // on value of form change
    onValueChanged(data?: any): void {
        if (!this.reportForm) { return; }

        this.reportParamater = this.reportForm.value;
        if (!this.reportParamater.EmployeeCode) { return; }

        let zone: string = "Asia/Bangkok";
        if (this.reportParamater) {
            if (this.reportParamater.AfterDate) {
                this.reportParamater.AfterDate = moment.tz(this.reportParamater.AfterDate, zone).toDate();
            }
        }

        this.onGetDataTrainingMasterFromWebApi();
    }

    // get data training course from webapi
    onGetDataTrainingMasterFromWebApi(): void {
        if (this.attachFiles) {
            this.attachFiles = new Array;
        }

        this.service.getReportByEmployeeCode(this.reportParamater)
            .subscribe(dbDataBase => this.trainingReports = dbDataBase);
    }

    // row Track by
    rowTrackBy(index: number, row: any): any { return row.id; }

    // open course dialogs
    openDialog(condition:string): void {
        if (condition === "Course") {
            this.serviceDialogCourse.toChild("open");
            this.showCourse = !this.showCourse;
        } else {
            this.serviceDialogEmployee.toChild("Single");
            this.showEmployee = !this.showEmployee;
        }
    }

    // reset
    resetFilter(): void {
        this.trainingReports = new Array;
        this.attachFiles = new Array;
        this.buildForm();
    }

    // change row class
    customRowClass(rowData: any): string {
        if (rowData) {
            return rowData.Pass ? "enabled-row" : "disabled-row";
        } else {
            return "";
        }
    }

    // on row select
    onGetAttachFile(trainingReport: ITrainingReport): void {
        if (trainingReport && trainingReport.CourseId) {
            this.servierTrainingCourse.getAttachFile(trainingReport.CourseId)
                .subscribe(dbAttach => this.attachFiles = dbAttach,error => this.attachFiles = new Array);
        }
        else
        {
            this.attachFiles = new Array;
        }
    }

    // open attact file
    onOpenNewLink(link: string): void {
        if (link) {
            window.open(link, "_blank");
        }
    }

    // on click report
    reportTrainingMasterExcel() {

        if (this.reportParamater.EmployeeCode) {
            this.service.getReportEmployeeHistory(this.reportParamater.EmployeeCode)
                .subscribe(data => {
                    let link = document.createElement('a');
                    link.href = window.URL.createObjectURL(data);
                    link.download = "Report_" + this.reportParamater.EmployeeCode + ".xlsx";
                    link.click();
                },
                error => this.dialogsService.error("Error Message", "ไม่พบข้อมููลที่ต้องการ", this.viewContainerRef),
                () => console.log('Completed file download.'));
        }
    }
}