// angular
import { Component } from "@angular/core";
import { FormBuilder, FormControl, Validators } from "@angular/forms";
// class
import { IPlace } from "../../classes/place.class";
// component
import { AbstractEditComponent } from "../abstract/abstract-edit.component";
// service
import { PlaceService } from "../../services/place.service";
import { PlaceCommunicateService } from "../../communicates/place.communicate";
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "place-edit",
    templateUrl: "./place-edit.component.html",
    styleUrls: ["../../styles/edit.style.scss"],
})

export class PlaceEditComponent
    extends AbstractEditComponent<IPlace, PlaceService> {
    constructor(
        placeService: PlaceService,
        placeCommunicateServiceCom: PlaceCommunicateService,
        private fb: FormBuilder,
    ) {
        super(
            placeService,
            placeCommunicateServiceCom,
        );
    }

    // on get data by key
    onGetDataByKey(place?: IPlace): void {
        if (place) {
            this.service.getOneKeyNumber(place.PlaceId)
                .subscribe(dbPlace => {
                    this.editValue = dbPlace;
                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                PlaceId: 0,
                PlaceName: ""
            };
            this.defineData()
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            PlaceId: [this.editValue.PlaceId],
            PlaceName: [this.editValue.PlaceName,
                [
                    Validators.required,
                    Validators.minLength(1),
                    Validators.maxLength(200),
                ]
            ],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate]
        });
        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
    }
}

