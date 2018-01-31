import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { Observable } from "rxjs/Observable";
// classes
import { IAttachFile } from "../classes/attact-file.class";
import { ITrainingCousre } from "../classes/training-cousre.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
import { AuthService } from "./auth.service";

@Injectable()
export class TrainingCourseService extends AbstractRestService<ITrainingCousre>{
    constructor(
        http: Http,
        private authService: AuthService
    ) {
        super(http, "api/TrainingCousre/");
    }

    testMethod(): void {
        //GetTest
        let url: string = `${this.actionUrl}GetTest/`;
        this.http.get(url).subscribe(data => console.log("Test:", data), error => console.error(error));
    }
    //===================== Upload File ===============================\\
    // get file
    getAttachFile(TrainingCourseId: number): Observable<Array<IAttachFile>> {
        let url: string = `${this.actionUrl}GetAttach/${TrainingCourseId}/`;
        return this.http.get(url)
            .map(this.extractData).catch(this.handleError);
    }

    // upload file
    postAttactFile(TrainingCourseId: number, files: FileList):Observable<any> {
        let input = new FormData();

        for (var i = 0; i < files.length; i++) {
            if (files[i].size <= 5242880)
                input.append("files", files[i]);
        }

        // debug here
        //console.log("(TrainingCoirseService) get auth var:", this.authService.getAuth);

        let CreateBy: string = this.authService.userName || "Someone";
        let url: string = `${this.actionUrl}PostAttach/${TrainingCourseId}/${CreateBy}`;
        return this.http.post(url, input).map(this.extractData).catch(this.handleError);
    }

    // delete file
    deleteAttactFile(AttachId: number): Observable<any> {
        let url: string = this.actionUrl + "DeleteAttach/" + AttachId;
        return this.http.delete(url).catch(this.handleError);
    }

    //===================== End Upload File ===========================\\
}