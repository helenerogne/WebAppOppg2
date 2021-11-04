import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Route } from "../Route";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: "routeList.component.html"
})

export class routeList {
  skjema: FormGroup;
  allRoutes: Array<Route>
  loading: boolean;

  validering = {
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router, private route: ActivatedRoute, private modalService: NgbModal) {
    this.skjema = fb.group(this.validering);
  }

  ngOnInit() {
    this.GetAllRoutes();
  }
  onSubmit() {
    this.saveRoute();
  }

  GetAllRoutes() {
    this.http.get<Route[]>("api/route")
      .subscribe(routes => {
        this.allRoutes = routes;
        this.loading = false;
      },
        error => console.log(error)
      );
  };

  saveRoute() {
    const lagretRoute = new Route();

    lagretRoute.portFrom = this.skjema.value.portFrom;
    lagretRoute.portTo = this.skjema.value.portTo;
    lagretRoute.departure = this.skjema.value.departure;
    lagretRoute.ticketDate = this.skjema.value.ticketDate;

    console.log(lagretRoute);

    this.http.post("api/route", lagretRoute)
      .subscribe(retur => {
        this.router.navigate(['/routeList]']);
      },
        error => console.log(error)
      );
  };

}
