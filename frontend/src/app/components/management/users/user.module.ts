import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RoleGuard } from 'src/app/services/role-guard.service';

import { UserManagementComponent } from './user-management/user-management.component';
import { EditUserRoleComponent } from './edit-user-role/edit-user-role.component';
import { AddUserComponent } from './add-user/add-user.component';

const USER_ROUTES: Routes = [
  {
    path: '',
    component: UserManagementComponent,
    canActivate: [RoleGuard],
    data: {
      expectedRoles: ['Owner', 'Manager'],
    },
  },
  {
    path: 'edit/:id',
    component: EditUserRoleComponent,
    canActivate: [RoleGuard],
    data: {
      expectedRoles: ['Owner', 'Manager'],
    },
  },
  {
    path: 'add',
    component: AddUserComponent,
    canActivate: [RoleGuard],
    data: {
      expectedRoles: ['Owner', 'Manager'],
    },
  },
] satisfies Routes;

@NgModule({
  imports: [RouterModule.forChild(USER_ROUTES)],
  exports: [RouterModule],
})
export class UserModule {}
