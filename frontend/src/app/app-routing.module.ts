import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { RoleGuard } from './services/role-guard.service';

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
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }