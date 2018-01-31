import { Injectable } from "@angular/core";
import { Subject } from "rxjs/Subject";
// classes
import { ITrainingMaster } from "../classes/training-master.class";
// services
import { AbstractCommunicateService } from "./abstract-com.communicate";


@Injectable()
export class TrainingMasterCommunicateService
    extends AbstractCommunicateService<ITrainingMaster> {
    constructor() {
        super();
    }

    // Observable string sources
    private ReportSource = new Subject<any>();
    // Observable string streams
    ToSender$ = this.ReportSource.asObservable();
    // Service message commands
    toSender(condition: any): void {
        this.ReportSource.next(condition);
    }
}