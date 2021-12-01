import { Component, OnInit } from '@angular/core';
import {Member} from '../_models/member';
import {MemberService} from '../_services/member.service';
import {Observable} from 'rxjs';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members$: Observable<Partial<Member[]>>;
  connectionType = 'liked';

  constructor(private memberService: MemberService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    this.members$ = this.memberService.getConnectionByType(this.connectionType);
  }
}
