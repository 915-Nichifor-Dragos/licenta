import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import {
  Observable,
  debounceTime,
  distinctUntilChanged,
  startWith,
  switchMap,
} from 'rxjs';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { DeleteDialogComponent } from 'src/app/shared/delete-dialog/delete-dialog.component';

import { UserManagementUserListing } from '../user.model';
import { UserManagementHotelListing } from '../../hotels/hotel.model';

import { HotelService } from '../../hotels/hotel.service';
import { UserService } from '../user.service';

import { AppPageHeaderComponent } from 'src/app/shared/page-header/page-header.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    RouterModule,
    MatIconModule,
    AppPageHeaderComponent,
  ],
})
export class UserManagementComponent implements OnInit {
  displayedColumns: string[] = [
    'firstName',
    'lastName',
    'role',
    'email',
    'birthDate',
    'registrationDate',
    'edit',
    'remove',
  ];
  dataSource = new MatTableDataSource<UserManagementUserListing>();

  myControl = new FormControl('');

  selectedHotelName = 'All hotels';
  selectedHotelId = '';

  sortField = 'None';
  isAscending = true;

  dataLoaded = false;

  totalItemCount = 0;

  pageSize = 5;
  pageIndex = 1;

  options: UserManagementHotelListing[] = [];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  hotelService = inject(HotelService);
  userService = inject(UserService);
  dialog = inject(MatDialog);
  router = inject(Router);

  ngOnInit() {
    this.options = [];

    this.myControl.valueChanges
      .pipe(
        startWith(''),
        debounceTime(300),
        distinctUntilChanged(),
        switchMap((value: any) => this.fetchAutocompleteOptions(value))
      )
      .subscribe((options: UserManagementHotelListing[]) => {
        this.options = options;
        this.fetchUserData();
      });
  }

  openDialog(userId: string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {
        title: 'Delete Confirmation',
        content: 'Are you sure you want to delete this user?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result == true) {
        this.userService.deleteUser(userId).subscribe(() => {
          this.fetchUserData();
        });
      }
    });
  }

  onEditRole(userId: string) {
    this.router.navigate(['user-management/edit', userId]);
  }

  onClearInput() {
    this.selectedHotelName = '';
  }

  onSort(sortField: string) {
    if (!this.selectedHotelId && !this.selectedHotelName) {
      return;
    }

    if (this.sortField == sortField) {
      this.isAscending = !this.isAscending;
      this.fetchUserData();

      return;
    }

    this.sortField = sortField;
    this.isAscending = true;

    this.fetchUserData();
  }

  optionSelected(event: any) {
    this.selectedHotelName = event.option.viewValue;
    this.selectedHotelId = event.option.value;
    this.pageIndex = 1;
    this.sortField = 'None';
    this.isAscending = true;

    this.fetchUserData();
  }

  fetchAutocompleteOptions(value: string): Observable<any> {
    return this.hotelService.getHotelAutocomplete(value);
  }

  fetchUserData() {
    this.dataLoaded = false;
    this.userService
      .getSubordinates(
        this.selectedHotelId,
        this.pageSize,
        this.pageIndex,
        this.isAscending,
        this.sortField
      )
      .subscribe((data: any) => {
        this.dataSource.data = data.users;
        this.totalItemCount = data.count;
        this.dataLoaded = true;
      });
  }

  onPageChange(event: any) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex + 1;

    if (this.selectedHotelName) {
      this.fetchUserData();
    }
  }
}
