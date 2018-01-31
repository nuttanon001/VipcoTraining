import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
// classes
import { ISupplementCourse } from "../classes/supplement-course.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class SupplementCourseService extends AbstractRestService<ISupplementCourse>{
    constructor(http: Http) {
        super(http, "api/SupplementCourse/");
    }

    // get by master id
    getByMasterId(id: number): Observable<Array<ISupplementCourse>> {
        let url: string = this.actionUrl + "GetByProgramID/" + id;
        // debug here
        // console.log(url);
        return this.http.get(url)
            .map(this.extractData).catch(this.handleError);
    }
}