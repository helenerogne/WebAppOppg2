import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Passenger } from '../Passenger';
import { PassengerType } from "../PassengerType";
import { Ticket } from "../Ticket";

@Component({
  templateUrl: "passengerList.component.html"
})

export class PassengerList {
  allPassengers: Array<Passenger>;
  allPassengerTypes: Array<PassengerType>;
  loading: boolean;
  skjema: FormGroup;
  allTickets: Array<Ticket>;

  validering = {
    id: [''],
    firstname: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
    ],
    lastname: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
    ],
    email: [''],
    passengerType: [''],
    passengerTypeID: ['']
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router, private route: ActivatedRoute) {
    this.skjema = fb.group(this.validering);
  }

  ngOnInit() {
    this.loading = true;
    this.getAllPassengers();
    this.getAllPassengerTypes();
  }


  getAllPassengers() {
    this.http.get<Passenger[]>("api/passenger")
      .subscribe(passengers => {
        this.allPassengers = passengers;
        this.loading = false;
      },
        error => console.log(error)
      );
  };

  getAllPassengerTypes() {
    this.http.get<PassengerType[]>("api/passengerType")
      .subscribe(passengerTypes => {
        this.allPassengerTypes = passengerTypes;
        this.loading = false;
      },
        error => console.log(error)
      );
  };

  onSubmit() {
    this.savePassenger();
  }

  savePassenger() {
    const savedPas = new Passenger();

    savedPas.firstname = this.skjema.value.firstname;
    savedPas.lastname = this.skjema.value.lastname;
    savedPas.email = this.skjema.value.email;
    savedPas.passengerType = this.skjema.value.passengerTypeID;

    //this.getAllTickets();

    this.http.post("api/passenger", savedPas)
      .subscribe(retur => {
        this.allPassengers.push(savedPas)
        //this.router.navigate(['/passengerList']);

      },
        error => console.log(error)
      );
  };

  deleteOnePassenger(id: number) {
    this.http.delete("api/passenger/" + id)
      .subscribe(retur => {
        this.getAllPassengers();
        this.router.navigate(['/passengerList.component.html']);
      },
        error => console.log(error)
      );
  };

  //hvis passasjer slettes så skal alle tickets tilhørende også slettes

/*
  getAllTickets() {
    this.http.get<Ticket[]>("api/order")
      .subscribe(tickets => {
        this.allTickets = tickets;
        this.loading = false;
      },
        error => console.log(error)
      );
  };

  deleteOneTicket(id: number) {
    this.http.delete("api/order/" + id)
      .subscribe(retur => {
        this.getAllTickets();
        this.router.navigate(['/admin.component.html']);
      },
        error => console.log(error)
      );
  };

  deleteTicketsOfPassenger(id: number) {
    if(id)


    for (var ticket of tickets) {
      this.deleteOneTicket(ticket.id)
    }
  }*/

}
