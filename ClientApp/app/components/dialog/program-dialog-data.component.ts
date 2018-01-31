// angular
import { Component, OnInit, OnDestroy, ElementRef, ViewChild } from "@angular/core";
// classes
import { LazyLoad } from "../../classes/lazyload.class";
import { ITrainingProgram } from "../../classes/training-program.class";
import { LazyLoadEvent } from "primeng/primeng";
// service
import { TrainingProgramService } from "../../services/training-program.service";
import { ProgramDialogDataCommunicateService } from "../../communicates/program-dialog-data.communicate"
// rxjs
import "rxjs/Rx";
import { Observable } from "rxjs/Observable"
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "program-dialog",
    templateUrl: "./program-dialog-data.component.html",
    styleUrls: ["../../styles/master.style.scss"],
})

export class ProgramDialogComponent implements OnInit, OnDestroy {
    program: ITrainingProgram;
    programs: Array<ITrainingProgram>;
    columns: Array<any>;
    totalRow: number;
    subscription: Subscription;
    // element
    @ViewChild("filter") filter: ElementRef;
    constructor(
        private service: TrainingProgramService,
        private serviceCom: ProgramDialogDataCommunicateService
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
                { field: "TrainingProgramCode", header: "Code", style: { 'width': '15%' } },
                { field: "TrainingProgramName", header: "Name" },
                { field: "Detail", header: "Detail" },
                { field: "TrainingProgramLeave", header: "Level", style: { 'width': '20%' } },
            ];
        }

        this.service.getAllWithLazyLoad(lazydata)
            .subscribe(restData => {
                this.programs = restData.Data;
                this.totalRow = restData.TotalRow;
            }, error => console.error(error));
    }

    // on send data
    onSendDataToPartne() {
        if (this.program) {
            this.serviceCom.toParent(this.program);
        }
    }
}