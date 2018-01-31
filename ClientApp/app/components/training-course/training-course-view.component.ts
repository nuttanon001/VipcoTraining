// angular
import { Component } from "@angular/core";
// class
import { ITrainingCousre } from "../../classes/training-cousre.class";
import { IAttachFile } from "../../classes/attact-file.class";
// component
import { AbstractViewComponent } from "../abstract/abstract-view.component";
// service
import { TrainingCourseService } from "../../services/training-course.service";
import { TrainingCousreCommunicateService } from "../../communicates/training-cousre.communicate"
// rxjs
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "training-course-view",
    templateUrl: "./training-course-view.component.html",
    styleUrls: ["../../styles/view.style.scss"],
})

export class TrainingCourseViewComponent
    extends AbstractViewComponent<ITrainingCousre, TrainingCourseService> {

    attachFiles: Array<IAttachFile> = new Array;

    constructor(
        trainingCourseService: TrainingCourseService,
        trainingCourseComService: TrainingCousreCommunicateService,
    ) {
        super(
            trainingCourseService,
            trainingCourseComService,
        );
    }

    // on get data by key
    onGetDataByKey(trainingCourse: ITrainingCousre): void {
        // clear data
        this.displayValue = {
            TrainingCousreId: 0,
            TrainingCousreCode: "",
            TrainingCousreName: "",
        };

        this.service.getOneKeyNumber(trainingCourse.TrainingCousreId)
            .subscribe(dbTrainingCourse => {
                if (dbTrainingCourse) {
                    this.service.getAttachFile(dbTrainingCourse.TrainingCousreId)
                        .subscribe(dbAttach => this.attachFiles = dbAttach.slice());
                }
                this.displayValue = dbTrainingCourse;
            }, error => console.error(error));
    }
    // open attact file
    onOpenNewLink(link: string): void {
        if (link) {
            window.open(link, "_blank");
        }
    }
}