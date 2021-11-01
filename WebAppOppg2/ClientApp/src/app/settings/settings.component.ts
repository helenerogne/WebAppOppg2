import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import { Port } from "../Port";
import { TravelType } from '../TravelType';
import { PassengerType } from "../PassengerType";


@Component({
  templateUrl: "settings.component.html"
})

export class Settings {
  allPtypes: any[];
  allTtypes: any[];
  allPorts: any[];
  skjema: FormGroup;
  skjema2: FormGroup;
  skjema3: FormGroup;

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router, private route: ActivatedRoute ) {
    this.skjema = new FormGroup({travelType: new FormGroup({travelTypeName: new FormControl()})})
    this.skjema2 = new FormGroup({passengerType: new FormGroup({passengerTypeName: new FormControl(), discount: new FormControl()})})
    this.skjema3 = new FormGroup({port: new FormGroup({portName: new FormControl()})})
  }

  ngOnInit() {
    this.getAllPorts();
    this.getPtypes();
    this.getTtypes();
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

  getAllPorts() {
    this.http.get<Port[]>("api/port")
      .subscribe(ports => {
        this.allPorts = ports;
      },
        error => console.log(error)
      );
  };

  onSubmit(){
    this.addNewPort();
  }

  deleteOnePtype(id: number){
    this.http.delete("api/passengerType/"+id)
    .subscribe(retur =>{
      this.getPtypes();
      this.router.navigate(['/settings.component.html']);
    },
    error => console.log(error)
    );
  }
/*
  savePort(id: number) {
    this.http.get<Port>("api/port/" + id)
      .subscribe(
        port => {
          this.skjema.patchValue({ portID: port.portID });
          this.skjema.patchValue({ portName: port.portName });
        },
        error => console.log(error)
      );
    }*/



addNewPort(){
  /*
  const port = new Port();
  port.portName = this.skjema.value.passengerID;

  this.http.put("api/port/", changedPort)
    .subscribe(
      retur => {
        this.router.navigate(['/settings']); 
      },
      error => console.log(error)
     );*/
  }
}



