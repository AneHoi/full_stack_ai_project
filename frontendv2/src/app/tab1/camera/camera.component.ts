import { AfterViewInit, Component, ElementRef, ViewChild } from "@angular/core";

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

  async save() {
    const imageDataUrl = this.captures[this.captures.length - 1]; // Assuming you want to send the latest captured image
    try {
      const blob = await fetch(imageDataUrl).then(res => res.blob());
      const file = new File([blob], 'image.jpg', { type: 'image/jpeg' });

      const formData = new FormData();
      formData.append('image', file);

      const response = await fetch('http://localhost:5096/api/analyze', {
        method: 'POST',
        body: formData
      });

      if (response.ok) {
        console.log('Image saved successfully!');
        // You can add further handling here if needed
      } else {
        console.error('Failed to save image.');
      }
    } catch (error) {
      console.error('Error saving image:', error);
    }
  }
}