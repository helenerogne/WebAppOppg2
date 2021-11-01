import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { admin } from './admin/admin.component';
import { AdminEdit } from './adminEdit/adminEdit.component';
import { AdminLoginComponent } from './adminLogin/adminLogin.component';
import { PassengerList } from './passengerList/passengerList.component';
import { TicketList } from './ticketList/ticketList.component';
import { Settings } from './settings/settings.component';
import { ticketAdd } from './ticketAdd/ticketAdd.component';


const appRoots: Routes = [
  { path: 'admin', component: admin },
  { path: 'adminLogin', component: AdminLoginComponent },
  { path: 'adminEdit/:id', component: AdminEdit },
  { path: '', redirectTo: '/adminLogin', pathMatch: 'full' },
  { path: 'ticketList', component: TicketList },
  { path: 'passengerList', component: PassengerList },
  {path: 'settings', component: Settings},
  {path: 'ticketAdd', component: ticketAdd}
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
