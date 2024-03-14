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
  allergens: Allergen[] = //Probably lidt for hacky?
    [
      {id: 1, name: "Celery"},
      {id: 2, name: "Crustaceans"},
      {id: 3, name: "Eggs"},
      {id: 4, name: "Fish"},
      {id: 5, name: "Gluten"},
      {id: 6, name: "Lupin"},
      {id: 7, name: "Milk"},
      {id: 8, name: "Molluscs"},
      {id: 9, name: "Mustard"},
      {id: 10, name: "Nuts"},
      {id: 11, name: "Peanuts"},
      {id: 12, name: "Sesame seeds"},
      {id: 13, name: "Soya"},
      {id: 14, name: "Sulphites"}
    ];
  usersAllergies: number[] = [];
  @ViewChild('select') select: IonSelect | undefined; //Used to open the select as default

  constructor(private readonly http: HttpClient) {
    //Default checks off all allergens
    this.allergens.forEach((a) => this.usersAllergies.push(a.id))

    //Make a method to get the users' specific allergies and overwrite usersAllergies list
  }

  ngAfterViewInit() {
    // Open the ion-select element after a short delay
    setTimeout(() => {
      // @ts-ignore
      this.select.open();
    }, 100);
  }

  handleUserSelection(event: CustomEvent) {
    if (event.detail.value !== undefined) {
      this.usersAllergies = event.detail.value
    }
  }

  async saveAllergens() {
    const call = this.http.post<Allergen[]>("http://localhost:5096/api/saveAllergens", this.usersAllergies);
    const response = await firstValueFrom<Allergen[]>(call);

    //TODO Handle response (show a toast & redirect?)
  }
}

export interface Allergen {
  id: number,
  name: string
}
