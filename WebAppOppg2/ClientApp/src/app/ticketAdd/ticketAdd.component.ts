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

  validering = {
    id: [''],
    passengerID: [''],
    firstname: [null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])],
    lastname: [null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])],
    email: [''],
    passengerType: [''],
    routeID: [''],
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
    /*
    this.route.params.subscribe(params => {
      this.getTicket(params.id);
    })*/
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

  onSubmit() {
    this.saveTicket();
  }

saveTicket(){
  const ticket = new Ticket();
  ticket.passengerID = 10;
  ticket.firstname = this.skjema.value.firstname;
  ticket.lastname = this.skjema.value.lastname;
  ticket.email = this.skjema.value.email;
  ticket.passengerType = this.skjema.value.passengerTypeName;
  ticket.routeID = this.skjema.value.routeID;
  ticket.travelType = this.skjema.value.travelTypeName;
  ticket.routeTo = "Bergen"
  ticket.routeFrom = "kristiansand"
  ticket.departure = this.skjema.value.departure;
  ticket.ticketDate = this.skjema.value.ticketDate;
  ticket.price = this.skjema.value.price;

  this.http.put("api/order/", ticket)
    .subscribe(
      retur => {
        this.router.navigate(['/ticketList']); 
      },
      error => console.log(error)
     );
  }
}


