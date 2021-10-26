import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';


import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { admin } from './admin/admin.component';
import { AdminLoginComponent } from './adminLogin/adminLogin.component';
import { AdminEdit } from './adminEdit/adminEdit.component';
import { Meny } from './meny/meny.component';


@NgModule({
  declarations: [
    AppComponent,
    admin,
    AdminEdit,
    Meny,
    AdminLoginComponent
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
