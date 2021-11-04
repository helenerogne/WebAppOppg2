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
  wrongLogin: any;

  validering = {
    username: [''],
    password: ['']
  }

  constructor(private http: HttpClient, private fb: FormBuilder,
    private route: ActivatedRoute, private router: Router) {
    this.LoginForm = fb.group(this.validering);
  }

  onSubmit() {
    this.wrongLogin = false;
    this.logIn();
  }

  logIn() {
    const logIn = new Admin();
    logIn.username = this.LoginForm.value.username;
    logIn.Password = this.LoginForm.value.password;
    this.http.post("api/admin/", logIn)
      .subscribe(isValidated => {
        if(isValidated){
          this.router.navigate(['/admin']);
        }else{
          this.wrongLogin = true;
          //alert('Feil brukernavn eller passord')
        }
      },
        error => console.log(error)
      );
  }
}
