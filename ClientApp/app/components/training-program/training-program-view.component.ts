// angular
import { Component } from "@angular/core";
// class
import { ITrainingProgram } from "../../classes/training-program.class";
// component
import { AbstractViewComponent } from "../abstract/abstract-view.component";
// service
import { TrainingProgramService } from "../../services/training-program.service";
import { BasicCourseService } from "../../services/basic-course.service";
import { StandardCourseService } from "../../services/standard-course.service";
import { SupplementCourseService } from "../../services/supplement-course.service";
import { ProgramHasPositionService } from "../../services/program-has-position.service";
import { ProgramHasGroupService } from "../../services/program-has-group.service";
import { TrainingProgramCommunicateService } from "../../communicates/training-program.communicate"
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "training-program-view",
    templateUrl: "./training-program-view.component.html",
    styleUrls: ["../../styles/view.style.scss"],
})

export class TrainingProgramViewComponent
    extends AbstractViewComponent<ITrainingProgram, TrainingProgramService> {
    scrollHeight: string;
    basicColumns: any;
    standardColumns: any;
    supplementColumns: any;
    hasPositionColumns: any;
    hasGroupColumns: any;


    constructor(
        trainingProgramService: TrainingProgramService,
        private basicCourseService: BasicCourseService,
        private standardCourseService: StandardCourseService,
        private supplementCourseService: SupplementCourseService,
        private programHasPositionService: ProgramHasPositionService,
        private programHasGroupService: ProgramHasGroupService,
        trainingProgramComService: TrainingProgramCommunicateService,
    ) {
        super(
            trainingProgramService,
            trainingProgramComService,
        );
    }

    //Property
    get HeaderPosition(): string {
        if (this.displayValue) {
            if (this.displayValue.TblTrainingProgramHasPosition)
                return "For Position (" + this.displayValue.TblTrainingProgramHasPosition.length + ")";
        }
        return "For Position";
        
    }

    get HeaderStandard(): string {
        if (this.displayValue) {
            if (this.displayValue.TblStandardCourse)
                return "Standard Course (" + this.displayValue.TblStandardCourse.length + ")";
        }
        return "Standard Course";
    }

    get HeaderBasic(): string {
        if (this.displayValue) {
            if (this.displayValue.TblBasicCourse)
                return "Basic Course (" + this.displayValue.TblBasicCourse.length + ")";
        }
        return "Basic Course";
    }

    get HeaderSupplement(): string {
        if (this.displayValue) {
            if (this.displayValue.TblSupplementCourse)
                return "Supplement Course (" + this.displayValue.TblSupplementCourse.length + ")";
        }
        return "Supplement Course";
    }

    get HeaderGroup(): string {
        if (this.displayValue && this.displayValue.TblTrainingProgramHasGroup) {
            return "For Group (" + this.displayValue.TblTrainingProgramHasGroup.length + ")";
        }
        return "For Group";
    }

    // on get data by key
    onGetDataByKey(trainingProgram: ITrainingProgram): void {
        if (!this.scrollHeight) {
            if (window.innerWidth >= 1600) {
                this.scrollHeight = 50 + "vh";
            } else if (window.innerWidth > 1360 && window.innerWidth < 1600) {
                this.scrollHeight = 45 + "vh";
            } else {
                this.scrollHeight = 40 + "vh";
            }
        }

        if (!this.basicColumns) {
            this.basicColumns = [
                { field: "$id", header: "Row", style: { 'width': '15%' }},
                { field: "CourseString", header: "Basic Course" },
            ];
        }

        if (!this.standardColumns) {
            this.standardColumns = [
                { field: "$id", header: "Row", style: { 'width': '15%' } },
                { field: "CourseString", header: "Standard Course" },
            ];
        }

        if (!this.supplementColumns) {
            this.supplementColumns = [
                { field: "$id", header: "Row", style: { 'width': '15%' }},
                { field: "CourseString", header: "Supplement Course" },
            ];
        }

        if (!this.hasPositionColumns) {
            this.hasPositionColumns = [
                { field: "$id", header: "Row", style: { 'width': '15%' } },
                { field: "PositionString", header: "Position" },
            ];
        }

        if (!this.hasGroupColumns) {
            this.hasGroupColumns = [
                { field: "$id", header: "Row", style: { 'width': '15%' } },
                { field: "GroupString", header: "Group" },
            ];
        }

        // clear data
        this.displayValue = {
            TrainingProgramId: 0
        };

        this.service.getOneKeyNumber(trainingProgram.TrainingProgramId)
            .subscribe(dbTrainingProgram => {
                this.displayValue = dbTrainingProgram;
                if (this.displayValue.TrainingProgramId) {
                    this.basicCourseService.getByMasterId(this.displayValue.TrainingProgramId)
                        .subscribe(db => this.displayValue.TblBasicCourse = db.slice());

                    this.standardCourseService.getByMasterId(this.displayValue.TrainingProgramId)
                        .subscribe(db => this.displayValue.TblStandardCourse = db.slice());

                    this.supplementCourseService.getByMasterId(this.displayValue.TrainingProgramId)
                        .subscribe(db => this.displayValue.TblSupplementCourse = db.slice());

                    this.programHasPositionService.getByMasterId(this.displayValue.TrainingProgramId)
                        .subscribe(db => this.displayValue.TblTrainingProgramHasPosition = db.slice());

                    this.programHasGroupService.getByMasterId(this.displayValue.TrainingProgramId)
                        .subscribe(db => this.displayValue.TblTrainingProgramHasGroup = db.slice());
                }
            }, error => console.error(error));
    }

    // row track
    rowTrackBy(index: number, row: any) { return row.id; }

    // change row class
    customRowClass(rowData : any): string {
        return rowData.StatusForTraining === 2 ? 'disabled-row' : 'enabled-row';
    }

}