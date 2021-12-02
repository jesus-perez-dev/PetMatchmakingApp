import { Injectable } from '@angular/core';
import {environment} from '../../environments/environment';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {ToastrService} from 'ngx-toastr';
import {User} from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;

  constructor(private toastrService: ToastrService) { }

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder().withUrl(`${this.hubUrl}presence`, {accessTokenFactory: () => user.token})
      .withAutomaticReconnect().build();

    this.hubConnection.start().catch(error => console.error(error));

    this.hubConnection.on('UserOnline', u => {this.toastrService.info(`${u} connected`)});

    this.hubConnection.on('UserOffline', u => {this.toastrService.warning(`${u} disconnected`)});
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.error(error));
  }
}
