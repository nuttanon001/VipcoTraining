// angular
import { Component, OnInit, OnDestroy, ViewContainerRef, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup, FormControl, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
// class
import { ITrainingCousre } from "../../classes/training-cousre.class";
import { ITrainingMaster } from "../../classes/training-master.class";
import { ITrainingFilter } from "../../classes/training-filter.class";
import { ITrainingReport } from "../../classes/training-report.class";
// service
import { LocationService } from "../../services/location.service";
import { GroupService } from "../../services/group.service";
import { DialogsService } from "../../services/dialogs.service";
import { PositionService } from "../../services/position.service";
import { TrainingMasterService } from "../../services/training-master.service";
import { TrainingCourseService } from "../../services/training-course.service";
import { TrainingProgramService } from "../../services/training-program.service";
// commuction
import { TrainingMasterCommunicateService } from "../../communicates/training-master.communicate";
import { CousreDialogDataCommunicateService } from "../../communicates/course-dialog-data.communicate";
import { PositionDialogDataCommunicateService } from "../../communicates/position-dialog-data.communicate";
import { ProgramDialogDataCommunicateService } from "../../communicates/program-dialog-data.communicate";
// rxjs
import { Subscription } from "rxjs/Subscription";
// primeng
import { SelectItem } from "primeng/primeng";
// timezone
import * as moment from "moment-timezone";

@Component({
    selector: "report-training",
    templateUrl: "./report-training.component.html",
    styleUrls: ["../../styles/master.style.scss", "../../styles/master-flax.style.scss"],
    providers: [
        LocationService, GroupService, PositionService,
        TrainingMasterService,
        TrainingCourseService,
        TrainingProgramService,
        TrainingMasterCommunicateService,
        CousreDialogDataCommunicateService,
        ProgramDialogDataCommunicateService,
        PositionDialogDataCommunicateService
    ]
})

export class ReportTrainingComponent implements OnInit, OnDestroy {
    reportForm: FormGroup;
    subscription: Subscription;
    reportParamater: ITrainingFilter;
    condition: string;
    stringName: string;
    stringPosition: string;
    scrollHeight: string;
    sortName: string;

    trainingReport: ITrainingReport;
    trainingReports: Array<ITrainingReport>;
    templateReports: Array<ITrainingReport>;

    groups: Array<SelectItem>;
    locations: Array<SelectItem>;
    typeReport: Array<SelectItem>;

    columns: Array<any>;
    showCourse: boolean;
    showProgram: boolean;
    showPosition: boolean;

    constructor(
        private service: TrainingMasterService,
        private locationService: LocationService,
        private groupService: GroupService,
        private programService: TrainingProgramService,
        private serviceCom: TrainingMasterCommunicateService,
        private serviceDialogPosition: PositionDialogDataCommunicateService,
        private serviceDialogCourse: CousreDialogDataCommunicateService,
        private serviceDialogProgram: ProgramDialogDataCommunicateService,
        private dialogsService: DialogsService,
        private route: ActivatedRoute,
        private fb: FormBuilder,
        private viewContainerRef: ViewContainerRef
    ) { }

    // on hook component
    ngOnInit(): void {
        if (window.innerWidth >= 1600) {
            this.scrollHeight = 70 + "vh";
        } else if (window.innerWidth > 1360 && window.innerWidth < 1600) {
            this.scrollHeight = 65 + "vh";
        } else {
            this.scrollHeight = 60 + "vh";
        }

        this.subscription = this.serviceCom.ToSender$.subscribe(
            (condition: any) => {
                this.condition = condition;
                this.defineData();
            });

        this.route.params.subscribe((params: any) => {
            let key: string = params.condition; // params.["condition"];
            // debug here
            console.log(key);
            this.columns = new Array;
            this.trainingReports = new Array;

            if (key) {
                this.condition = key;
                this.defineData();
            }
        }, error => console.error(error));
    }

    // on hook component
    ngOnDestroy(): void {
        // prevent memory leak when component destroyed
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    // get location array
    getLocationArray(): void {
        this.locationService.getAll()
            .subscribe(result => {
                this.locations = new Array;
                this.locations.push({ label: "Selected location of employee", value: "" });
                for (let item of result) {
                    this.locations.push({ label: item.LocateDesc, value: item.LocateId });
                }
            }, error => console.error(error));
    }

    // get group array
    getGroupArray(): void {
        this.groupService.getAll()
            .subscribe(result => {
                this.groups = new Array;
                this.groups.push({ label: "Selected group name of employee", value: "" });
                for (let item of result) {
                    this.groups.push({ label: item.GroupDesc || "", value: item.GroupCode });
                }
            }, error => console.error(error));
    }

    // define
    defineData(): void {
        this.showCourse = false;
        this.showProgram = false;
        this.showPosition = false;
        if (this.condition === "Course") {
            this.sortName = "CourseName";

            this.columns = [
                // { field: "CourseCode", header: "Code", style: { "width": "10%" }  },
                { field: "CourseName", header: "Course" },
                { field: "EmpCode", header: "EmpCode", style: { "width": "5%" } },
                { field: "NameThai", header: "Employee" },
                { field: "GroupName", header: "GroupName" },
                { field: "SectionName", header: "SectionName" },
                { field: "CourseDate", header: "Date", style: { "width": "5%" } },
                { field: "PassString", header: "Status", style: { "width": "10%" } },

            ];
        } else {
            this.sortName = "TypeProgram";

            this.columns = [
                { field: "TypeProgram", header: "Type" },
                { field: "CourseName", header: "Course" },
                { field: "EmpCode", header: "EmpCode", style: { "width": "5%" } },
                { field: "NameThai", header: "Employee" },
                { field: "PositionName", header: "Position" },
                { field: "GroupName", header: "GroupName" },
                { field: "SectionName", header: "SectionName" },
                { field: "CourseDate", header: "Date", style: { "width": "5%" } },
                { field: "PassString", header: "Status", style: { "width": "10%" } },

            ];
        }

        this.buildForm();

        this.getLocationArray();
        this.getGroupArray();

        this.typeReport = new Array;
        if (this.condition === "Course") {
            this.typeReport.push(
                { label: "ผู้ผ่านการอบรม", value: 1 },
                { label: "ผู้ไม่ผ่านการอบรม", value: 2 }
            );
        } else {
            this.typeReport.push(
                { label: "ผู้ผ่านการอบรม", value: 1 },
                { label: "ผู้ไม่ผ่านการอบรม", value: 2 },
                { label: "ทั้งหมด", value: 3 }
            );
        }

        this.serviceDialogCourse.ToParent$.subscribe(
            (selectedCouse) => {
                if (selectedCouse) {
                    console.log(selectedCouse);

                    this.reportForm.patchValue({
                        TrainingId: selectedCouse.TrainingCousreId,
                        stringName: selectedCouse.TrainingCousreName,
                    });
                }
                this.showCourse = !this.showCourse;
            });

        this.serviceDialogProgram.ToParent$.subscribe(
            (selectProgram) => {
                if (selectProgram) {
                    this.reportForm.patchValue({
                        TrainingId: selectProgram.TrainingProgramId,
                        stringName: selectProgram.TrainingProgramName,
                    });
                }
                this.showProgram = !this.showProgram;
            });

        this.serviceDialogPosition.ToParent$.subscribe(
            (selectPosition) => {
                if (selectPosition) {
                    this.reportForm.patchValue({
                        PositionCode: selectPosition.PositionCode,
                        stringPosition: selectPosition.PositionName,
                    });
                }
                this.showPosition = !this.showPosition;
            });
    }

    // build form
    buildForm(): void {
        this.reportParamater = {
            TrainingId: 0,
            GetTypeProgram: 1,
            GroupCode: "",
            LocateID: "",
            AfterDate: undefined
        };

        this.reportForm = this.fb.group({
            stringName: [this.stringName],
            stringPosition: [this.stringPosition],
            TrainingId: [this.reportParamater.TrainingId],
            GetTypeProgram: [this.reportParamater.GetTypeProgram],
            LocateID: [this.reportParamater.LocateID],
            GroupCode: [this.reportParamater.GroupCode],
            PositionCode: [this.reportParamater.PositionCode],
            AfterDate: [this.reportParamater.AfterDate]
        });

        this.reportForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        this.onValueChanged();
    }

    // on value of form change
    onValueChanged(data?: any): void {
        if (!this.reportForm) { return; }

        this.reportParamater = this.reportForm.value;
        if (!this.reportParamater.TrainingId) { return; }

        let zone: string = "Asia/Bangkok";
        if (this.reportParamater) {
            if (this.reportParamater.AfterDate) {
                this.reportParamater.AfterDate = moment.tz(this.reportParamater.AfterDate, zone).toDate();
            }
        }

        if (this.condition === "Course") {
            this.onGetDataTrainingCourseFromWebApi();
        } else if (this.condition === "Program") {
            if (!this.reportParamater.GroupCode &&
                !this.reportParamater.LocateID &&
                !this.reportParamater.AfterDate &&
                !this.reportParamater.PositionCode) {
                this.dialogsService
                    .confirm("System Message", "การไม่เลือกเงื่อนอื่นร่วมด้วย การโหลดข้อมูลอาจใช้ระยะเวลานานมากขึ้น!!! คุณต้องการดำเนินการต่อหรือไม่?", this.viewContainerRef)
                    .subscribe(result => {
                        if (result) {
                            if (this.reportParamater.GetTypeProgram === 3) {
                                this.onGetDataTrainingProgramFromWebApiFull();
                            } else {
                                this.onGetDataTrainingProgramFromWebApi();
                            }
                        }
                    });
            } else {
                if (this.reportParamater.GetTypeProgram === 3) {
                    this.onGetDataTrainingProgramFromWebApiFull();
                } else {
                    this.onGetDataTrainingProgramFromWebApi();
                }
            }
        } else {
            console.error("Don,t have condition.");
        }
    }

    // get data training program full map
    onGetDataTrainingProgramFromWebApiFull(): void {
        this.trainingReports = new Array;
        // data 1
        let result1: ITrainingFilter = {
            AfterDate: this.reportParamater.AfterDate,
            GetTypeProgram: 1,
            GroupCode: this.reportParamater.GroupCode,
            LocateID: this.reportParamater.LocateID,
            TrainingId: this.reportParamater.TrainingId
        };
        // data 2
        let result2: ITrainingFilter = {
            AfterDate: this.reportParamater.AfterDate,
            GetTypeProgram: 2,
            GroupCode: this.reportParamater.GroupCode,
            LocateID: this.reportParamater.LocateID,
            TrainingId: this.reportParamater.TrainingId
        };

        this.service.getReportByTrainingProgramForBasic(result1)
            .subscribe(dbDataBase => {
                dbDataBase.forEach(value => {
                    this.trainingReports.push(value);
                });
            }, Error => console.error(Error), () => {
                this.service.getReportByTrainingProgramForStandard(result1)
                    .subscribe(dbDataBase => {
                        dbDataBase.forEach(value => {
                            this.trainingReports.push(value);
                        });
                    }, Error => console.error(Error), () => {
                        this.service.getReportByTrainingProgramForSupplement(result1)
                            .subscribe(dbDataBase => {
                                dbDataBase.forEach(value => {
                                    this.trainingReports.push(value);
                                });
                            }, Error => console.error(Error), () => {
                                this.service.getReportByTrainingProgramForBasic(result2)
                                    .subscribe(dbDataBase => {
                                        dbDataBase.forEach(value => {
                                            this.trainingReports.push(value);
                                        });
                                    }, Error => console.error(Error), () => {
                                        this.service.getReportByTrainingProgramForStandard(result2)
                                            .subscribe(dbDataBase => {
                                                dbDataBase.forEach(value => {
                                                    this.trainingReports.push(value);
                                                });
                                            }, Error => console.error(Error), () => {
                                                this.service.getReportByTrainingProgramForSupplement(result2)
                                                    .subscribe(dbDataBase => {
                                                        dbDataBase.forEach(value => {
                                                            this.trainingReports.push(value);
                                                        });
                                                    }, Error => console.error(Error), () => {
                                                        this.trainingReports = this.trainingReports.slice();
                                                    });
                                            });
                                    });
                            });
                    });
            });
    }

    // get data training program from webapi
    onGetDataTrainingProgramFromWebApi(): void {
        this.trainingReports = new Array;

        this.service.getReportByTrainingProgramForBasic(this.reportParamater)
            .subscribe(dbDataBase => {
                dbDataBase.forEach(value => {
                    this.trainingReports.push(value);
                });
            }, Error => console.error(Error), () => {
                this.service.getReportByTrainingProgramForStandard(this.reportParamater)
                    .subscribe(dbDataBase => {
                        dbDataBase.forEach(value => {
                            this.trainingReports.push(value);
                        });
                    }, Error => console.error(Error), () => {
                        this.service.getReportByTrainingProgramForSupplement(this.reportParamater)
                            .subscribe(dbDataBase => {
                                dbDataBase.forEach(value => {
                                    this.trainingReports.push(value);
                                });
                            }, Error => console.error(Error), () => {
                                this.trainingReports = this.trainingReports.slice();
                            });
                    });
            });
    }

    // get data training course from webapi
    onGetDataTrainingCourseFromWebApi(): void {
        this.service.getReportByTrainingCourse(this.reportParamater)
            .subscribe(dbDataBase => this.trainingReports = dbDataBase);
    }

    // row Track by
    rowTrackBy(index: number, row: any): any { return row.id; }

    // open course dialogs
    openDialog(moreCondition?: string): void {
        if (moreCondition) {
            if (moreCondition === "Position") {
                this.serviceDialogPosition.toChild("open");
                this.showPosition = !this.showPosition;
                return;
            }
        }

        if (this.condition === "Course") {
            this.serviceDialogCourse.toChild("open");
            this.showCourse = !this.showCourse;
        } else {
            this.serviceDialogProgram.toChild("open");
            this.showProgram = !this.showProgram;
        }
    }

    // reset
    resetFilter(): void {
        this.trainingReports = new Array;
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

    // on click report
    reportExcel() {
        if (this.condition === "Course") {
            if (this.reportParamater.TrainingId) {
                this.service.getReportExcelByTrainingCourse(this.reportParamater)
                    .subscribe(data => {
                        let link = document.createElement('a');
                        link.href = window.URL.createObjectURL(data);
                        link.download = "report_" + this.reportParamater.TrainingId + ".xlsx";
                        link.click();
                    },
                    error => this.dialogsService.error("Error Message","ไม่พบข้อมููลที่ต้องการ",this.viewContainerRef),
                    () => console.log('Completed file download.'));
            }
        } else {
            if (this.reportParamater.TrainingId) {
                if (!this.reportParamater.GroupCode ||
                    !this.reportParamater.PositionCode) {
                    this.dialogsService
                        .confirm("System Message", "การไม่เลือกกลุ่มงานและตำแหน่งงาน การโหลดข้อมูลอาจใช้ระยะเวลานานมากขึ้น!!! คุณต้องการดำเนินการต่อหรือไม่?", this.viewContainerRef)
                        .subscribe(result => {
                            if (result) {
                                this.service.getReportExcelByTrainingProgram(this.reportParamater)
                                    .subscribe(data => {
                                        let link = document.createElement('a');
                                        link.href = window.URL.createObjectURL(data);
                                        link.download = "report_" + this.reportParamater.TrainingId + ".xlsx";
                                        link.click();
                                    },
                                    error => this.dialogsService.error("Error Message", "ไม่พบข้อมููลที่ต้องการ", this.viewContainerRef),
                                    () => console.log('Completed file download.'));
                            }
                        });
                }
                else {
                    this.service.getReportExcelByTrainingProgram(this.reportParamater)
                        .subscribe(data => {
                            let link = document.createElement('a');
                            link.href = window.URL.createObjectURL(data);
                            link.download = "report_" + this.reportParamater.TrainingId + ".xlsx";
                            link.click();
                        },
                        error => this.dialogsService.error("Error Message", "ไม่พบข้อมููลที่ต้องการ", this.viewContainerRef),
                        () => console.log('Completed file download.'));
                }
            }
        }
    }
}