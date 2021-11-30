import {Component, Input, OnInit} from '@angular/core';
import {Member} from '../../_models/member';
import {FileUploader} from 'ng2-file-upload';
import {environment} from '../../../environments/environment';
import {AccountService} from '../../_services/account.service';
import {User} from '../../_models/user';
import {take} from 'rxjs/operators';
import {MemberService} from '../../_services/member.service';
import {Photo} from '../../_models/photo';

@Component({
  selector: 'app-photo-edit',
  templateUrl: './photo-edit.component.html',
  styleUrls: ['./photo-edit.component.css']
})
export class PhotoEditComponent implements OnInit {
  @Input() member: Member;
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl
  user: User;

  constructor(private accountService: AccountService, private memberService: MemberService) {
    this.accountService.currentUser$.pipe(
      take(1)
    ).subscribe(u => {
      this.user = u;
    })
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.member.photos.push(photo);
      }
    }
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  setProfilePicture(photo: Photo) {
    this.memberService.setProfilePicture(photo.id).subscribe(() => {
      this.user.photoUrl = photo.url;
      this.accountService.setCurrentUser(this.user);
      this.member.profilePhoto = photo.url;
      this.member.photos.forEach(p => {
        if (p.isProfilePic) {
          p.isProfilePic = false;
        }
        if (p.id === photo.id)  {
          p.isProfilePic = true;
        }
      })
    })
  }

  deletePhoto(photo: Photo) {
    this.memberService.deletePhoto(photo.id).subscribe(() => {
      this.member.photos = this.member.photos.filter(p => p.id !== photo.id);
      })
  }
}
