import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RoleGuard } from 'src/app/services/role-guard.service';

import { RoomManagementComponent } from './room-management/room-management.component';

const ROOM_ROUTES: Routes = [
  {
    path: '',
    component: RoomManagementComponent,
    canActivate: [RoleGuard],
    data: {
      expectedRoles: ['Owner', 'Manager'],
    },
  },
] satisfies Routes;

@NgModule({
  imports: [RouterModule.forChild(ROOM_ROUTES)],
  exports: [RouterModule],
})
export class RoomModule {}
