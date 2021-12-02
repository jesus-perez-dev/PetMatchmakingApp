import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registering = false;
  info = false;

  constructor() { }

  ngOnInit(): void {
  }

  registerToggle() {
    this.registering = true;
  }

  cancelRegistering(event: boolean) {
    this.registering = event;
  }

  infoToggle() {
    this.info = true;
  }

  cancelInfo(event: boolean) {
    this.info = event;
  }
}
