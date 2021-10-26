import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { admin } from './admin/admin.component';
import { AdminEdit } from './adminEdit/adminEdit.component';
import { AdminLoginComponent } from './adminLogin/adminLogin.component';


const appRoots: Routes = [
  { path: 'admin', component: admin },
  { path: 'adminLogin', component: AdminLoginComponent },
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
