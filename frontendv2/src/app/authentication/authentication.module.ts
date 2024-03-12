import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LoginComponent} from "./login/login.component";
import {IonicModule} from "@ionic/angular";
import {RouterModule, Routes} from "@angular/router";
import {BrowserModule} from "@angular/platform-browser";
import {HttpClientModule} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {RegisterComponent} from "./register/register.component";

const routes: Routes = [
  {component: LoginComponent, path: 'login'},
  {component: RegisterComponent, path: 'register'}
];

@NgModule({
  declarations: [LoginComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  imports: [
    RouterModule.forRoot(routes),
    CommonModule,
    RouterModule,
    BrowserModule,
    IonicModule.forRoot({mode: 'ios'}),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    IonicModule
  ]
})
export class AuthenticationModule {
}
