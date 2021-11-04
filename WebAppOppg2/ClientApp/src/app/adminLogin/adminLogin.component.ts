import { HttpClient } from "@angular/common/http";
import { Component } from "@angular/core";
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute } from "@angular/router";
import { Router } from '@angular/router';
import { Admin } from "../Admin";


@Component({
  templateUrl: 'adminLogin.component.html'
})

export class AdminLoginComponent {
  LoginForm: FormGroup;

  validering = {
    id: [""],
    username: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZæøåÆØÅ. \-]{2,20}")])
    ],
    password: [
      null, Validators.compose([Validators.required, Validators.pattern("^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{3,}$")])
    ]
  }

  constructor(private http: HttpClient, private fb: FormBuilder,
    private route: ActivatedRoute, private router: Router) {
    this.LoginForm = fb.group(this.validering);
  }

  onSubmit() {
    this.logIn();
  }

  logIn() {
    const logIn = new Admin();

    logIn.username = this.LoginForm.value.username;
    logIn.Password = this.LoginForm.value.password;
    this.http.post("api/admin", logIn)
      .subscribe(retur => {
        this.router.navigate(['/adminLogin.component.html']);
      },
        error => console.log(error)
      );
  }
}
