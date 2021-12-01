import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {Message} from '../../_models/message';
import {MessageService} from '../../_services/message.service';
import {Observable} from 'rxjs';
import {NgForm} from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @Input() username: string;
  @ViewChild("messageForm") messageForm: NgForm;
  messages$: Observable<Message[]>;
  content: string;

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadThread();
  }

  loadThread() {
    this.messages$ = this.messageService.getMessageThread(this.username);
  }

  sendMessage() {
    this.messageService.sendMessage(this.username, this.content).subscribe(m => {
      this.loadThread();
      this.messageForm.reset();
    })
  }
}
