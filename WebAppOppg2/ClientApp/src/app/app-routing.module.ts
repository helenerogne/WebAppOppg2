import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { admin } from './admin/admin';
import { AdminEdit } from './adminEdit/adminEdit.component';
//import { AdminLogin } from './adminLogin/adminLogin.component';


const appRoots: Routes = [
  { path: 'admin', component: admin },
  //{ path: 'adminLogin', component: AdminLogin },
  { path: 'adminEdit/:id', component: AdminEdit },
  { path: '', redirectTo: '/admin', pathMatch: 'full' }  
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
