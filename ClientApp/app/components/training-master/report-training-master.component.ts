import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

@Component({
    templateUrl: "./report-training-master.component.html",
    styleUrls: ["../../styles/center.style.scss"],
})
export class ReportTrainingMasterComponent implements OnInit {
    constructor(private route: ActivatedRoute, private router: Router,) { }

    ngOnInit(): void {
        this.route.params.subscribe((params: any) => {
            let key: string = params["condition"];
            // debug here
            console.log("Key is :", key);

            if (key === "EmployeeReport") {
                this.router.navigate(["training-master/employee-report","open"]);
            }
            else if (key === "CostReport") {
                this.router.navigate(["training-master/cost-report", "open"]);
            }
            else {
                this.router.navigate(["training-master/report", key]);
            }
        }, error => console.error(error));
    }
}