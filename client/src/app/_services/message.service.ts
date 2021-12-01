import { Injectable } from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Message} from '../_models/message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMessages() {
    return this.http.get<Message[]>(`${this.baseUrl}messages`);
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(`${this.baseUrl}messages/thread/${username}`);
  }

  sendMessage(username: string, content: string) {
    return this.http.post<Message>(`${this.baseUrl}messages`, { receiverUsername: username, content });
  }
}
