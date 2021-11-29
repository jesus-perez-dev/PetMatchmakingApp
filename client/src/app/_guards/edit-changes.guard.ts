import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import {MemberEditComponent} from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class EditChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(
    component: MemberEditComponent): boolean {
    if (component.editForm.dirty) {
      return confirm('There has been changes to your profile. Continue to redirect?');
    }
    return true;
  }

}
