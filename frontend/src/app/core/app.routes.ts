import { inject } from '@angular/core';
import { Routes } from '@angular/router';

import { RoleGuard } from '../services/role-guard.service';
import { HomeComponent } from '../features/home/home.component';
import { AuthService } from '../features/auth/auth.service';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [RoleGuard],
    data: {
      expectedRoles: ['Owner', 'Manager', 'Employee', 'Client'],
    },
    resolve: {
      userClaims: () => inject(AuthService).getUserClaims(),
    },
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('../features/auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: 'user-management',
    loadChildren: () =>
      import('../features/management/users/user.module').then(
        (m) => m.UserModule
      ),
  },
  {
    path: 'hotel-management',
    loadChildren: () =>
      import('../features/management/hotels/hotel.module').then(
        (m) => m.HotelModule
      ),
  },
  {
    path: 'room-management',
    loadChildren: () =>
      import('../features/management/rooms/room.module').then(
        (m) => m.RoomModule
      ),
  },
  {
    path: 'booking-management',
    loadChildren: () =>
      import('../features/management/bookings/booking.module').then(
        (m) => m.BookingModule
      ),
  },
];
