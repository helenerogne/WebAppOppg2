import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Ticket } from '../Ticket';

@Component({
  templateUrl: "admin.component.html"
})

export class admin {
  allTickets: Array<Ticket>;
  loading: boolean;

  constructor(private http: HttpClient, private router: Router) { }

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

  deleteOneTicket(id: number) {
    this.http.delete("api/order/" + id)
      .subscribe(retur => {
        this.getAllTickets();
        this.router.navigate(['/admin.component.html']);
      },
        error => console.log(error)
      );
  };
}
