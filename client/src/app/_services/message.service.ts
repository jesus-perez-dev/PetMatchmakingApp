import { Injectable } from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Message} from '../_models/message';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {User} from '../_models/user';
import {BehaviorSubject} from 'rxjs';
import {take} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient) { }

  createHubConnection(user: User, otherUser: string) {
    this.hubConnection = new HubConnectionBuilder().withUrl(`${this.hubUrl}message?user=${otherUser}`, {accessTokenFactory: () => user.token})
      .withAutomaticReconnect().build();

    this.hubConnection.start().catch(error => console.error(error));

    this.hubConnection.on('ReceiveMessageThread', messages => {this.messageThreadSource.next(messages)});

    this.hubConnection.on('NewMessage', message => {
      this.messageThread$.pipe(
        take(1)
      ).subscribe(m => {
        this.messageThreadSource.next([...m, message])
      })
      this.messageThreadSource.next(message)
    });

  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop().catch(error => console.error(error));
    }
  }

  getMessages() {
    return this.http.get<Message[]>(`${this.baseUrl}messages`);
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(`${this.baseUrl}messages/thread/${username}`);
  }

  async sendMessage(username: string, content: string) {
    this.hubConnection.invoke("SendMessage", { receiverUsername: username, content }).catch(e => console.error(e));
    // return this.http.post<Message>(`${this.baseUrl}messages`, { receiverUsername: username, content });
  }
}
