import {Component, HostListener, OnInit, ViewChild} from '@angular/core';
import {Member} from '../../_models/member';
import {User} from '../../_models/user';
import {AccountService} from '../../_services/account.service';
import {MemberService} from '../../_services/member.service';
import {take} from 'rxjs/operators';
import {ToastrService} from 'ngx-toastr';
import {NgForm} from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  member: Member;
  user: User;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private accountService: AccountService, private memberService: MemberService, private toastrService: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(u => this.user = u);
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    this.memberService.getMember(this.user.username).subscribe(m => {
      this.member = m;
    })
  }

  updateMember() {
    this.memberService.updateMember(this.member).subscribe(r => {
      this.toastrService.success('Update success');
      this.editForm.reset(this.member);
    })
  }
}
