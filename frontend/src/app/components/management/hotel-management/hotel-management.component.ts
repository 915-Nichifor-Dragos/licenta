import { Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Observable, debounceTime, distinctUntilChanged, startWith, switchMap } from 'rxjs';
import { HotelManagementHotelListing, UserManagementHotelListing } from 'src/app/models/hotel.model';
import { UserManagementUserListing } from 'src/app/models/user.model';
import { HotelService } from 'src/app/services/hotel.service';
import { UserService } from 'src/app/services/user.service';
import { DeleteDialogComponent } from '../../shared/delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-hotel-management',
  templateUrl: './hotel-management.component.html',
  styleUrls: ['./hotel-management.component.css']
})
export class HotelManagementComponent {
  displayedColumns: string[] = ['hotelName', 'location', 'numberOfEmployees', 'availability', 'edit', 'remove'];
  dataSource = new MatTableDataSource<HotelManagementHotelListing>();

  sortField = "None"
  isAscending = true;

  totalItemCount = 0;

  pageSize = 5;
  pageIndex = 1;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private hotelService: HotelService,
    private userService: UserService,
    public dialog: MatDialog,
    public router: Router
  ) {}

  ngOnInit() {
    this.fetchHotelData();
  }

  openDialog(userId: string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: "hotel",
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {
        this.userService.deleteUser(userId).subscribe(response => {
            this.fetchHotelData();
          })
      }
    });
  }

  onSort(sortField: string) {  
    if (this.sortField == sortField) {
      this.isAscending = ! this.isAscending
      this.fetchHotelData()

      return;
    }

    this.sortField = sortField;
    this.isAscending = true;

    this.fetchHotelData()
  }

  onEditHotel(id: string) {

  }

  fetchHotelData() {
    this.hotelService.getHotels(this.pageSize, this.pageIndex, this.isAscending, this.sortField).subscribe((data: any) => {
      this.dataSource.data = data.hotels;
      this.totalItemCount = data.count;
    });
  }

  onPageChange(event: any) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex + 1;
  
    this.fetchHotelData();
  }
}
