import { Injectable } from '@angular/core';
import {NgxSpinnerService} from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  count = 0;

  constructor(private spinnerService: NgxSpinnerService) { }

  loading() {
    this.count++;
    this.spinnerService.show(undefined, {
      type: 'line-scale-party',
      bdColor: 'rgba(255,255,255,0)',
      color: '#333333',
    })
  }

  idle() {
    this.count--;
    if (this.count <= 0) {
      this.count = 0;
      this.spinnerService.hide();
    }
  }
}
