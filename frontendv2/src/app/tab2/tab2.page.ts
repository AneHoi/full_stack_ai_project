import {Component, ViewChild} from '@angular/core';
import {IonSelect} from '@ionic/angular';
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-tab2',
  templateUrl: 'tab2.page.html',
  styleUrls: ['tab2.page.scss']
})
export class Tab2Page {
  allergens: Allergen[] = [];
  usersAllergies: number[] = [];

  constructor(private readonly http: HttpClient) {
    this.getAllergenCategories();
    this.getUsersAllergens();

    //Default checks off all allergens
    this.allergens.forEach((a) => this.usersAllergies.push(a.id))

    //Make a method to get the users' specific allergies and overwrite usersAllergies list
  }

  handleUserSelection(event: CustomEvent) {
    if (event.detail.value !== undefined) {
      this.usersAllergies = event.detail.value
    }
  }

  async getAllergenCategories() {
    const call = this.http.get<Allergen[]>("http://localhost:5096/api/getAllergens");
    this.allergens = await firstValueFrom(call);
  }

  async getUsersAllergens() {
    const call = this.http.get<number[]>("http://localhost:5096/api/getUsersAllergens");
    this.usersAllergies = await firstValueFrom(call);
  }

  async saveAllergens() {
    const call = this.http.post<Allergen[]>("http://localhost:5096/api/saveAllergens", this.usersAllergies);
    const response = await firstValueFrom<Allergen[]>(call);

    //TODO Handle response (show a toast & redirect?)
  }
}

export interface Allergen {
  id: number,
  category_name: string
}
