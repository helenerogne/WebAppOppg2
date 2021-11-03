import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Passenger } from '../Passenger';
import { PassengerType } from "../PassengerType";
import { Ticket } from "../Ticket";
import { PassengerModal } from './deletePassengerModal';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: "passengerList.component.html"
})

export class PassengerList {
  allPassengers: Array<Passenger>;
  allPassengerTypes: Array<PassengerType>;
  loading: boolean;
  skjema: FormGroup;
  allTickets: Array<Ticket>;
  passengerForDeleting: string;
  passengerType: any;

  validering = {
    passengerID: [''],
    firstname: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
    ],
    lastname: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
    ],
    email: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,30}")])
    ],
    passengerTypeID: [
      null, Validators.compose([Validators.required, Validators.pattern("[1-9]{1}")])
    ]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router, private route: ActivatedRoute, private modalService: NgbModal) {
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
    savedPas.passengerTypeID = Number(this.skjema.value.passengerTypeID);

    this.http.post("api/passenger", savedPas)
      .subscribe(retur => {
        this.getAllPassengers();
        this.clearFrom();
      },
        error => console.log(error)
      );
  };

  clearFrom(){
    this.skjema.reset();
  }

  delete(id: number){
    this.getAllTickets(id)
  }

  getAllTickets(id: number) {
    this.http.get<Ticket[]>("api/order")
      .subscribe(tickets => {
        this.allTickets = tickets;
        this.checkTickets(id);
        this.loading = false;
      },
        error => console.log(error)
      );
  };

  checkTickets(id: number){
    let hasTickets = this.allTickets.find(x => x.passengerID === id);
    if(!hasTickets){
      this.deleteOnePassenger(id);
    }else{
      console.log("passesjer har illett");
    }
  }

  deleteOnePassenger(id: number) {
    this.http.get<Passenger>("api/passenger/" + id)
    .subscribe(passenger => {
      this.passengerForDeleting = passenger.firstname + " " + passenger.lastname;
      this.showModalandDelete(id);
    },
      error => console.log(error)
    );
  }

  showModalandDelete(id: number) {
    const modalRef = this.modalService.open(PassengerModal);
    modalRef.componentInstance.name = this.passengerForDeleting;
    modalRef.result.then(retur => {
      console.log('Lukket med:' + retur);
      if (retur == "Slett") {
        this.http.delete("api/passenger/" + id)
          .subscribe(retur => {
            this.getAllPassengers();
            this.router.navigate(['/passengerList.component.html']);
          },
            error => console.log(error)
          );
      }
      this.router.navigate(['/passengerList']);
    });
  }

}
