import {Component, OnInit, ViewChild} from '@angular/core';
import {Member} from '../../_models/member';
import {MemberService} from '../../_services/member.service';
import {ActivatedRoute} from '@angular/router';
import {NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions} from '@kolkov/ngx-gallery';
import {TabsetComponent} from 'ngx-bootstrap/tabs';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs') memberTabs: TabsetComponent;
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private memberService: MemberService, private route: ActivatedRoute, private toastrService: ToastrService) { }

  ngOnInit(): void {
    this.loadMember();

    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
    }]
  }

  getPhotos(): NgxGalleryImage[] {
    const imageUrls = [];

    for (const photo of this.member.photos) {
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url,
      })
    }

    return imageUrls;
  }

  loadMember() {
    this.memberService.getMember(this.route.snapshot.paramMap.get('userName')).subscribe(m => {
      this.member = m;
      this.galleryImages = this.getPhotos();
    })
  }

  addLike(member: Member) {
    this.memberService.likeUser(member.userName).subscribe(() => {
      this.toastrService.success("Liked " + member.alias);
    })
  }

  selectMessage() {
    this.memberTabs.tabs[3].active = true;
  }
}
