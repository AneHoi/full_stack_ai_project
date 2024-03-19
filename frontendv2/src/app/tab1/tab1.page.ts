import { Component } from '@angular/core';
import {UserHandler} from "../authentication/userhandler";
import {AuthGuard} from "../../services/AuthGuard";
import {ToastController} from "@ionic/angular";
import {Subscription} from "rxjs";
import {Router} from "@angular/router";

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['/tap1.scss']
})
export class Tab1Page {
  private subscriptionForLoginOut: Subscription;
  loginOut: string = 'login';

  constructor(private userHandler: UserHandler, private tokenService: AuthGuard, public toastcontroller: ToastController) {
    this.subscriptionForLoginOut = this.userHandler.logInOutValue$.subscribe((value) => {
      this.loginOut = value;
    })
    this.userHandler.logInOutValue$;
  }


  async loginoutUser() {
    if (this.userHandler.getCurrentUser().username !== undefined) {
      this.tokenService.clearToken();

      (await this.toastcontroller.create({
        message: 'Successfully logged out',
        duration: 5000,
        color: 'success',
      })).present()

      this.userHandler.updateLoginOut("Login");
      this.userHandler.updateCurrentUser('');
    }
  }
}
