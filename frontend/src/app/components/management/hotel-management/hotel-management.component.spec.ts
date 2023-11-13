import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HotelManagementComponent } from './hotel-management.component';

describe('HotelManagementComponent', () => {
  let component: HotelManagementComponent;
  let fixture: ComponentFixture<HotelManagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HotelManagementComponent]
    });
    fixture = TestBed.createComponent(HotelManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
