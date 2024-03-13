import {Component, OnInit} from "@angular/core";
import {ResponseDto, User} from "../../../models";
import {AbstractControl, FormBuilder, Validators} from "@angular/forms";
import {firstValueFrom} from "rxjs";
import {environment} from "../../../environments/environment";
import {ToastController} from "@ionic/angular";
import {AuthGuard} from "../../../services/AuthGuard";
import {HttpClient} from "@angular/common/http";
import {UserHandler} from "../userhandler";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  currentUser: User | undefined;
  //This is the formbuilder, it is important to SPELL the items as they are spelled in the dto in the API
  createNewUserForm = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(2)]],
    tlfnumber: ['', [Validators.required, Validators.min(10000000)]],
    email: ['', [Validators.required, Validators.minLength(2)]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    repeatPassword: ['', [Validators.required, Validators.minLength(8)]]
  })

  constructor(public fb: FormBuilder, private userHandler: UserHandler, private tokenService: AuthGuard, public http: HttpClient, public toastcontroller: ToastController) {
  }

  ngOnInit() {
  }

  /**
   * While registering, the "Log in" part is also implemented here, by getting the response in form of the object "User"
   * The current user is stored inside the state-file
   */
  async submit() {
    if (this.isPasswordSame(this.createNewUserForm)) {
      try {
        const observable = this.http.post<ResponseDto<User>>(environment.baseURL + '/account/register', this.createNewUserForm.getRawValue())

        const response = await firstValueFrom(observable);
        //Setting the current user.
        this.currentUser = response.responseData;
        //Securing that the logged in user accually has the information, and not just an empty object
        if (this.currentUser !== undefined) {
          this.userHandler.setCurrentUser(this.currentUser);
        }
        const toast = await this.toastcontroller.create({
          message: 'The registration was sucessfull',
          duration: 1233,
          color: "success"
        })
        toast.present();
      } catch (e) {
      }
    } else {
      const toast = await this.toastcontroller.create({
        message: 'The passwords does not match',
        duration: 1233,
        color: "danger"
      })
      toast.present();
    }
    this.logUserIn()
  }


  isPasswordSame(control: AbstractControl): boolean {
    const password = control.get('password')?.value;
    const repeatPassword = control.get('repeatPassword')?.value;

    if (password === repeatPassword) {
      return true;
    } else {
      return false;
    }
  }


  async logUserIn() {

    try {
      const observable = this.http.post<ResponseDto<{
        token: string
      }>>(environment.baseURL + '/account/login', {email : this.createNewUserForm.controls.email.getRawValue(), password: this.createNewUserForm.controls.password.getRawValue()})
      const response = await firstValueFrom(observable);
      this.tokenService.setToken(response.responseData!.token)

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
  changeNameOfCurrentUser(name: any): void {
    this.userHandler.updateCurrentUser(name);
  }

}
