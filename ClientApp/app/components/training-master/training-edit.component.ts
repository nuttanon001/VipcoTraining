// angular
import { Component } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// class
import { ITrainingMaster } from "../../classes/training-master.class";
import { ITrainingDetail } from "../../classes/training-detail.class";
import { ITrainingCousre } from "../../classes/training-cousre.class";
import { IEmployee } from "../../classes/employee.class";
import { SelectItem } from "primeng/primeng";
// component
import { AbstractEditComponent } from "../abstract/abstract-edit.component";
// service
import { TrainingDetailService } from "../../services/training-detail.service";
import { TrainingMasterService } from "../../services/training-master.service";
import { TrainingCourseService } from "../../services/training-course.service";
import { PlaceService } from "../../services/place.service";
import { TrainingMasterCommunicateService } from "../../communicates/training-master.communicate";
import { CousreDialogDataCommunicateService } from "../../communicates/course-dialog-data.communicate";
import { EmployeeDialogDataCommunicateService } from "../../communicates/employee-dialog-data.communicate";
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "training-edit",
    templateUrl: "./training-edit.component.html",
    styleUrls: ["../../styles/edit.style.scss"],
    providers: [PlaceService]
})

export class TrainingEditComponent
    extends AbstractEditComponent<ITrainingMaster, TrainingMasterService> {

    showCourse: boolean;
    showEmployee: boolean;
    columns: any;
    places: Array<SelectItem>;
    course: ITrainingCousre|undefined;

    constructor(
        trainingService: TrainingMasterService,
        trainingComService: TrainingMasterCommunicateService,
        private trainingDetailService: TrainingDetailService,
        private courseService: TrainingCourseService,
        private placeService: PlaceService,
        private couserDialComService: CousreDialogDataCommunicateService,
        private employeeDialComService: EmployeeDialogDataCommunicateService,
        private fb: FormBuilder,
    ) {
        super(
            trainingService,
            trainingComService,
        );
    }
    // on get data by key
    onGetDataByKey(training?: ITrainingMaster): void {
        if (training) {
            this.service.getOneKeyNumber(training.TrainingMasterId)
                .subscribe(dbTrainingMaster => {
                    this.editValue = dbTrainingMaster;
                    if (this.editValue.TrainingDate) {
                        this.editValue.TrainingDate = this.editValue.TrainingDate != null ?
                            new Date(this.editValue.TrainingDate) : new Date();
                    }
                    // get Training Detail
                    if (this.editValue.TrainingMasterId) {
                        this.trainingDetailService.getByMasterId(this.editValue.TrainingMasterId)
                            .subscribe(dbTrainingDetail => {
                                setTimeout(() => {
                                    this.editValue.TblTrainingDetail = dbTrainingDetail.slice();
                                    this.editValueForm.patchValue({
                                        TblTrainingDetail: this.editValue.TblTrainingDetail.slice(),
                                    });
                                }, 150);
                            });
                    }

                    // get Training Course
                    if (this.editValue.TrainingCousreId) {
                        this.courseService.getOneKeyNumber(this.editValue.TrainingCousreId)
                            .subscribe(dbCourse => this.course = dbCourse);
                    }

                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                TrainingMasterId : 0
            };
            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
        this.showCourse = false;
        this.showEmployee = false;
        // get data of place
        this.getPlaceArray();

        if (!this.columns) {
            this.columns = [
                { field: "EmployeeTraining", header: "Code", style: { "width": "10%" } },
                { field: "EmployeeNameString", header: "Employee" },
                { field: "Score", header: "Score", style: { "width": "10%" }, edit: true  },
                { field: "StatusForTrainingString", header: "Status", style: { "width": "10%" }},
            ];
        }

        this.couserDialComService.ToParent$.subscribe(
            (selectedCouse) => {
                if (selectedCouse) {
                    this.editValueForm.patchValue({
                        TrainingCousreId: selectedCouse.TrainingCousreId,
                        TrainingCode: selectedCouse.TrainingCousreCode,
                        TrainingName: selectedCouse.TrainingCousreName,
                    });

                    if (selectedCouse.BaseCost) {
                        const trainingCost = this.editValueForm.get("TrainingCost");

                        if (trainingCost) {
                            if (!trainingCost.value) {
                                this.editValueForm.patchValue({
                                    TrainingCost: selectedCouse.BaseCost
                                });
                            }
                        }
                    }

                    // get Training Course
                    if (selectedCouse.TrainingCousreId) {
                        this.courseService.getOneKeyNumber(selectedCouse.TrainingCousreId)
                            .subscribe(dbCourse => this.course = dbCourse);
                    }
                }

                this.showCourse = !this.showCourse;
            });

        this.employeeDialComService.ToParentArray$.subscribe(
            (employees) => {
                if (employees) {
                    employees.forEach((value, index) => {
                        // categories.find(item => item.categoryId === 1)
                        if (!this.editValue.TblTrainingDetail) {
                            this.editValue.TblTrainingDetail = new Array;
                        }

                        if (this.editValue.TblTrainingDetail.length > 0) {
                            if (!this.editValue.TblTrainingDetail.find(item => item.EmployeeTraining === value.EmpCode)) {
                                // cloning an object
                                this.editValue.TblTrainingDetail.push(Object.assign({}, this.newTrainingDetail(value)));
                            }
                        } else {
                            // cloning an object
                            this.editValue.TblTrainingDetail.push(Object.assign({}, this.newTrainingDetail(value)));
                        }
                    });
                    this.editValue.TblTrainingDetail = this.editValue.TblTrainingDetail!.slice();
                    this.editValueForm.patchValue({
                        TblTrainingDetail: this.editValue.TblTrainingDetail.slice(),
                    });
                }
                this.showEmployee = !this.showEmployee;
            }
        );
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            TrainingMasterId: [this.editValue.TrainingMasterId],
            TrainingCousreId: [this.editValue.TrainingCousreId],
            TrainingCode: [this.editValue.TrainingCode,
                [
                    Validators.required,
                ]
            ],
            TrainingName: [this.editValue.TrainingName,
                [
                    Validators.required,
                ]
            ],
            Detail: [this.editValue.Detail,
                [
                    Validators.maxLength(200)
                ]
            ],
            TrainingDate: [this.editValue.TrainingDate,
                [
                    Validators.required,
                ]
            ],
            TrainingDateTime: [this.editValue.TrainingDateTime,
                [
                    Validators.required,
                ]
            ],
            TrainingDateEnd: [this.editValue.TrainingDateEnd,
                [
                    Validators.required,
                ]
            ],
            TrainingDateEndTime: [this.editValue.TrainingDateEndTime,
                [
                    Validators.required,
                ]
            ],
            LecturerName: [this.editValue.LecturerName,
                [
                    Validators.maxLength(200)
                ]
            ],
            TrainingDurationHour: [this.editValue.TrainingDurationHour],
            TrainingCost:[this.editValue.TrainingCost],
            Remark: [this.editValue.Remark,
                [
                    Validators.maxLength(200)
                ]
            ],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            TblTrainingDetail: [this.editValue.TblTrainingDetail],
            PlaceId:[this.editValue.PlaceId],
        });
        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        this.onValueChanged(); // (re)set validation message now
    }
    // get places data to array
    getPlaceArray(): void {
        this.placeService.getAll()
            .subscribe(result => {
                this.places = new Array;
                this.places.push({ label: "-", value: undefined });
                for (let item of result) {
                    this.places.push({ label: item.PlaceName || "", value: item.PlaceId });
                }
            }, error => console.error(error));
    }
    // new TrainingDetail
    newTrainingDetail(employee: IEmployee): ITrainingDetail {
        let detail: ITrainingDetail = {
            TrainingDetailId:0
        };

        detail.EmployeeNameString = employee.NameThai;
        detail.EmployeeTraining = employee.EmpCode;
        detail.StatusForTraining = 0;
        detail.StatusForTrainingString = "รอคะแนน";
        return detail;
    }
    // open course dialogs
    openCourseDialog():void {
        this.showCourse = !this.showCourse;
        this.couserDialComService.toChild("open");
    }
    // open employee dialogs
    openEmployeeDialog():void {
        this.showEmployee = !this.showEmployee;
        this.employeeDialComService.toChild("open");
    }
    // rowTrackBy
    rowTrackBy(index: number, row: any):any { return row.id; }
    // on row edit
    onMasterEditDataTable(trainingDetail: ITrainingDetail): void {
        if (!trainingDetail) {
            return;
        }
        // find breed
        const template: ITrainingDetail|undefined = this.findTrainingDetailByEmpCode(trainingDetail.EmployeeTraining);
        if (template) {
            template.Score = trainingDetail.Score;
            if (this.course && this.course.MinimunScore && template.Score !== undefined) {
                template.StatusForTrainingString = template.Score >= this.course.MinimunScore
                    ? "ผ่าน" : "ไม่ผ่าน";

                if (this.editValue.TblTrainingDetail) {
                    this.editValueForm.patchValue({
                        TblTrainingDetail: this.editValue.TblTrainingDetail.slice(),
                    });
                }
            }
        }
    }
    // find beerd by send id
    findTrainingDetailByEmpCode(employeeCode: string|undefined): ITrainingDetail|undefined {
        if (this.editValue.TblTrainingDetail) {
            return this.editValue.TblTrainingDetail.find((value, index) => value.EmployeeTraining === employeeCode);
        } else {
            return undefined;
        }
    }
    // on Pass Or Not
    onPassOrNotTraining(trainingDetail: ITrainingDetail,pass:boolean): void {
        if (this.course) {
            if (this.course.MinimunScore !== trainingDetail.Score) {
                trainingDetail.Score = pass ? (this.course.MinimunScore || 0) + 1 :
                    (this.course.MinimunScore || 0) - (this.course.MinimunScore || 1);
                this.onMasterEditDataTable(trainingDetail);
            }
        }
    }
    // on All Pass Or Not
    onAllPassOrNotTraining(pass: boolean): void {
        if (this.editValue.TblTrainingDetail) {
            this.editValue.TblTrainingDetail.forEach((value) => {
                this.onPassOrNotTraining(value, pass);
            });
        }
    }
    // on remove training detail
    removerEmployee(trainingDetail: ITrainingDetail): void {
        if (trainingDetail) {
            if (this.editValue.TblTrainingDetail) {
                let template : Array<ITrainingDetail> = this.editValue.TblTrainingDetail
                    .filter(item => item.EmployeeTraining !== trainingDetail.EmployeeTraining);
                this.editValue.TblTrainingDetail = new Array();
                setTimeout(() => {
                    this.editValue.TblTrainingDetail = template.slice();
                    this.editValueForm.patchValue({
                        TblTrainingDetail: this.editValue.TblTrainingDetail.slice(),
                    });
                }, 150);
            }
        }
    }
}

