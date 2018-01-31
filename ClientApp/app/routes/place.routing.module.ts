import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
// componentes
import { PlaceMasterComponent } from "../components/place/place-master.component";
import { PlaceCenterComponent } from "../components/place/place-center.component";
// service
import { AuthGuard } from "../services/auth-guard.service";

const placeRoutes: Routes = [
    {
        path: "place",
        component: PlaceCenterComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                component: PlaceMasterComponent,
            }
        ]
    }
]

@NgModule({
    imports: [
        RouterModule.forChild(placeRoutes)
    ],
    exports: [
        RouterModule
    ]
})

export class PlaceRouters { }