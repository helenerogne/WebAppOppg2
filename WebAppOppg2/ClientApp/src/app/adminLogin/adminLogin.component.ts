import { Component } from "@angular/core";
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

@Component({
  templateUrl: 'adminLogin.component.html'
})

export class AdminLoginComponent {
  LoginForm: FormGroup;

  // fb kan endres til et annet logisk navn
  constructor(private fb: FormBuilder) {
    this.LoginForm = fb.group({
      username: ["", Validators.required],
      password: ["", Validators.pattern("[0-9]{6,15}")]
    });
  }

  onSubmit() {
    console.log("LoginForm innsendt:");
    console.log(this.LoginForm);
    console.log(this.LoginForm.value.brukernavn);
    console.log(this.LoginForm.touched);
  }
}
