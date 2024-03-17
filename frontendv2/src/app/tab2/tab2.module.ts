import { IonicModule } from '@ionic/angular';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Tab2Page } from './tab2.page';
import { ExploreContainerComponentModule } from '../explore-container/explore-container.module';

import { Tab2PageRoutingModule } from './tab2-routing.module';
import {AuthHttpInterceptor} from "../../interceptors/auth-http-interceptor";
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {TypeaheadComponent} from "./typeahead/typeahead.component";

@NgModule({
  imports: [
    IonicModule,
    CommonModule,
    FormsModule,
    ExploreContainerComponentModule,
    Tab2PageRoutingModule
  ],
  declarations: [Tab2Page, TypeaheadComponent]
})
export class Tab2PageModule {}
