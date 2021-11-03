import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import { Port } from "../Port";
import { TravelType } from '../TravelType';
import { Route } from '../Route';
import { PassengerType } from "../PassengerType";
import { PortModal } from './deletePortModal';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: "settings.component.html"
})

export class Settings {
  allPtypes: any[];
  allTtypes: any[];
  allPorts: any[];
  allRoutes: Array<Route>;
  skjema: FormGroup;
  skjema2: FormGroup;
  skjema3: FormGroup;
  saveButton: boolean;
  portForDeleting: string;
  passengertypeForDeleting: string;
  traveltypeForDeleting: string;

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router, private route: ActivatedRoute,private modalService: NgbModal ) {
    this.skjema = new FormGroup({travelType: new FormGroup({travelTypeName: new FormControl()})})
    this.skjema2 = new FormGroup({passengerType: new FormGroup({passengerTypeName: new FormControl(), discount: new FormControl()})})
    this.skjema3 = new FormGroup({port: new FormGroup({portName: new FormControl()})})
  }

  ngOnInit() {
    this.getAllPorts();
    this.getPtypes();
    this.getTtypes();
    this.getAllRoutes();
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
    this.saveButton = false;
    this.http.get<Port[]>("api/port")
      .subscribe(ports => {
        this.allPorts = ports;
      },
        error => console.log(error)
      );
  };

  getAllRoutes() {
    this.http.get<Route[]>("api/route")
      .subscribe(allroutes => {
        this.allRoutes = allroutes;
      },
        error => console.log(error)
      );
  };

  addNewFieldPort(){
    const blank = new Port();
    blank.isNew = true;
    this.allPorts.push(blank)
    //this.changeButton();
    /*
    this.http.put("api/port/", blank)
    .subscribe(
      retur =>{
        this.allPorts.push(blank)
      },
      error => console.log(error)
    );*/
  }

  changeButton(){
    this.saveButton = true;
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
      this.sendNewPort(this.skjema3.value.port.portName)
    }
    
sendNewPort(port: String){
  this.http.post("api/port", {portName: port})
    .subscribe(
      retur => {
        this.allPorts[this.allPorts.length - 1].isNew = false;
      },
      error => console.log(error)
     );
  }







  deleteOnePort(port) {
    this.http.get<Port>("api/port/" + port)
    .subscribe(port => {
      this.portForDeleting = port.portName;
      this.checkRoutes(port.portID);
      
      //this.showModalandDelete(port.portID);
    },
      error => console.log(error)
    );
  }

  checkRoutes(port){
    let isPortFrom = this.allRoutes.find(x => x.portFrom === port.portName);
    let isPortTo = this.allRoutes.find(x => x.portTo === port.portName);

    if(!isPortFrom || !isPortTo){
      this.portForDeleting = port.portName;
      this.showModalandDelete(port.portID);
    }else{
      console.log("port tilhører en rute");
    }
  }

  showModalandDelete(id: number) {
    const modalRef = this.modalService.open(PortModal);
    modalRef.componentInstance.name = this.portForDeleting;
    modalRef.result.then(retur => {
      console.log('Lukket med:' + retur);
      if (retur == "Slett") {
        this.http.delete("api/port/" + id)
          .subscribe(retur => {
            this.getAllPorts();
            this.router.navigate(['/settings.component.html']);
          },
            error => console.log(error)
          );
      }
      this.router.navigate(['/settings']);
    });
  }

}
