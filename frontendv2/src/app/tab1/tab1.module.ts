import { IonicModule } from '@ionic/angular';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Tab1Page } from './tab1.page';
import { ExploreContainerComponentModule } from '../explore-container/explore-container.module';

import { Tab1PageRoutingModule } from './tab1-routing.module';
import {WebcamSnapshotComponent} from "./camera/camera.component";
import {CustomModalComponent} from "./custom-modal/custom-modal.component";

@NgModule({
  imports: [
    IonicModule,
    CommonModule,
    FormsModule,
    ExploreContainerComponentModule,
    Tab1PageRoutingModule
  ],
    declarations: [Tab1Page, WebcamSnapshotComponent, CustomModalComponent]
})
export class Tab1PageModule {}
