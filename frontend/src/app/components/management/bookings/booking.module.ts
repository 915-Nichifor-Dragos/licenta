import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RoleGuard } from 'src/app/services/role-guard.service';

import { BookingManagementComponent } from './booking-management/booking-management.component';

const BOOKING_ROUTES: Routes = [
  {
    path: '',
    component: BookingManagementComponent,
    canActivate: [RoleGuard],
    data: {
      expectedRoles: ['Owner', 'Manager'],
    },
  },
] satisfies Routes;

@NgModule({
  imports: [RouterModule.forChild(BOOKING_ROUTES)],
  exports: [RouterModule],
})
export class BookingModule {}
