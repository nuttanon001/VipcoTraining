import { Component, ViewContainerRef } from "@angular/core";
// component
import { AbstractMaster2ndEditionComponent } from "../abstract/abstract-master-2nd-edition.component";
// classes
import { IPlace } from "../../classes/place.class";
import { LazyLoad } from "../../classes/lazyload.class";
import { LazyLoadEvent } from "primeng/primeng";
// service
import { PlaceService } from "../../services/place.service";
import { DialogsService } from "../../services/dialogs.service";
// communicate
import { PlaceCommunicateService } from "../../communicates/place.communicate";
// timezone
import * as moment from "moment-timezone";

@Component({
    selector: "place-master",
    templateUrl: "./place-master.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        PlaceService,
        PlaceCommunicateService,
        DialogsService
    ],
})

export class PlaceMasterComponent extends AbstractMaster2ndEditionComponent<IPlace, PlaceService> {
    constructor(
        placeService: PlaceService,
        placeComService: PlaceCommunicateService,
        dialogsService: DialogsService,
        viewContainerRef: ViewContainerRef,
    ) {
        super(
            placeService,
            placeComService,
            dialogsService,
            viewContainerRef
        );
    }

    // on get data with lazy load
    onGetAllWithLazyload(lazyload: LazyLoad): void {
        if (!this.columns) {
            this.columns = [
                { field: "PlaceId", header: "PlaceNo", style: { "width": "15%" }},
                { field: "PlaceName", header: "PlaceName" },
            ];
        }

        this.service.getAllWithLazyLoad(lazyload)
            .subscribe(restData => {
                this.values = restData.Data;
                this.totalRow = restData.TotalRow;
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(place: IPlace): IPlace {
        var zone = "Asia/Bangkok";
        if (place) {
            if (place.CreateDate) {
                place.CreateDate = moment.tz(place.CreateDate, zone).toDate();
            }
            if (place.ModifyDate) {
                place.ModifyDate = moment.tz(place.ModifyDate, zone).toDate();
            }
        }
        return place;
    }

    // on insert data
    onInsertToDataBase(plcae: IPlace): void {
        // change timezone
        plcae = this.changeTimezone(plcae);
        // insert data
        this.service.post(plcae).subscribe(
            (complete: any) => {
                this.displayValue = complete;
                this.onSaveComplete();
            },
            (error: any) => {
                console.error(error);
                this.editValue.Creator = undefined;
                this.canSave = true;
                this.dialogsService.error("Failed !", "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef)
            }
        );
    }

    // on update data
    onUpdateToDataBase(place: IPlace): void {
        // change timezone
        place = this.changeTimezone(place);
        // update data
        this.service.putKeyNumber(place, place.PlaceId).subscribe(
            (complete: any) => {
                this.displayValue = complete;
                this.onSaveComplete();
            },
            (error: any) => {
                console.error(error);
                this.canSave = true;
                this.dialogsService.error("Failed !", "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef)
            }
        );
    }
}