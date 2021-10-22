import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { Admin } from './admin/admin';
import { AdminLogin } from './adminLogin/adminLogin.component';
import { AdminEdit } from './adminEdit/adminEdit.component';
import { Home } from './home/home.component';
import { OrderComplete } from './orderComplete/orderComplete.component'; 

const appRoots: Routes = [
  { path: 'admin', component: Admin },
  { path: 'adminLogin', component: AdminLogin },
  { path: 'adminEdit/:id', component: AdminEdit },
  { path: 'home', component: Home },
  { path: 'orderComplete', component: OrderComplete },
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
