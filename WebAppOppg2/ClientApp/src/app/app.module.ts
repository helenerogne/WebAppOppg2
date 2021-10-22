import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';


import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { Admin } from './admin/admin';
import { AdminLogin } from './adminLogin/adminLogin.component';
import { AdminEdit } from './adminEdit/adminEdit.component';
import { Home } from './home/home.component';
import { OrderComplete } from './orderComplete/orderComplete.component';

@NgModule({
  declarations: [
    AppComponent,
    Admin,
    AdminLogin,
    AdminEdit,
    Home,
    OrderComplete
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
