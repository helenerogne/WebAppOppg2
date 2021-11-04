import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { admin } from './admin/admin.component';
import { AdminLoginComponent } from './adminLogin/adminLogin.component';
import { TicketEdit } from './ticketEdit/ticketEdit.component';
import { Meny } from './meny/meny.component';
import { TicketList } from './ticketList/ticketList.component';
import { PassengerList } from './passengerList/passengerList.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TicketModal } from './ticketList/deleteTicketModal';
import { PassengerModal } from './passengerList/deletePassengerModal';
import { PortModal } from './settings/deletePortModal';
import { Settings } from './settings/settings.component';
import { ticketAdd } from './ticketAdd/ticketAdd.component';
import { passengerEdit } from './passengerEdit/passengerEdit.component';



@NgModule({
  declarations: [
    AppComponent,
    admin,
    TicketEdit,
    Meny,
    AdminLoginComponent,
    TicketList,
    PassengerList,
    TicketModal,
    PassengerModal,
    Settings,
    PortModal,
    ticketAdd,
    passengerEdit
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
  entryComponents: [TicketModal, PassengerModal, PortModal]
})
export class AppModule { }
