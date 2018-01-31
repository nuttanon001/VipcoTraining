import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
// classes
import { IStandardCourse } from "../classes/standard-course.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class StandardCourseService extends AbstractRestService<IStandardCourse>{
    constructor(http: Http) {
        super(http, "api/StandardCourse/");
    }

    // get by master id
    getByMasterId(id: number): Observable<Array<IStandardCourse>> {
        let url: string = this.actionUrl + "GetByProgramID/" + id;
        // debug here
        // console.log(url);
        return this.http.get(url)
            .map(this.extractData).catch(this.handleError);
    }
}