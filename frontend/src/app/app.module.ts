import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { NavbarComponent } from './components/shared/navbar/navbar.component';
import { FooterComponent } from './components/shared/footer/footer.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { LoginComponent } from './components/auth/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { UserManagementComponent } from './components/management/user-management/user-management.component';
import { EditUserRoleComponent } from './components/management/user/edit-user-role/edit-user-role.component';
import { AddUserComponent } from './components/management/user/add-user/add-user.component';
import { HotelManagementComponent } from './components/management/hotel-management/hotel-management.component';
import { BookingManagementComponent } from './components/management/booking-management/booking-management.component';
import { RoomManagementComponent } from './components/management/room-management/room-management.component';
import { DeleteDialogComponent } from './components/shared/delete-dialog/delete-dialog.component';

import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { AuthService } from './services/auth.service';
import { RoleGuard } from './services/role-guard.service';
import { UserService } from './services/user.service';
import { HotelService } from './services/hotel.service';
import { EditHotelComponent } from './components/management/hotel/edit-hotel/edit-hotel.component';

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    FooterComponent,
    NavbarComponent,
    HomeComponent,
    UserManagementComponent,
    DeleteDialogComponent,
    EditUserRoleComponent,
    AddUserComponent,
    HotelManagementComponent,
    BookingManagementComponent,
    RoomManagementComponent,
    EditHotelComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatAutocompleteModule,
    BrowserAnimationsModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    HttpClientModule,
    MatPaginatorModule,
    MatTableModule,
    MatIconModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
  ],
  providers: [
    AuthService,
    RoleGuard,
    UserService,
    HotelService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
