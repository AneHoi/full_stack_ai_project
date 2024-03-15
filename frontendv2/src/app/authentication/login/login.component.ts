import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthGuard} from "../../../services/AuthGuard";
import {ResponseDto, User} from "../../../models";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.prod";
import {firstValueFrom, Subscription} from "rxjs";
import {ToastController} from "@ionic/angular";
import {UserHandler} from "../userhandler";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent  implements OnInit {
  currentUser: User | undefined;
  //This is the formbuilder, it is important to SPELL the items as they are spelled in the dto in the API
  emailForm = new FormControl('', [Validators.required, Validators.minLength(2)])
  passwordForm = new FormControl('',[Validators.required, Validators.minLength(8)])

  loginForm = new FormGroup(
    {
      email: this.emailForm,
      password: this.passwordForm
    }
  )

  private subscription: Subscription;
  dynamicLogInOutText: string = 'Login';

  constructor(private tokenService: AuthGuard, private userHandler: UserHandler, public http: HttpClient, public toastcontroller: ToastController) {
    this.subscription = this.userHandler.logInOutValue$.subscribe((value) => {
      this.dynamicLogInOutText = value;
    })
  }

  ngOnInit() {}

  //check if we should login ot logout
  loginOut() {
    if (this.dynamicLogInOutText == 'Login') {
      this.login()
    } else {
      this.logout()
    }
  }


  async login() {
    try {
      const observable = this.http.post<ResponseDto<{
        token: string
      }>>(environment.baseURL + '/account/login', this.loginForm.getRawValue());
      const response = await firstValueFrom(observable);
      this.tokenService.setToken(response.responseData!.token)
      console.log("your token " + response.responseData!.token)

      const toast = await this.toastcontroller.create({
        message: 'Login was sucessfull',
        duration: 5000,
        color: "success"
      })
      toast.present();
    } catch (e) {
    }
    //Setting the current user.
    const observable = this.http.get<ResponseDto<User>>(environment.baseURL + '/account/whoami');
    const response = await firstValueFrom(observable);
    this.currentUser = response.responseData;
    //Securing that the logged in user accually has the information, and not just an empty object
    if (this.currentUser !== undefined) {
      this.userHandler.setCurrentUser(this.currentUser);
      this.changeNameOfCurrentUser(this.userHandler.getCurrentUser().username);
      this.userHandler.updateLoginOut("Logout");
    }
  }

  async logout() {
    this.tokenService.clearToken();

    (await this.toastcontroller.create({
      message: 'Successfully logged out',
      duration: 5000,
      color: 'success',
    })).present()

    this.userHandler.updateLoginOut("Login");
    this.userHandler.updateCurrentUser('');
  }

  changeNameOfCurrentUser(name: any): void {
    this.userHandler.updateCurrentUser(name);
  }

}
