import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RoleGuard } from 'src/app/services/role-guard.service';

import { HotelManagementComponent } from './hotel-management/hotel-management.component';
import { EditHotelComponent } from './edit-hotel/edit-hotel.component';

const HOTEL_ROUTES: Routes = [
  {
    path: '',
    component: HotelManagementComponent,
    canActivate: [RoleGuard],
    data: {
      expectedRoles: ['Owner', 'Manager'],
    },
  },
  {
    path: 'edit/:id',
    component: EditHotelComponent,
    canActivate: [RoleGuard],
    data: {
      expectedRoles: ['Owner'],
    },
  },
] satisfies Routes;

@NgModule({
  imports: [RouterModule.forChild(HOTEL_ROUTES)],
  exports: [RouterModule],
})
export class HotelModule {}
