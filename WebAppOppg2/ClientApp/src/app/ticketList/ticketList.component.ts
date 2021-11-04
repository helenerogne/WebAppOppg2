import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Ticket } from '../Ticket';
import { TicketModal } from './deleteTicketModal';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: "ticketList.component.html"
})

export class TicketList {
  allTickets: Array<Ticket>;
  loading: boolean;
  ticketForDeleting: string;
  deleteOK: boolean;

  constructor(private http: HttpClient, private router: Router, private modalService: NgbModal) { }

  ngOnInit() {
    this.loading = true;
    this.getAllTickets();
  }

  getAllTickets() { 
    this.http.get<Ticket[]>("api/order")
      .subscribe(tickets => {
        this.allTickets = tickets;
        this.loading = false;
      },
        error => console.log(error)
      );
  };

  /*
  deleteOneTicket(id: number) {
    this.http.delete("api/order/" + id)
      .subscribe(retur => {
        this.getAllTickets();
        this.router.navigate(['/admin.component.html']);
      },
        error => console.log(error)
      );
  };
  */

  deleteOneTicket(id: number) {
    // først hent navnet på kunden
    
    this.http.get<Ticket>("api/order/" + id)
      .subscribe(ticket => {
        this.ticketForDeleting = ticket.firstname + " " + ticket.lastname;

        // så vis modalen og evt. kall til slett

        this.showModalandDelete(id);
      },
        error => console.log(error)
      );
  }

  showModalandDelete(id: number) {
    const modalRef = this.modalService.open(TicketModal);

    modalRef.componentInstance.name = this.ticketForDeleting;

    modalRef.result.then(retur => {
      console.log('Lukket med:' + retur);
      if (retur == "Slett") {

        // kall til server for sletting
        this.http.delete("api/order/" + id)
          .subscribe(retur => {
            this.getAllTickets();
          },
            error => console.log(error)
          );
      }
      this.router.navigate(['/ticketList']);
    });
  }

}
