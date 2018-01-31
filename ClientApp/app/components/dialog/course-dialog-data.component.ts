// angular
import { Component, OnInit, OnDestroy, ElementRef, ViewChild } from "@angular/core";
// classes
import { ITrainingCousre } from "../../classes/training-cousre.class";
import { LazyLoad } from "../../classes/lazyload.class";
import { LazyLoadEvent } from "primeng/primeng";
// service
import { TrainingCourseService } from "../../services/training-course.service";
import { CousreDialogDataCommunicateService } from "../../communicates/course-dialog-data.communicate"
// rxjs
import "rxjs/Rx";
import { Observable } from "rxjs/Observable"
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "corse-dialog",
    templateUrl: "./course-dialog-data.component.html",
    styleUrls: ["../../styles/master.style.scss"],
})

export class CourseDialogComponent implements OnInit, OnDestroy {
    course: ITrainingCousre;
    courses: Array<ITrainingCousre>;
    columns: Array<any>;
    totalRow: number;
    subscription: Subscription;
    // element
    @ViewChild("filter") filter: ElementRef;
    constructor(
        private service: TrainingCourseService,
        private serviceCom: CousreDialogDataCommunicateService
    ) { }

    // on hook init
    ngOnInit(): void {
        this.subscription = this.serviceCom.ToChild$.subscribe(
            (condition: any) => {
                // if have condition try here
                this.onLoadData(undefined);
            });

        Observable.fromEvent(this.filter.nativeElement, 'keyup')
            .debounceTime(350)
            .distinctUntilChanged()
            .subscribe(() => {
                this.onLoadData(undefined);
            });
    }

    // on hook destory
    ngOnDestroy(): void {
        // prevent memory leak when component destroyed
        this.subscription.unsubscribe();
    }

    // row Track by
    rowTrackBy(index: number, row: any) { return row.id; }

    // on load data
    onLoadData(event: LazyLoadEvent|undefined): void {
        if (!event) {
            event = {
                first: 0,
                rows: 25,
                sortField: "",
                sortOrder: 1,
            }
        }

        let lazydata: LazyLoad = {
            First: event.first,
            Rows: event.rows,
            SortField: event.sortField,
            SortOrder: event.sortOrder,
            Filter: this.filter.nativeElement.value
        };

        if (!this.columns) {
            this.columns = [
                { field: "TrainingCousreCode", header: "Code", style: { 'width': '15%' } },
                { field: "TrainingCousreName", header: "Name" },
                { field: "TrainingTypeString", header: "Type" },
                { field: "TrainingLevelString", header: "Level", style: { 'width': '10%' } },
                { field: "EducationRequirementString", header: "Education" },
            ];
        }

        this.service.getAllWithLazyLoad(lazydata)
            .subscribe(restData => {
                this.courses = restData.Data;
                this.totalRow = restData.TotalRow;
            }, error => console.error(error));
    }

    // on send data
    onSendDataToPartne(course: ITrainingCousre) {
        // debug here
        // console.log("Course click", course);
        if (course) {
            this.serviceCom.toParent(course);
        }
    }
}