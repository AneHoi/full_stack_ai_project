import { Injectable } from '@angular/core';
import {Router} from '@angular/router';
import {ToastController} from "@ionic/angular";

@Injectable({providedIn: "root"})
export class AuthGuard {

  constructor(private router: Router, private toast: ToastController) { }

  //The token will be stored in the window storrage, so it stays on the user instead of the server
  setToken(token : string){
    sessionStorage.setItem("token", token);
  }

  clearToken() {
    sessionStorage.removeItem("token");
  }
  async canActivate() {
    if (sessionStorage.getItem('token')) {
      // logged in so return true
      return true;
    }

    await (await this.toast.create({
      message: "Please login to access the requested URL",
      color: "warning",
      duration: 5000
    })).present();

    // not logged in so redirect to login page with the return url
    this.router.navigate(['/login'])
    return false
  }

  isLoggedIn() {
    if (sessionStorage.getItem('token')) {
      // logged in so return true
      return true
    }
    return false
  }
}
