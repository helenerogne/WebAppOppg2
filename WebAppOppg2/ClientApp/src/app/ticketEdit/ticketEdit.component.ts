import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import { Ticket } from "../Ticket";
import { Route } from "../Route";
import { Passenger } from '../Passenger';
import { PassengerType } from "../PassengerType";
import { TravelType } from '../TravelType';


@Component({
  templateUrl: "ticketEdit.component.html"
})

export class TicketEdit {
  allTickets: Array<Ticket>;
  skjema: FormGroup;
  routes: any = [];
  allPtypes: any[];
  allTtypes: any[];
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
  t
  ngOnInit() {
    this.getRoutes();
    this.getTtypes();
    this.route.params.subscribe(params => {
      this.getTicket(params.id);
    })
  }

  onSubmit() {
    this.changeOneTicket();
  }

  getTicket(id: number) {
    this.http.get<Ticket>("api/order/" + id)
      .subscribe(
        ticket => {
          this.skjema.patchValue({ id: ticket.ticketID });
          this.skjema.patchValue({ passengerID: ticket.passengerID });
          this.skjema.patchValue({ firstname: ticket.firstname });
          this.skjema.patchValue({ lastname: ticket.lastname });
          this.skjema.patchValue({ email: ticket.email });
          this.skjema.patchValue({ passengerType: ticket.passengerType });
          this.skjema.patchValue({ routeID: ticket.routeID });
          this.skjema.patchValue({ travelType: ticket.travelType });
          this.skjema.patchValue({ travelTypeID: ticket.travelTypeID });
          this.skjema.patchValue({ routeTo: ticket.routeTo });
          this.skjema.patchValue({ routeFrom: ticket.routeFrom });
          this.skjema.patchValue({ departure: ticket.departure });
          this.skjema.patchValue({ ticketDate: ticket.ticketDate });
          this.skjema.patchValue({ price: ticket.price });
        },
        error => console.log(error)
    );
    }
 
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

getTtypes() {
  this.http.get<TravelType[]>("api/travelType")
    .subscribe(travelTypes => {
      this.allTtypes = travelTypes;
    },
      error => console.log(error)
    );
};

changeOneTicket(){
  const changedTicket = new Ticket();
  changedTicket.ticketID = Number(this.skjema.value.id);
  changedTicket.passengerID = Number(this.skjema.value.passengerID);
  changedTicket.firstname = this.skjema.value.firstname;
  changedTicket.lastname = this.skjema.value.lastname;
  changedTicket.email = this.skjema.value.email;
  changedTicket.passengerType = this.skjema.value.passengerType;
  changedTicket.routeID = Number(this.skjema.value.routeID);
  changedTicket.travelTypeID = Number(this.skjema.value.travelTypeID);
  changedTicket.travelType = this.skjema.value.travelType;
  changedTicket.routeTo = this.skjema.value.routeTo;
  changedTicket.routeFrom = this.skjema.value.routeFrom;
  changedTicket.departure = this.skjema.value.departure;
  changedTicket.ticketDate = this.skjema.value.ticketDate;
  changedTicket.price = this.skjema.value.price;

  this.http.put("api/order/", changedTicket)
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



