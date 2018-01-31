import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { TestComponent } from "../components/test/test.component";
import { Test2Component } from "../components/test/test2.component";

const testRoutes: Routes = [
    {
        path: 'test',
        component: TestComponent,
        children: [
            {
                path: "",
                component: Test2Component,
            }
        ]
    },
    { path: 'test2', component: Test2Component }
];

@NgModule({
    imports: [
        RouterModule.forChild(testRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class TestRoutingModule { }