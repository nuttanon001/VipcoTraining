// angular
import { Component, OnInit, OnDestroy, ElementRef, ViewChild } from "@angular/core";
// classes
import { LazyLoad } from "../../classes/lazyload.class";
import { IPosition } from "../../classes/position.class"
import { LazyLoadEvent } from "primeng/primeng";
// service
import { PositionService } from "../../services/position.service";
import { PositionDialogDataCommunicateService } from "../../communicates/position-dialog-data.communicate"
// rxjs
import "rxjs/Rx";
import { Observable } from "rxjs/Observable"
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "position-dialog",
    templateUrl: "./position-dialog-data.component.html",
    styleUrls: ["../../styles/master.style.scss"],
})

export class PositionDialogComponent implements OnInit, OnDestroy {
    position: IPosition;
    positions: Array<IPosition>;
    columns: Array<any>;
    totalRow: number;
    subscription: Subscription;
    // element
    @ViewChild("filter") filter: ElementRef;
    constructor(
        private service: PositionService,
        private serviceCom: PositionDialogDataCommunicateService
    ) { }

    // on hook init
    ngOnInit(): void {
        this.subscription = this.serviceCom.ToChild$.subscribe(
            (condition: any) => {
                // if have condition try here
                this.onLoadData(undefined);
            });

        Observable.fromEvent(this.filter.nativeElement, 'keyup')
            .debounceTime(200)
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
                //{ field: "$id", header: "Row", style: { 'width': '10%' } },
                { field: "PositionCode", header: "Code", style: { 'width': '25%' } },
                { field: "PositionName", header: "PositionName" },
            ];
        }

        this.service.getAllWithLazyLoad(lazydata)
            .subscribe(restData => {
                this.positions = restData.Data;
                this.totalRow = restData.TotalRow;
            }, error => console.error(error));
    }

    // on send data
    onSendDataToPartne() {
        if (this.position) {
            this.serviceCom.toParent(this.position);
        }
    }
}