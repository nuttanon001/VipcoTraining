import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TestComponent } from "../components/test/test.component";
import { Test2Component } from "../components/test/test2.component";

import { TestRoutingModule } from "../routes/test.routing.module";
@NgModule({
    imports: [
        CommonModule,
        TestRoutingModule
    ],
    declarations: [
        TestComponent,
        Test2Component
    ],
})
export class TestModule{ }