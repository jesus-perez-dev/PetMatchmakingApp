import {Component, Input, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Message} from '../../_models/message';
import {MessageService} from '../../_services/message.service';
import {Observable} from 'rxjs';
import {NgForm} from '@angular/forms';
import {AccountService} from '../../_services/account.service';
import {User} from '../../_models/user';
import {take} from 'rxjs/operators';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit, OnDestroy {
  @Input() username: string;
  @ViewChild("messageForm") messageForm: NgForm;
  messages$: Observable<Message[]>;
  content: string;
  currentUser: User;

  constructor(public messageService: MessageService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(
      take(1)
    ).subscribe(u => this.currentUser = u);
  }

  ngOnInit(): void {
    this.loadThread();
  }

  ngOnDestroy() {
    this.messageService.stopHubConnection();
  }

  loadThread() {
    this.messageService.createHubConnection(this.currentUser, this.username);
    // this.messages$ = this.messageService.getMessageThread(this.username);
  }

  sendMessage() {
    this.messageService.sendMessage(this.username, this.content).then(() => {
      this.loadThread();
      this.messageForm.reset();
    })
  }
}
