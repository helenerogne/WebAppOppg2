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
  }
}
