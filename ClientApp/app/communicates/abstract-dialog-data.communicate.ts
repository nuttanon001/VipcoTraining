import { Subject } from "rxjs/Subject";

export abstract class AbstractDialogDataCommunicateService<TInterface> {

    // Observable string sources
    private ParentSource = new Subject<TInterface>();
    private ParentSourceArray = new Subject<Array<TInterface>>();
    private ChildSource = new Subject<any>();

    // Observable string streams
    ToParent$ = this.ParentSource.asObservable();
    ToParentArray$ = this.ParentSourceArray.asObservable();
    ToChild$ = this.ChildSource.asObservable();

    // Service message commands
    toParent(returnValue: TInterface): void {
        this.ParentSource.next(returnValue);
    }

    ToParentArray(returnArrayValue: Array<TInterface>): void {
        this.ParentSourceArray.next(returnArrayValue);
    }

    toChild(condition: any): void {
        this.ChildSource.next(condition);
    }
}