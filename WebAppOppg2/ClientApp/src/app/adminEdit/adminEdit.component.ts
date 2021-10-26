import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Ticket } from "../Ticket"


@Component({
  templateUrl: "adminEdit.component.html"
})

export class AdminEdit {
  allTickets: Array<Ticket>;
  skjema: FormGroup;


  validering = {

  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router, private route: ActivatedRoute ) {
    this.skjema = fb.group(this.validering);

  }


  ngOnInit() {
    this.route.params.subscribe(params => {
      this.changeTicket(params.id);

    })
  }

  vedSubmit() {
    this.changeOneTicket();
  }

  changeTicket(id: number) {
    this.http.get<Ticket>("api/ticket" + id)
      .subscribe()
      ticket => {
        this.skjema.patchValue({ id: ticket.ID });
        this.skjema.patchValue({ id: ticket.FirstName });
        this.skjema.patchValue({ id: ticket.LastName });
        this.skjema.patchValue({ id: ticket.Email });
        this.skjema.patchValue({ id: ticket.Route });
        this.skjema.patchValue({ id: ticket.Date });
        this.skjema.patchValue({ id: ticket.Quantity });
        this.skjema.patchValue({ id: ticket.Type });
        this.skjema.patchValue({ id: ticket.Price });
      }
    }
  

changeOneTicket(){
  const changedTicket = new Ticket();
  changedTicket.id = this.skjema.value.id;
  changedTicket.FirstName = this.skjema.value.firstname;
  changedTicket.LastName = this.skjema.value.lastname;
  changedTicket.Email = this.skjema.value.email;
  changedTicket.Route = this.skjema.value.route;
  changedTicket.Date = this.skjema.value.date;
  changedTicket.Quantity = this.skjema.value.quantity;
  changedTicket.Type = this.skjema.value.type;
  changedTicket.Price = this.skjema.value.price;

  this.http.put("api/kunde/", changedTicket)
    .subscribe(
      retur => {
        this.router.navigate(['/home.component']); 
        },
        );
  }
}



