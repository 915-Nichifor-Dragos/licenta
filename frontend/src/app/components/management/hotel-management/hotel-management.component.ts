import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

import { HotelService } from 'src/app/services/hotel.service';

import { HotelManagementHotelListing } from 'src/app/models/hotel.model';
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

  dataLoaded = false;

  totalItemCount = 0;

  pageSize = 5;
  pageIndex = 1;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private hotelService: HotelService,
    public dialog: MatDialog,
    public router: Router
  ) {}

  ngOnInit() {
    this.fetchHotelData();            
  }

  openDialog(hotelId: string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: "hotel",
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {
        this.hotelService.deleteHotel(hotelId).subscribe(response => {
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

  onEditHotel(hotelId: string) {
    this.router.navigate(['hotel-management/edit-hotel', hotelId]);
  }

  fetchHotelData() {
    this.hotelService.getHotels(this.pageSize, this.pageIndex, this.isAscending, this.sortField).subscribe((data: any) => {
      this.dataSource.data = data.hotels;
      this.totalItemCount = data.count;
      this.dataLoaded = true;
    });
  }

  onPageChange(event: any) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex + 1;
  
    this.fetchHotelData();
  }
}
