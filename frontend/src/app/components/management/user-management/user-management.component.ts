import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable, debounceTime, distinctUntilChanged, startWith, switchMap } from 'rxjs';
import { Router } from '@angular/router';

import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

import { UserManagementHotelListing } from 'src/app/models/hotel.model';
import { UserManagementUserListing } from 'src/app/models/user.model';
import { DeleteDialogComponent } from '../../shared/delete-dialog/delete-dialog.component';

import { HotelService } from 'src/app/services/hotel.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  displayedColumns: string[] = ['firstName', 'lastName', 'role', 'email', 'birthDate', 'registrationDate', 'edit', 'remove'];
  dataSource = new MatTableDataSource<UserManagementUserListing>();

  myControl = new FormControl('');

  selectedHotelName = "All hotels"
  selectedHotelId = ""

  sortField = "None"
  isAscending = true;

  dataLoaded = false;

  totalItemCount = 0;

  pageSize = 5;
  pageIndex = 1;

  options: UserManagementHotelListing[] = [];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private hotelService: HotelService,
    private userService: UserService,
    public dialog: MatDialog,
    public router: Router
  ) {}

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
      this.dataLoaded = true;
    });
  }

  openDialog(userId: string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: "user",
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {
        this.userService.deleteUser(userId).subscribe(response => {
            this.fetchUserData();
          })
      }
    });
  }

  onEditRole(userId: string) {
    this.router.navigate(['user-management/edit-role', userId]);
  }

  onClearInput() {
    this.selectedHotelName = '';
  }

  onSort(sortField: string) {
    if (!this.selectedHotelId && !this.selectedHotelName) {
      return;
    }
    
    if (this.sortField == sortField) {
      this.isAscending = ! this.isAscending
      this.fetchUserData()

      return;
    }

    this.sortField = sortField;
    this.isAscending = true;

    this.fetchUserData()
  }
  
  optionSelected(event: any) {
    this.selectedHotelName = event.option.viewValue;
    this.selectedHotelId = event.option.value;
    this.pageIndex = 1;
    this.sortField = "None";
    this.isAscending = true;

    this.fetchUserData();
  }

  fetchAutocompleteOptions(value: string): Observable<any> {
    return this.hotelService.getHotelAutocomplete(value);
  }

  fetchUserData() {
    this.userService.getSubordinates(this.selectedHotelId, this.pageSize, this.pageIndex, this.isAscending, this.sortField).subscribe((data: any) => {
      this.dataSource.data = data.users;
      this.totalItemCount = data.count;
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


