// angular
import { Component } from "@angular/core";
// class
import { IPlace } from "../../classes/place.class";
// component
import { AbstractViewComponent } from "../abstract/abstract-view.component";
// service
import { PlaceService } from "../../services/place.service";
import { PlaceCommunicateService } from "../../communicates/place.communicate";
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "place-view",
    templateUrl: "./place-view.component.html",
    styleUrls: ["../../styles/view.style.scss"],
})

export class PlaceViewComponent extends AbstractViewComponent<IPlace, PlaceService> {

    constructor(
        placeService: PlaceService,
        placeCommunicateService: PlaceCommunicateService,
    ) {
        super(
            placeService,
            placeCommunicateService,
        );
    }

    // on get data by key
    onGetDataByKey(place: IPlace): void {
        // clear data
        this.displayValue = {
            PlaceId: 0,
            PlaceName: ""
        };

        this.service.getOneKeyNumber(place.PlaceId)
            .subscribe(dbPlace => {
                this.displayValue = dbPlace;
            }, error => console.error(error));
    }
}