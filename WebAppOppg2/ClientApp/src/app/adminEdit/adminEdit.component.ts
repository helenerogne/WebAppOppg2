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
        this.skjema.patchValue({ id: ticket.TicketID });
        this.skjema.patchValue({ firstname: ticket.Firstname });
        this.skjema.patchValue({ lastname: ticket.Lastname });
        this.skjema.patchValue({ email: ticket.Email });
        this.skjema.patchValue({ passengerType: ticket.PassengerType });
        this.skjema.patchValue({ travelType: ticket.TravelType });
        this.skjema.patchValue({ routeFrom: ticket.RouteFrom });
        this.skjema.patchValue({ routeTo: ticket.RouteTo });
        this.skjema.patchValue({ departure: ticket.Departure });
        this.skjema.patchValue({ ticketDate: ticket.TicketDate });
        this.skjema.patchValue({ price: ticket.Price });
      }
    }
  

changeOneTicket(){
  const changedTicket = new Ticket();
  changedTicket.TicketID = this.skjema.value.id;
  changedTicket.Firstname = this.skjema.value.firstname;
  changedTicket.Lastname = this.skjema.value.lastname;
  changedTicket.PassengerType = this.skjema.value.passengerType;
  changedTicket.Email = this.skjema.value.email;
  changedTicket.TravelType = this.skjema.value.travelType;
  changedTicket.RouteTo = this.skjema.value.routeTo;
  changedTicket.RouteFrom = this.skjema.value.routeFrom;
  changedTicket.Departure = this.skjema.value.departure;
  changedTicket.TicketDate = this.skjema.value.ticketDate;
  changedTicket.Price = this.skjema.value.price;

  this.http.put("api/kunde/", changedTicket)
    .subscribe(
      retur => {
        this.router.navigate(['/home.component']); 
        },
        );
  }
}



