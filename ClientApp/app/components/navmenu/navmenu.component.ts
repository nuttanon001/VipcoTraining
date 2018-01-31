import { Component, OnInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { MdMenuTrigger } from "@angular/material";
// service
import { AuthService } from "../../services/auth.service";

@Component({
    selector: "nav-menu",
    templateUrl: "./navmenu.component.html",
    styleUrls: ["../../styles/navmenu.style.scss"],
})
export class NavMenuComponent implements OnInit {
    @ViewChild("mainMenu") mainMenu: MdMenuTrigger;
    @ViewChild("subMenu") subMenu: MdMenuTrigger;

    constructor(
        private authService: AuthService,
        private router: Router
    ) { }

    ngOnInit(): void {
    }

    get showLogin(): boolean {
        return !this.authService.isLoggedIn;
    }

    // On menu close
    //=============================================\\
    menuOnCloseMenu1(): void {
        this.subMenu.closeMenu();
    }

    menuOnCloseMenu2(): void {
        this.mainMenu.closeMenu();
    }

    //=============================================\\
    // On menu open
    //=============================================\\
    menuOnOpenMenu1(): void {
        this.mainMenu.openMenu();
    }

    menuOnOpenMenu2(): void {
        this.subMenu.openMenu();
    }
    //=============================================\\
    onLogOut(): void {
        this.authService.logout();
        this.router.navigate(["home"]);
    }
}

function test() {

}
