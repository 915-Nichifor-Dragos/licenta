import { Component, ViewChild, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

import { HotelService } from 'src/app/components/management/hotels/hotel.service';

import { HotelManagementHotelListing } from 'src/app/components/management/hotels/hotel.model';
import { DeleteDialogComponent } from '../../../../shared/delete-dialog/delete-dialog.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-hotel-management',
  templateUrl: './hotel-management.component.html',
  styleUrls: ['./hotel-management.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatProgressSpinnerModule,
    MatTableModule,
    MatPaginatorModule,
    RouterModule,
  ],
})
export class HotelManagementComponent {
  hotelService = inject(HotelService);
  dialog = inject(MatDialog);
  router = inject(Router);

  displayedColumns: string[] = [
    'hotelName',
    'location',
    'numberOfEmployees',
    'availability',
    'edit',
    'remove',
  ];

  dataSource!: HotelManagementHotelListing[];

  totalItemCount = 0;
  pageSize = 5;
  pageIndex = 1;
  sortField = 'None';
  isAscending = true;

  dataLoaded = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit() {
    this.fetchHotelData();
  }

  openDialog(hotelId: string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: 'hotel',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result == true) {
        this.hotelService.deleteHotel(hotelId).subscribe((response) => {
          this.fetchHotelData();
        });
      }
    });
  }

  onSort(sortField: string) {
    if (this.sortField == sortField) {
      this.isAscending = !this.isAscending;
      this.fetchHotelData();

      return;
    }

    this.sortField = sortField;
    this.isAscending = true;

    this.fetchHotelData();
  }

  onEditHotel(hotelId: string) {
    this.router.navigate(['hotel-management/edit', hotelId]);
  }

  fetchHotelData() {
    this.hotelService
      .getHotels(
        this.pageSize,
        this.pageIndex,
        this.isAscending,
        this.sortField
      )
      .subscribe((data: any) => {
        this.dataSource = data.hotels;
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
