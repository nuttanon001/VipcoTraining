// angular
import { Component } from "@angular/core";
// class
import { IEducation } from "../../classes/education.class";
// component
import { AbstractViewComponent } from "../abstract/abstract-view.component";
// service
import { EducationService } from "../../services/education.service";
import { EducationCommunicateService } from "../../communicates/education.communicate"
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "education-view",
    templateUrl: "./education-view.component.html",
    styleUrls: ["../../styles/view.style.scss"],
})

export class EducationViewComponent
    extends AbstractViewComponent<IEducation, EducationService> {

    constructor(
        educationService: EducationService,
        educationServiceCom: EducationCommunicateService,
    ) {
        super(
            educationService,
            educationServiceCom,
        );
    }

    // on get data by key
    onGetDataByKey(education: IEducation): void {

        // clear data
        this.displayValue = {
            EducationId: 0,
            EducationName: ""
        };

        this.service.getOneKeyNumber(education.EducationId)
            .subscribe(dbEducation => {
                this.displayValue = dbEducation;
            }, error => console.error(error));
    }
}