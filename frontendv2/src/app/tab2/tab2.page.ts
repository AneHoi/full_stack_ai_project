import {Component, ViewChild} from '@angular/core';
import {IonModal} from '@ionic/angular';
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-tab2',
  templateUrl: 'tab2.page.html',
  styleUrls: ['tab2.page.scss']
})
export class Tab2Page {
  @ViewChild('modal', {static: true}) modal!: IonModal;
  allergens: Allergen[] = [];
  usersAllergies: number[] = [];
  selectedAllergensText: string = '0 Allergens';

  constructor(private readonly http: HttpClient) {
    this.getAllergenCategories();
    this.getUsersAllergens();
  }


  async getAllergenCategories() {
    const call = this.http.get<Allergen[]>("http://localhost:5096/api/getAllergens");
    this.allergens = await firstValueFrom(call);
  }

  async getUsersAllergens() {
    const call = this.http.get<number[]>("http://localhost:5096/api/getUsersAllergens");
    this.usersAllergies = await firstValueFrom(call);
    this.updateSelectedAllergensText();
  }

  async saveAllergens() {
    const call = this.http.post<Allergen[]>("http://localhost:5096/api/saveAllergens", this.usersAllergies);
    const response = await firstValueFrom<Allergen[]>(call);

    //TODO Handle response (show a toast & redirect?)
  }

  allergenSelectionChanged(allergens: number[], modal: IonModal) {
    this.usersAllergies = allergens;
    this.updateSelectedAllergensText();
    modal.dismiss();
  }

  private updateSelectedAllergensText() {
    const selectedAllergens = this.allergens.filter(allergen => this.usersAllergies.includes(allergen.id));

    if (selectedAllergens.length === 0) {
      this.selectedAllergensText = '0 Allergens';
    } else {
      this.selectedAllergensText = selectedAllergens.map(allergen => allergen.category_name).join(', ');
    }
  }
}

export interface Allergen {
  id: number,
  category_name: string
}
