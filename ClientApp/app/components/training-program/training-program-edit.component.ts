// angular
import { Component } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// class
import { ITrainingProgram } from "../../classes/training-program.class";
import { IBasicCourse } from "../../classes/basic-course.class";
import { IStandardCourse } from "../../classes/standard-course.class";
import { ISupplementCourse } from "../../classes/supplement-course.class";
import { IProgramHasPosition } from "../../classes/program-has-position.class";
import { IProgramHasGroup } from "../../classes/program-has-group.class";
// component
import { AbstractEditComponent } from "../abstract/abstract-edit.component";
// service
import { TrainingProgramService } from "../../services/training-program.service";
import { BasicCourseService } from "../../services/basic-course.service";
import { StandardCourseService } from "../../services/standard-course.service";
import { SupplementCourseService } from "../../services/supplement-course.service";
import { ProgramHasPositionService } from "../../services/program-has-position.service";
import { ProgramHasGroupService } from "../../services/program-has-group.service";
// communicate
import { TrainingProgramCommunicateService } from "../../communicates/training-program.communicate";
import { CousreDialogDataCommunicateService } from "../../communicates/course-dialog-data.communicate";
import { PositionDialogDataCommunicateService } from "../../communicates/position-dialog-data.communicate";
import { GroupDialogDataCommunicateService } from "../../communicates/group-dialog-data.communiate";
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "training-program-edit",
    templateUrl: "./training-program-edit.component.html",
    styleUrls: ["../../styles/edit.style.scss"],
})

export class TrainingProgramEditComponent
    extends AbstractEditComponent<ITrainingProgram, TrainingProgramService> {

    showCourse: boolean;
    showPosition: boolean;
    showGroup: boolean;
    basicColumns: any;
    standardColumns: any;
    supplementColumns: any;
    hasPositionColumns: any;
    hasGroupColumns: any;
    courseFor: string;
    constructor(
        trainingProgramService: TrainingProgramService,
        trainingProgramComService: TrainingProgramCommunicateService,
        private basicCourseService: BasicCourseService,
        private standardCourseService: StandardCourseService,
        private supplementCourseService: SupplementCourseService,
        private programHasPositionService: ProgramHasPositionService,
        private programHasGroupService: ProgramHasGroupService,
        private couserDialComService: CousreDialogDataCommunicateService,
        private postionDialComService: PositionDialogDataCommunicateService,
        private groupDialComService: GroupDialogDataCommunicateService,
        private fb: FormBuilder,
    ) {
        super(
            trainingProgramService,
            trainingProgramComService,
        );
    }

    //Property
    get HeaderPosition(): string {
        if (this.editValue) {
            if (this.editValue.TblTrainingProgramHasPosition)
                return "For Position (" + this.editValue.TblTrainingProgramHasPosition.length + ")";
        }
        return "For Position";
    }

    get HeaderStandard(): string {
        if (this.editValue) {
            if (this.editValue.TblStandardCourse)
                return "Standard Course (" + this.editValue.TblStandardCourse.length + ")";
        }
        return "Standard Course";
    }

    get HeaderBasic(): string {
        if (this.editValue) {
            if (this.editValue.TblBasicCourse)
                return "Basic Course (" + this.editValue.TblBasicCourse.length + ")";
        }
        return "Basic Course";
    }

    get HeaderSupplement(): string {
        if (this.editValue) {
            if (this.editValue.TblSupplementCourse)
                return "Supplement Course (" + this.editValue.TblSupplementCourse.length + ")";
        }
        return "Supplement Course";
    }

    get HeaderGroup(): string {
        if (this.editValue && this.editValue.TblTrainingProgramHasGroup) {
            return "For Group (" + this.editValue.TblTrainingProgramHasGroup.length + ")";
        }
        return "For Group";
    }

    // on get data by key
    onGetDataByKey(trainingProgram?: ITrainingProgram): void {
        if (trainingProgram) {
            this.service.getOneKeyNumber(trainingProgram.TrainingProgramId)
                .subscribe(dbTrainingProgram => {
                    this.editValue = dbTrainingProgram;
                    if (this.editValue.TrainingProgramId) {
                        this.basicCourseService.getByMasterId(this.editValue.TrainingProgramId)
                            .subscribe(db => {
                                this.editValue.TblBasicCourse = db.slice();
                                if (this.editValueForm) {
                                    this.editValueForm.patchValue({
                                        TblBasicCourse: this.editValue.TblBasicCourse.slice(),
                                    });
                                }
                            });

                        this.standardCourseService.getByMasterId(this.editValue.TrainingProgramId)
                            .subscribe(db => {
                                this.editValue.TblStandardCourse = db.slice();
                                if (this.editValueForm) {
                                    this.editValueForm.patchValue({
                                        TblStandardCourse: this.editValue.TblStandardCourse.slice(),
                                    });
                                }
                            });

                        this.supplementCourseService.getByMasterId(this.editValue.TrainingProgramId)
                            .subscribe(db => {
                                this.editValue.TblSupplementCourse = db.slice();
                                if (this.editValueForm) {
                                    this.editValueForm.patchValue({
                                        TblSupplementCourse: this.editValue.TblSupplementCourse,
                                    });
                                }
                            });

                        this.programHasPositionService.getByMasterId(this.editValue.TrainingProgramId)
                            .subscribe(db => {
                                this.editValue.TblTrainingProgramHasPosition = db.slice();
                                if (this.editValueForm) {
                                    this.editValueForm.patchValue({
                                        TblTrainingProgramHasPosition: this.editValue.TblTrainingProgramHasPosition,
                                    });
                                }
                            });

                        this.programHasGroupService.getByMasterId(this.editValue.TrainingProgramId)
                            .subscribe(db => {
                                this.editValue.TblTrainingProgramHasGroup = db.slice();
                                if (this.editValueForm) {
                                    this.editValueForm.patchValue({
                                        TblTrainingProgramHasGroup: this.editValue.TblTrainingProgramHasGroup,
                                    });
                                }
                            });
                    }
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                TrainingProgramId:0
            };
            this.defineData()
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
        this.showCourse = false;
        this.showPosition = false;
        this.showGroup = false;
        // define column for data-table
        if (!this.basicColumns) {
            this.basicColumns = [
                { field: "CourseString", header: "Basic Course" },
            ];
        }
        if (!this.standardColumns) {
            this.standardColumns = [
                { field: "CourseString", header: "Standard Course" },
            ];
        }
        if (!this.supplementColumns) {
            this.supplementColumns = [
                { field: "CourseString", header: "Supplement Course" },
            ];
        }
        if (!this.hasPositionColumns) {
            this.hasPositionColumns = [
                { field: "PositionString", header: "Position" },
            ];
        }

        if (!this.hasGroupColumns) {
            this.hasGroupColumns = [
                { field: "GroupString", header: "Group" },
            ];
        }

        this.couserDialComService.ToParent$.subscribe(
            (Couse) => {
                if (Couse) {

                    // debug here
                    console.log("Type :", this.courseFor);

                    if (this.courseFor === "basic") {

                        // debug here
                        console.log("In : basic");

                        if (!this.editValue.TblBasicCourse) {
                            this.editValue.TblBasicCourse = new Array;
                            let newData: IBasicCourse = {
                                BasicCourseId : 0
                            };
                            newData.TrainingCousreId = Couse.TrainingCousreId;
                            newData.CourseString = Couse.TrainingCousreName;
                            this.editValue.TblBasicCourse.push(Object.assign({}, newData));
                        }
                        else {
                            if (!this.editValue.TblBasicCourse.find(item => item.TrainingCousreId === Couse.TrainingCousreId)) {
                                let newData: IBasicCourse = {
                                    BasicCourseId : 0
                                };
                                newData.TrainingCousreId = Couse.TrainingCousreId;
                                newData.CourseString = Couse.TrainingCousreName;
                                this.editValue.TblBasicCourse.push(Object.assign({}, newData));
                            }
                        }
                        this.editValue.TblBasicCourse = this.editValue.TblBasicCourse.slice();
                    } else if (this.courseFor === "standard") {
                        if (!this.editValue.TblStandardCourse) {
                            this.editValue.TblStandardCourse = new Array;
                            let newData: IStandardCourse = {
                                StandardCourseId : 0
                            };
                            newData.TrainingCousreId = Couse.TrainingCousreId;
                            newData.CourseString = Couse.TrainingCousreName;
                            this.editValue.TblStandardCourse.push(Object.assign({}, newData));
                        }
                        else {
                            if (!this.editValue.TblStandardCourse.find(item => item.TrainingCousreId === Couse.TrainingCousreId)) {
                                let newData: IStandardCourse = {
                                    StandardCourseId : 0
                                };
                                newData.TrainingCousreId = Couse.TrainingCousreId;
                                newData.CourseString = Couse.TrainingCousreName;
                                this.editValue.TblStandardCourse.push(Object.assign({}, newData));
                            }
                        }
                        this.editValue.TblStandardCourse = this.editValue.TblStandardCourse.slice();
                       ;
                    } else if (this.courseFor === "supplement") {
                        if (!this.editValue.TblSupplementCourse) {
                            this.editValue.TblSupplementCourse = new Array;
                            let newData: ISupplementCourse = {
                                SupplermentCourseId : 0
                            };
                            newData.TrainingCousreId = Couse.TrainingCousreId;
                            newData.CourseString = Couse.TrainingCousreName;
                            this.editValue.TblSupplementCourse.push(Object.assign({}, newData));
                        }
                        else {
                            if (!this.editValue.TblSupplementCourse.find(item => item.TrainingCousreId === Couse.TrainingCousreId)) {
                                let newData: ISupplementCourse = {
                                    SupplermentCourseId : 0
                                };
                                newData.TrainingCousreId = Couse.TrainingCousreId;
                                newData.CourseString = Couse.TrainingCousreName;
                                this.editValue.TblSupplementCourse.push(Object.assign({}, newData));
                            }
                        }
                        this.editValue.TblSupplementCourse = this.editValue.TblSupplementCourse.slice();

                    }
                    if (this.editValue.TblBasicCourse)
                        this.editValueForm.patchValue({
                            TblBasicCourse: this.editValue.TblBasicCourse.slice(),
                        });

                    if (this.editValue.TblStandardCourse)
                        this.editValueForm.patchValue({
                            TblStandardCourse: this.editValue.TblStandardCourse.slice(),
                            })

                    if (this.editValue.TblSupplementCourse)
                        this.editValueForm.patchValue({
                            TblSupplementCourse: this.editValue.TblSupplementCourse.slice(),
                        });
                }
                // this.showCourse = !this.showCourse;
            });
        this.postionDialComService.ToParent$.subscribe(
            (Postion) => {
                if (Postion) {
                    if (!this.editValue.TblTrainingProgramHasPosition) {
                        this.editValue.TblTrainingProgramHasPosition = new Array;
                        let newData: IProgramHasPosition = {
                            ProgramHasPositionId : 0
                        };
                        newData.PositionCode = Postion.PositionCode;
                        newData.PositionString = Postion.PositionName;
                        this.editValue.TblTrainingProgramHasPosition.push(Object.assign({}, newData));
                    }
                    else {
                        if (!this.editValue.TblTrainingProgramHasPosition.find(item => item.PositionCode === Postion.PositionCode)) {
                            let newData: IProgramHasPosition = {
                                ProgramHasPositionId: 0
                            };
                            newData.PositionCode = Postion.PositionCode;
                            newData.PositionString = Postion.PositionName;
                            this.editValue.TblTrainingProgramHasPosition.push(Object.assign({}, newData));
                        }
                    }
                    this.editValue.TblTrainingProgramHasPosition = this.editValue.TblTrainingProgramHasPosition.slice();
                    this.editValueForm.patchValue({
                        TblTrainingProgramHasPosition: this.editValue.TblTrainingProgramHasPosition.slice(),
                    });
                }
            }
        );
        this.groupDialComService.ToParent$.subscribe(
            (Group) => {
                if (Group) {
                    if (!this.editValue.TblTrainingProgramHasGroup) {
                        this.editValue.TblTrainingProgramHasGroup = new Array;
                        let newData: IProgramHasGroup = {
                            ProgramHasGroupId: 0
                        };
                        newData.GroupCode = Group.GroupCode;
                        newData.GroupString = Group.GroupDesc;
                        this.editValue.TblTrainingProgramHasGroup.push(Object.assign({}, newData));
                    }
                    else {
                        if (!this.editValue.TblTrainingProgramHasGroup.find(item => item.GroupCode === Group.GroupCode)) {
                            let newData: IProgramHasGroup = {
                                ProgramHasGroupId: 0
                            };
                            newData.GroupCode = Group.GroupCode;
                            newData.GroupString = Group.GroupDesc;
                            this.editValue.TblTrainingProgramHasGroup.push(Object.assign({}, newData));
                        }
                    }
                    this.editValue.TblTrainingProgramHasGroup = this.editValue.TblTrainingProgramHasGroup.slice();
                    this.editValueForm.patchValue({
                        TblTrainingProgramHasGroup: this.editValue.TblTrainingProgramHasGroup.slice(),
                    });
                }
            }
        );
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            TrainingProgramId: [this.editValue.TrainingProgramId],
            TrainingProgramCode: [this.editValue.TrainingProgramCode,
                [
                    Validators.required,
                    Validators.maxLength(50)
                ]
            ],
            TrainingProgramName: [this.editValue.TrainingProgramName,
                [
                    Validators.required,
                    Validators.maxLength(200)
                ]
            ],
            TrainingProgramLevelString: [this.editValue.TrainingProgramLevelString],
            Detail: [this.editValue.Detail,
                [
                    Validators.maxLength(200)
                ]
            ],
            Remark: [this.editValue.Remark,
                [
                    Validators.maxLength(200)
                ]
            ],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            TblBasicCourse: [this.editValue.TblBasicCourse],
            TblStandardCourse: [this.editValue.TblStandardCourse],
            TblSupplementCourse: [this.editValue.TblSupplementCourse],
            TblTrainingProgramHasPosition: [this.editValue.TblTrainingProgramHasPosition],
            TblTrainingProgramHasGroup:[this.editValue.TblTrainingProgramHasGroup]
        });
        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        this.onValueChanged(); // (re)set validation message now
    }

    // open course dialogs
    openCourseDialog(courseFor: string) {
        this.courseFor = courseFor;
        this.showCourse = !this.showCourse;
        this.couserDialComService.toChild("open");
    }

    // open employee dialogs
    openPositionDialog() {
        this.showPosition = !this.showPosition;
        this.postionDialComService.toChild("open");
    }

    // open employee dialogs
    openGroupDialog() {
        this.showGroup = !this.showGroup;
        this.groupDialComService.toChild("open");
    }
    // rowTrackBy
    rowTrackBy(index: number, row: any) { return row.id; }

    // on remove training detail
    removerItem(item: any): void {

        // debug here
        // console.log(JSON.stringify(item));
        if (item) {
            if ("BasicCourseId" in item) {
                if (this.editValue.TblBasicCourse) {
                    let template = this.editValue.TblBasicCourse
                        .filter(x => x.TrainingCousreId !== item.TrainingCousreId);
                    this.editValue.TblBasicCourse = new Array();
                    setTimeout(() => {
                        this.editValue.TblBasicCourse = template.slice();
                        this.editValueForm.patchValue({
                            TblBasicCourse: this.editValue.TblBasicCourse.slice(),
                        });
                    }, 150);
                }
            }
            else if ("StandardCourseId" in item) {
                if (this.editValue.TblStandardCourse) {
                    let template = this.editValue.TblStandardCourse
                        .filter(x => x.TrainingCousreId !== item.TrainingCousreId);
                    this.editValue.TblStandardCourse = new Array();
                    setTimeout(() => {
                        this.editValue.TblStandardCourse = template.slice();
                        this.editValueForm.patchValue({
                            TblStandardCourse: this.editValue.TblStandardCourse.slice(),
                        });
                    }, 150);
                }
            }
            else if ("SupplermentCourseId" in item) {
                if (this.editValue.TblSupplementCourse) {
                    let template = this.editValue.TblSupplementCourse
                        .filter(x => x.TrainingCousreId !== item.TrainingCousreId);
                    this.editValue.TblSupplementCourse = new Array();
                    setTimeout(() => {
                        this.editValue.TblSupplementCourse = template.slice();
                        this.editValueForm.patchValue({
                            TblSupplementCourse: this.editValue.TblSupplementCourse.slice(),
                        });
                    }, 150);
                }
            }
            else if ("ProgramHasPositionId" in item) {
                //this.editValue.TblTrainingProgramHasPosition = this.editValue.TblTrainingProgramHasPosition
                //    .filter(x => x.PositionCode !== item.PositionCode);
                //this.editValue.TblTrainingProgramHasPosition = this.editValue.TblTrainingProgramHasPosition.slice();

                if (this.editValue.TblTrainingProgramHasPosition) {
                    let template = this.editValue.TblTrainingProgramHasPosition.filter(x => x.PositionCode !== item.PositionCode);
                    this.editValue.TblTrainingProgramHasPosition = new Array();
                    setTimeout(() => {
                        this.editValue.TblTrainingProgramHasPosition = template.slice();
                        this.editValueForm.patchValue({
                            TblTrainingProgramHasPosition: this.editValue.TblTrainingProgramHasPosition.slice(),
                        });
                    }, 150);
                }
            }
            else if ("ProgramHasGroupId" in item) {
                if (this.editValue.TblTrainingProgramHasGroup) {
                    let template = this.editValue.TblTrainingProgramHasGroup.filter(x => x.GroupCode !== item.GroupCode);
                    this.editValue.TblTrainingProgramHasGroup = new Array();
                    setTimeout(() => {
                        this.editValue.TblTrainingProgramHasGroup = template.slice();
                        this.editValueForm.patchValue({
                            TblTrainingProgramHasGroup: this.editValue.TblTrainingProgramHasGroup.slice(),
                        });
                    }, 150);
                }
            }
        }
    }
}

