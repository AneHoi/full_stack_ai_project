import {Injectable} from "@angular/core";
import {Subject} from "rxjs";
import {User} from "../../models";

@Injectable({
  providedIn: 'root',
})
export class UserHandler {
  //Subject is a special type of Observable that allows values to be multicasted to many Observers
  //It is observing, for things like changes, by using the .next()
  private textValue = new Subject<string>(); //the subject is with strings
  textValue$ = this.textValue.asObservable(); //It is observing on the textvalue

  private logInOutValue = new Subject<string>();
  logInOutValue$ = this.logInOutValue.asObservable();


  // this "{}" means: Empty object
  currentuser: User = {};
  /**
   * When the value is passed into this method, it is going to observe
   * a change and all the classes that subscribed will react to it
   * @param value
   */
  updateCurrentUser(value: string): void {
    this.textValue.next(value);
  }
  updateLoginOut(value: string): void {
    this.logInOutValue.next(value)
  }


  getCurrentUser(): User {
    return this.currentuser;
  }

  setCurrentUser(user: User): void {
    this.currentuser = user;
  }

}
