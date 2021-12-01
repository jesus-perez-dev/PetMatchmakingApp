import { Component, OnInit } from '@angular/core';
import {Message} from '../_models/message';
import {MessageService} from '../_services/message.service';
import {Observable} from 'rxjs';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages$: Observable<Message[]>;

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messages$ = this.messageService.getMessages();
  }
}
