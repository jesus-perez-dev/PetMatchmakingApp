import { Injectable } from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Member} from '../_models/member';
import {of} from 'rxjs';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) { }

  getMembers() {
    if (this.members.length > 0) {
      return of(this.members);
    }
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(m => {
        this.members = m;
        return m;
      })
    );
  }

  getMember(username: string) {
    const member = this.members.find(m => m.userName === username);
    if (member !== undefined) {
      return of(member);
    }
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }

  setProfilePicture(photoId: number) {
    return this.http.put(`${this.baseUrl}users/set-profile-picture/${photoId}`, {})
  }

  deletePhoto(photoId: number) {
    return this.http.delete(`${this.baseUrl}users/delete-photo/${photoId}`)
  }

  likeUser(username: string) {
    return this.http.post(`${this.baseUrl}connections/${username}`, {})
  }

  // todo
  reportUser(username: string) {
    // return this.http.post(`${this.baseUrl}users/report/${username}`, {})
  }

  getConnectionByType(connectionType: string) {
    return this.http.get<Partial<Member[]>>(`${this.baseUrl}connections?connectionType=${connectionType}`)
  }
}
