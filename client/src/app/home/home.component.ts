import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registering = false;

  constructor() { }

  ngOnInit(): void {
  }

  registerToggle() {
    this.registering = !this.registering;
  }

  cancelRegistering(event: boolean) {
    this.registering = event;
  }
}
