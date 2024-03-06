import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {IonicModule} from "@ionic/angular";
import {FormBuilder, ReactiveFormsModule} from "@angular/forms";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, IonicModule, ReactiveFormsModule, NgIf],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'frontend';
  imageBytes: string | ArrayBuffer | null = null;

  constructor(private readonly fb: FormBuilder) {
  }

  imageForm = this.fb.group({
    image: [null as File | null],
  })

  imageUrlForm = this.fb.group({
    url: ['']
  });

  onFileChanged($event: Event) {
    const files = ($event.target as HTMLInputElement).files;
    if (!files) return;
    this.imageForm.patchValue({image: files[0]});
    this.imageForm.controls.image.updateValueAndValidity();
    const reader = new FileReader();
    reader.readAsDataURL(files[0]);
    reader.onload = () => {
      this.imageBytes = reader.result;
    }
  }
}
