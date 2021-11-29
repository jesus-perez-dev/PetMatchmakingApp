import { TestBed } from '@angular/core/testing';

import { EditChangesGuard } from './edit-changes.guard';

describe('EditChangesGuard', () => {
  let guard: EditChangesGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(EditChangesGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
