import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { RoleGuard } from './services/role-guard.service';
import { UserManagementComponent } from './components/user-management/user-management.component';
import { EditUserRoleComponent } from './components/user/edit-user-role/edit-user-role.component';

const routes: Routes = [
  { 
    path: '', 
    redirectTo: 'home', 
    pathMatch: 'full' 
  },
  { 
    path: 'login', 
    component: LoginComponent 
  },
  { 
    path: 'register', 
    component: RegisterComponent 
  },
  { 
    path: 'home',
    component: HomeComponent,
    canActivate: [RoleGuard],
    data: {
      expectedRoles: ['Owner', 'Manager', 'Employee', 'Client']
    }
  },
  { 
    path: 'user-management',
    component: UserManagementComponent,
    canActivate: [RoleGuard],
    data: {
      expectedRoles: ['Owner', 'Manager']
    }
  },
  { 
    path: 'user-management/edit-role/:id', 
    component: EditUserRoleComponent,
    canActivate: [RoleGuard],
    data: {
      expectedRoles: ['Owner', 'Manager']
    }
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }