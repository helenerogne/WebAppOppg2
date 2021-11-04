import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import { Ticket } from "../Ticket";
import { Route } from "../Route";
import { Passenger } from '../Passenger';
import { PassengerType } from "../PassengerType";
import { TravelType } from "../TravelType";


@Component({
  templateUrl: "ticketAdd.component.html"
})

export class ticketAdd {
  skjema: FormGroup;
  allPtypes: any[];
  allTtypes: any[];
  routes: any = [];
  midRoute: any = {};

  validering = {
    id: [''],
    passengerID: [''],
    firstname: [null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])],
    lastname: [null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])],
    email: [null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,30}")])],
    passengerType: [''],
    routeID: [''],
    travelTypeID: [''],
    travelType: [''],
    routeFrom: [''],
    routeTo: [''],
    departure: [''],
    ticketDate: [''],
    price: [null, Validators.compose([Validators.required, Validators.pattern("[0-9]{1,9999}")])]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router, private route: ActivatedRoute ) {
    this.skjema = fb.group(this.validering);
  }

  ngOnInit() {
    this.getPtypes();
    this.getTtypes();
    this.getRoutes();

  }

  getPtypes() {
    this.http.get<PassengerType[]>("api/passengerType")
      .subscribe(passengerTypes => {
        this.allPtypes = passengerTypes;
      },
        error => console.log(error)
      );
  };

  getTtypes() {
    this.http.get<TravelType[]>("api/travelType")
      .subscribe(travelTypes => {
        this.allTtypes = travelTypes;
      },
        error => console.log(error)
      );
  };

   
getRoutes() {
  this.http.get("api/route/")
    .subscribe(
      routes => {
        this.routes = routes;
      },
      error => console.log(error)
    );
}

getRoute(routeId) {
  return new Promise(function(resolve, reject){
    this.http.get("api/route/" + routeId)
      .subscribe(
        route => {
          resolve(route);
        },
        error => reject(error)
      );
    }.bind(this));
}

  onSubmit() {
    this.saveTicket();
  }

saveTicket(){
  const ticket = new Ticket();
  ticket.firstname = this.skjema.value.firstname;
  ticket.lastname = this.skjema.value.lastname;
  ticket.email = this.skjema.value.email;
  ticket.passengerTypeID = Number(this.skjema.value.passengerType);
  ticket.travelTypeID = Number(this.skjema.value.travelType);
  ticket.routeID = Number(this.skjema.value.routeID);
  ticket.ticketDate = this.skjema.value.ticketDate;

  this.http.post("api/order/", ticket)
    .subscribe(
      retur => {
        this.router.navigate(['/ticketList']); 
      },
      error => console.log(error)
     );
  }

  async onRouteChange(){
    let routeId = this.skjema.value.routeID;
    this.midRoute  = await this.getRoute(routeId);
    this.skjema.patchValue({departure: this.midRoute.departure});
    this.skjema.patchValue({price: this.midRoute.routePrice});
  } 
}



