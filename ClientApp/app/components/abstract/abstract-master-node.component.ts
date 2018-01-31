import { OnInit, ElementRef, ViewChild, ViewContainerRef } from "@angular/core";
import "rxjs/Rx";
import { Observable } from "rxjs/Observable"
// classes
import { LazyLoad } from "../../classes/lazyload.class";
import { LazyLoadEvent,TreeNode } from "primeng/primeng";
//services
import { DialogsService } from "../../services/dialogs.service";

export abstract class AbstractMasterNodeComponent<Interface, Service> implements OnInit {
    selectNode: TreeNode | undefined;
    editValue: any;
    nodes: Array<TreeNode>;
    columns: Array<any>;
    totalRow: number;
    scrollHeight: string;
    // boolean event
    _showEdit: boolean;
    canSave: boolean;
    hideleft: boolean;

    constructor(
        protected service: Service,
        protected serviceCom: any,
        protected dialogsService: DialogsService,
        protected viewContainerRef: ViewContainerRef,
    ) { }

    // property
    get DisplayDataNull(): boolean {
        return this.selectNode === undefined;
    }

    get ShowEdit(): boolean {
        if (this._showEdit != null)
            return this._showEdit;
        else
            return false;
    }

    set ShowEdit(showEdit: boolean) {
        if (showEdit !== this._showEdit) {
            this.hideleft = !showEdit;
            this._showEdit = showEdit;
        }
    }

    // angular hook
    ngOnInit(): void {
        if (window.innerWidth >= 1600) {
            this.scrollHeight = 70 + "vh";
        } else if (window.innerWidth > 1360 && window.innerWidth < 1600) {
            this.scrollHeight = 65 + "vh";
        } else {
            this.scrollHeight = 60 + "vh";
        }

        this.ShowEdit = false;
        this.canSave = false;

        this.serviceCom.ToParent$.subscribe(
            (TypeValue: [Interface, boolean]) => {
                this.editValue = TypeValue[0];
                this.canSave = TypeValue[1];
            });

        this.onGetAll();
    }
    // on get all with lazyload
    abstract onGetAll(): void;

    // on detail view
    onDetailView(): void {
        if (this.selectNode && this.selectNode.data) {
            const data = this.selectNode.data;
            setTimeout(() => this.serviceCom.toChild(data), 500);
        }
    }

    // on detail edit
    onDetailEdit(editValue?: TreeNode): void {
        this.selectNode = editValue;
        this.ShowEdit = true;
        let editData: Interface|undefined;

        if (this.selectNode)
            editData = this.selectNode.data;
        else
            editData = undefined;

        setTimeout(() => this.serviceCom.toChildEdit(editData), 1000);
    }

    // on cancel edit
    onCancelEdit(): void {
        this.editValue = undefined;
        this.selectNode = undefined;
        this.canSave = false;
        this.ShowEdit = false;
        this.onDetailView();
    }

    // on submit
    onSubmit(): void {
        this.canSave = false;
        if (this.editValue.Creator) {
            this.onUpdateToDataBase(this.editValue);
        } else {
            this.onInsertToDataBase(this.editValue);
        }
    }

    // change data to timezone
    abstract changeTimezone(value: Interface): Interface;

    // on insert data
    abstract onInsertToDataBase(value: Interface): void;

    // on update data
    abstract onUpdateToDataBase(value: Interface): void;

    // on save complete
    onSaveComplete(): void {
        this.dialogsService
            .context("System message", "Save completed.", this.viewContainerRef)
            .subscribe(result => {
                this.canSave = false;
                this.ShowEdit = false;
                this.onDetailView();
                this.editValue = undefined;
                this.onGetAll();
            });
    }
}