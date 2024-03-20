import {Component, EventEmitter, Input, Output} from '@angular/core';
import {ResultDto} from "../camera/camera.component";

@Component({
  selector: 'app-custom-modal',
  templateUrl: './custom-modal.component.html',
  styleUrls: ['./custom-modal.component.scss']
})
export class CustomModalComponent {
  @Input() result: ResultDto | undefined;
  @Input() isOpen: boolean | undefined;
  @Output() closeModal = new EventEmitter<void>();

  close(): void {
    this.isOpen = false;
    this.closeModal.emit();
  }
}


