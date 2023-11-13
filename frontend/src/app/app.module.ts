import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { LoginComponent } from './components/auth/login/login.component';
import { NavbarComponent } from './components/shared/navbar/navbar.component';
import { FooterComponent } from './components/shared/footer/footer.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatNativeDateModule} from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './services/auth.service';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './components/home/home.component';
import { RoleGuard } from './services/role-guard.service';
import { UserManagementComponent } from './components/management/user-management/user-management.component';
import { UserService } from './services/user.service';
import { HotelService } from './services/hotel.service';
import { MatDialogModule } from '@angular/material/dialog';
import { DeleteDialogComponent } from './components/shared/delete-dialog/delete-dialog.component';
import { EditUserRoleComponent } from './components/user/edit-user-role/edit-user-role.component';
import { AddUserComponent } from './components/user/add-user/add-user.component';
import { HotelManagementComponent } from './components/management/hotel-management/hotel-management.component';
import { BookingManagementComponent } from './components/management/booking-management/booking-management.component';
import { RoomManagementComponent } from './components/management/room-management/room-management.component';

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
    RoomManagementComponent
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
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-center',
      preventDuplicates: true,
      progressBar: true,
      closeButton: true,
    })
  ],
  providers: [
    AuthService,
    RoleGuard,
    UserService,
    HotelService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
