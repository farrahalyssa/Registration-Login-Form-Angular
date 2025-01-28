import { HomeComponent } from './home/home.component';
import { RegistrationComponent } from './registration/registration.component';
import { LoginComponent } from './login/login.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

export const routes: Routes = [
    {path: '', component: RegistrationComponent},
    {path: 'home', component:HomeComponent},
    {path: 'login', component:LoginComponent},
    { path: '**', redirectTo: 'home' }  
];
