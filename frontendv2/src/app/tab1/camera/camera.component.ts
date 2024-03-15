import { AfterViewInit, Component, ElementRef, ViewChild } from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {Allergen} from "../../tab2/tab2.page";



@Component({
  selector: "app-webcam-snapshot",
  templateUrl: "./camera.component.html",
  styleUrls: ["./camera.component.scss"]
})
export class WebcamSnapshotComponent implements AfterViewInit {

  WIDTH = 640;
  HEIGHT = 480;

  @ViewChild("video")
  public video!: ElementRef;

  @ViewChild("canvas")
  public canvas!: ElementRef;

  captures: string[] = [];
  error: any;
  isCaptured!: boolean;

  constructor(private readonly http: HttpClient) {
  }

  async ngAfterViewInit() {
    await this.setupDevices();
  }

  async setupDevices() {
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
      try {
        const stream = await navigator.mediaDevices.getUserMedia({
          video: true
        });
        if (stream) {
          this.video.nativeElement.srcObject = stream;
          this.video.nativeElement.play();
          this.error = null;
        } else {
          this.error = "You have no output video device";
        }
      } catch (e) {
        this.error = e;
      }
    }
  }

  capture() {
    this.drawImageToCanvas(this.video.nativeElement);
    this.captures.push(this.canvas.nativeElement.toDataURL("image/png"));
    this.isCaptured = true;
  }

  removeCurrent() {
    this.isCaptured = false;
  }

  setPhoto(idx: number) {
    this.isCaptured = true;
    var image = new Image();
    image.src = this.captures[idx];
    this.drawImageToCanvas(image);
  }

  drawImageToCanvas(image: any) {
    this.canvas.nativeElement
      .getContext("2d")
      .drawImage(image, 0, 0, this.WIDTH, this.HEIGHT);
  }

  /*
  async save() {
    const imageDataUrl = this.captures[this.captures.length - 1]; // Assuming you want to send the latest captured image
    try {
      const blob = await fetch(imageDataUrl).then(res => res.blob());
      const file = new File([blob], 'image.jpg', { type: 'image/jpeg' });

      const formData = new FormData();
      formData.append('image', file);

      const call = this.http.post<ResultDto>("http://localhost:5096/api/analyze", formData);
      const response = await firstValueFrom<ResultDto>(call);

      console.log(response)

    } catch (error) {
      console.error('Error saving image:', error);
    }
  }
  */

  isModalOpen: boolean = false;
  result: ResultDto = { text: '', allergenes: [] };

  async save() {
    const imageDataUrl = this.captures[this.captures.length - 1]; // Assuming you want to send the latest captured image
    try {
      const blob = await fetch(imageDataUrl).then(res => res.blob());
      const file = new File([blob], 'image.jpg', { type: 'image/jpeg' });

      const formData = new FormData();
      formData.append('image', file);

      const call = this.http.post<ResultDto>("http://localhost:5096/api/analyze", formData);
      const response = await firstValueFrom<ResultDto>(call);

      console.log(response);

      this.result = response;
      this.isModalOpen = true;

    } catch (error) {
      console.error('Error saving image:', error);
    }
  }
  closeModal() {
    this.isModalOpen = false; // Close the modal
  }

}
export interface ResultDto{
  text: string,
  allergenes: string[];
}
