import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Passenger } from '../Passenger';
import { PassengerType } from "../PassengerType";

@Component({
  templateUrl: "passengerEdit.component.html"
})

export class passengerEdit {
  allPassengers: Array<Passenger>;
  allPassengerTypes: Array<PassengerType>;
  loading: boolean;
  skjema: FormGroup;
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

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router, private route: ActivatedRoute) {
    this.skjema = fb.group(this.validering);
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.getPassenger(params.id);
    })
    this.getAllPassengerTypes();
  }


  getAllPassengerTypes() {
    this.http.get<PassengerType[]>("api/passengerType")
      .subscribe(passengerTypes => {
        this.allPassengerTypes = passengerTypes;
        this.loading = false;
      },
        error => console.log(error)
      );
  };

  getPassenger(id: number) {
    this.http.get<Passenger>("api/passenger/" + id)
      .subscribe(
        passenger => {
          this.skjema.patchValue({ passengerID: passenger.passengerID });
          this.skjema.patchValue({ firstname: passenger.firstname });
          this.skjema.patchValue({ lastname: passenger.lastname });
          this.skjema.patchValue({ email: passenger.email });
          this.skjema.patchValue({ passengerTypeID: passenger.passengerTypeID });
        },
        error => console.log(error)
      );
    }

  onSubmit() {
    this.changeOnePassenger();
  }

  changeOnePassenger(){
    const changedPassenger = new Passenger();
    changedPassenger.passengerID = this.skjema.value.passengerID;
    changedPassenger.firstname = this.skjema.value.firstname;
    changedPassenger.lastname = this.skjema.value.lastname;
    changedPassenger.email = this.skjema.value.email;
    changedPassenger.passengerTypeID = Number(this.skjema.value.passengerTypeID);
    this.http.put("api/passenger/", changedPassenger)
      .subscribe(
        retur => {
          this.router.navigate(['/passengerList']); 
        },
        error => console.log(error)
       );
    }
}
