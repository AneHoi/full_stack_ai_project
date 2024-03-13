import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LoginComponent} from "./login/login.component";
import {IonicModule} from "@ionic/angular";
import {RouterLink, RouterModule, Routes} from "@angular/router";
import {BrowserModule} from "@angular/platform-browser";
import {HttpClientModule} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {RegisterComponent} from "./register/register.component";


@NgModule({
  declarations: [LoginComponent, RegisterComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  imports: [
    RouterLink,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    IonicModule,
  ]
})
export class AuthenticationModule {
}
