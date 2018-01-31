// angular
import { Component, OnInit, OnDestroy, ElementRef, ViewChild } from "@angular/core";
// classes
import { IEmployee } from "../../classes/employee.class";
import { LazyLoad } from "../../classes/lazyload.class";
import { LazyLoadEvent } from "primeng/primeng";
// service
import { LocationService } from "../../services/location.service";
import { EmployeeService } from "../../services/employee.service";
import { EmployeeDialogDataCommunicateService } from "../../communicates/employee-dialog-data.communicate"
// rxjs
import "rxjs/Rx";
import { Observable } from "rxjs/Observable"
import { Subscription } from "rxjs/Subscription";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
// primeng
import { SelectItem } from "primeng/primeng"

@Component({
    selector: "employee-dialog",
    templateUrl: "./employee-dialog-data.component.html",
    styleUrls: ["../../styles/master.style.scss"],
})

export class EmployeeDialogComponent implements OnInit, OnDestroy {
    selectionEmployees: Array<IEmployee>;
    observableComments: BehaviorSubject<Array<IEmployee>>;
    location: string;
    employees: Array<IEmployee>;
    employee: IEmployee;
    locations: Array<SelectItem>;
    columns: Array<any>;
    totalRow: number;
    subscription: Subscription;
    isSingle: boolean = false;
    // element
    @ViewChild("filter") filter: ElementRef;

    constructor(
        private service: EmployeeService,
        private locationService: LocationService,
        private serviceCom: EmployeeDialogDataCommunicateService
    ) { }

    // on hook init
    ngOnInit(): void {
        this.subscription = this.serviceCom.ToChild$.subscribe(
            (condition: string) => {
                // if have condition try here
                if (condition === "Single") {
                    this.isSingle = true;;
                }
                this.onLoadData(undefined);
            });

        this.getLocationArray();

        Observable.fromEvent(this.filter.nativeElement, "keyup")
            .debounceTime(150)
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

        let filter = this.filter.nativeElement.value + "|" + (this.location ? this.location :"");
        // console.log(JSON.stringify(filter));

        let lazydata: LazyLoad = {
            First: event.first,
            Rows: event.rows,
            SortField: event.sortField,
            SortOrder: event.sortOrder,
            Filter: filter//this.filter.nativeElement.value
        };

        if (!this.columns) {
            this.columns = [
                { field: "EmpCode", header: "Code", style: { 'width': '25%' } },
                { field: "NameThai", header: "Name" },
                { field: "GroupString", header: "Group" },
                { field: "SectionString", header: "Section" },
            ];
        }

        this.service.getEmployeeWithLazyLoadByPost(lazydata)
            .subscribe(restData => {
                this.employees = restData.Data;

                //debug here
                // console.log(JSON.stringify(this.employees));

                this.totalRow = restData.TotalRow;
            }, error => console.error(error));
    }

    // on change data
    onChange(): void {
        this.onLoadData(undefined);
    }

    // on send data
    onSendDataToParent() {
        if (this.selectionEmployees) {
            this.serviceCom.ToParentArray(this.selectionEmployees);
        }
    }

    onSendDataSingeToParent(employee:IEmployee) {
        if (employee) {
            this.serviceCom.toParent(employee);
        }
    }
    // get location array
    getLocationArray(): void {
        this.locationService.getAll()
            .subscribe(result => {
                this.locations = new Array;
                this.locations.push({ label: "All Location", value: "All" });
                for (let item of result) {
                    this.locations.push({ label: item.LocateDesc, value: item.LocateId });
                }
            }, error => console.error(error));
    }
}