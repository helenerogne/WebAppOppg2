import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { admin } from './admin/admin.component';
import { TicketEdit } from './ticketEdit/ticketEdit.component';
import { AdminLoginComponent } from './adminLogin/adminLogin.component';
import { PassengerList } from './passengerList/passengerList.component';
import { TicketList } from './ticketList/ticketList.component';
import { Settings } from './settings/settings.component';
import { ticketAdd } from './ticketAdd/ticketAdd.component';
import { passengerEdit } from './passengerEdit/passengerEdit.component';
import { routeList } from './routeList/routeList.component';


const appRoots: Routes = [
  { path: 'admin', component: admin },
  { path: 'adminLogin', component: AdminLoginComponent },
  { path: 'ticketEdit/:id', component: TicketEdit },
  { path: '', redirectTo: '/adminLogin', pathMatch: 'full' },
  { path: 'ticketList', component: TicketList },
  { path: 'passengerList', component: PassengerList },
  {path: 'settings', component: Settings},
  {path: 'ticketAdd', component: ticketAdd},
  { path: 'passengerEdit/:id', component: passengerEdit },
  { path: 'routeList', component: routeList}
]

@NgModule({
  imports: [
    RouterModule.forRoot(appRoots)
  ],
  exports: [
    RouterModule
  ]
})

export class AppRoutingModule { }
