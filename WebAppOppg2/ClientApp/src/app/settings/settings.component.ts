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
  savePort: boolean;
  saveP: boolean;
  saveT: boolean;
  portForDeleting: string;
  passengertypeForDeleting: string;
  traveltypeForDeleting: string;

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router, private route: ActivatedRoute,private modalService: NgbModal ) {
    this.skjema = new FormGroup({travelType: new FormGroup({travelTypeName: new FormControl(), travelTypeID: new FormControl()})})
    this.skjema2 = new FormGroup({passengerType: new FormGroup({passengerTypeName: new FormControl(), discount: new FormControl(), passengerTypeID: new FormControl()})})
    this.skjema3 = new FormGroup({port: new FormGroup({portName: new FormControl(), portID: new FormControl()})})
  }

  ngOnInit() {
    this.getAllPorts();
    this.getPtypes();
    this.getTtypes();
    this.getAllRoutes();
    this.saveP = true;
    this.saveT = true;
    this.savePort = true;
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

  getAllRoutes() {
    this.http.get<Route[]>("api/route")
      .subscribe(allroutes => {
        this.allRoutes = allroutes;
      },
        error => console.log(error)
      );
  };

  //Traveltype add and edit

  addNewFieldTtype(){
    const blank = new TravelType();
    blank.isNew = true;
    this.saveT = false;
    this.allTtypes.push(blank)
  }

  addNewTraveltype(){
    const newTtype = new TravelType();
    newTtype.travelTypeName = this.skjema.value.travelType.travelTypeName;
    this.saveT = true;
    this.sendNewTraveltype(newTtype);
  }
    
  sendNewTraveltype(travelType){
    this.http.post("api/travelType", travelType)
    .subscribe(
      retur => {
        this.allTtypes[this.allTtypes.length - 1].isNew = false;
      },
      error => console.log(error)
     );
  }

  changeOneTtype(travelTypeID){
    const t = new TravelType();
    t.travelTypeID = Number(travelTypeID);
    t.travelTypeName = this.skjema.value.travelType.travelTypeName;
    this.http.put("api/travelType/", t)
      .subscribe(
        retur => {
          this.router.navigate(['/settings']); 
        },
        error => console.log(error)
       );
    }


  //Passengertype add and edit

  addNewFieldPtype(){
    const blank = new PassengerType();
    blank.isNew = true;
    this.saveP = false;
    this.allPtypes.push(blank)
  }

  addNewPassengertype(){
    const newPtype = new PassengerType();
    newPtype.passengerTypeName = this.skjema2.value.passengerType.passengerTypeName;
    newPtype.discount = Number(this.skjema2.value.passengerType.discount);
    this.saveP = true;
    this.sendNewPassengertype(newPtype);
  }
    
  sendNewPassengertype(passengerType){
    this.http.post("api/passengerType", passengerType)
    .subscribe(
      retur => {
        this.allPtypes[this.allPtypes.length - 1].isNew = false;
      },
      error => console.log(error)
     );
  }

  changeOnePtype(passengerTypeID){
    const t = new PassengerType();
    t.passengerTypeID = Number(passengerTypeID);
    t.passengerTypeName = this.skjema2.value.passengerType.passengerTypeName;
    t.discount = Number(this.skjema2.value.passengerType.discount);
    this.http.put("api/passengerType/", t)
      .subscribe(
        retur => {
          this.router.navigate(['/settings']); 
        },
        error => console.log(error)
       );
    }



  //Port add and edit

  addNewFieldPort(){
    const blank = new Port();
    blank.isNew = true;
    this.savePort = false;
    this.allPorts.push(blank)
  }

  addNewPort(){
    this.savePort = true;
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

    changeOnePort(portID){
      const t = new Port();
      t.portID = Number(portID);
      t.portName = this.skjema3.value.port.portName;
      this.http.put("api/port/", t)
        .subscribe(
          retur => {
            this.router.navigate(['/settings']); 
          },
          error => console.log(error)
         );
      }




/*
//Delete passengertype

  deleteOnePtype(id: number){
    this.http.delete("api/passengerType/"+id)
    .subscribe(retur =>{
      this.getPtypes();
      this.router.navigate(['/settings.component.html']);
    },
    error => console.log(error)
    );
  }


//Delete port

  deleteOnePort(port) {
    this.http.get<Port>("api/port/" + port)
    .subscribe(port => {
      this.portForDeleting = port.portName;
      this.checkRoutes(port.portID);
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
  }*/

}
