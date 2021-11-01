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
import { TicketList } from './ticketList/ticketList.component';
import { PassengerList } from './passengerList/passengerList.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Modal } from './ticketList/deleteModal';
import { Settings } from './settings/settings.component';
import { ticketAdd } from './ticketAdd/ticketAdd.component';


@NgModule({
  declarations: [
    AppComponent,
    admin,
    AdminEdit,
    Meny,
    AdminLoginComponent,
    TicketList,
    PassengerList,
    Modal,
    Settings,
    ticketAdd
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents: [Modal]
})
export class AppModule { }
