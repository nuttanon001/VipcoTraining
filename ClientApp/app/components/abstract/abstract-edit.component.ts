﻿// angular
import { OnInit, OnDestroy, ViewContainerRef } from "@angular/core";
import { FormGroup } from "@angular/forms";
// rxjs
import { Subscription } from 'rxjs/Subscription';

export abstract class AbstractEditComponent<Class,Service> implements OnInit, OnDestroy {
    editValue: Class;
    editValueForm: FormGroup;
    subscription: Subscription;

    constructor(
        protected service: Service,
        protected serviceCom: any,
    ) { }

    // on hook component
    ngOnInit(): void {
        this.subscription = this.serviceCom.toChildEdit$.subscribe(
            (editValue: Class) => this.onGetDataByKey(editValue));
    }

    // on hook component
    ngOnDestroy(): void {
        // prevent memory leak when component destroyed
        this.subscription.unsubscribe();
    }

    // on get data by key
    abstract onGetDataByKey(value: Class): void;

    // define data for edit form
    abstract defineData(): void;

    // build form
    abstract buildForm(): void;

    // on value of form change
    onValueChanged(data?: any): void {
        if (!this.editValueForm) { return; }
        const form = this.editValueForm;
        // on form valid or not
        this.onFormValid(form.valid);
    }

    // on valid data
    onFormValid(isValid: boolean): void {
        this.editValue = this.editValueForm.value;
        this.serviceCom.toParent([this.editValue, isValid]);
    }
}