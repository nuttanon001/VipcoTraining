import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
// classes
import { IGroup } from "../classes/group.class";
// seriver
import { AbstractRestService } from "./abstract-rest.service";
@Injectable()
export class GroupService extends AbstractRestService<IGroup>{
    constructor(http: Http) {
        super(http, "api/GroupName/");
    }
}