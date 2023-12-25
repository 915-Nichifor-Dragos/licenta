import { Component, ViewChild, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { HotelService } from '../hotel.service';
import { HotelManagementHotelListing } from '../hotel.model';
import { DeleteDialogComponent } from 'src/app/shared/delete-dialog/delete-dialog.component';
import { AppPageHeaderComponent } from 'src/app/shared/page-header/page-header.component';

@Component({
  selector: 'app-hotel-management',
  templateUrl: './hotel-management.component.html',
  styleUrls: ['./hotel-management.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    MatProgressSpinnerModule,
    MatTableModule,
    MatPaginatorModule,
    RouterModule,
    AppPageHeaderComponent,
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

  dataSource: HotelManagementHotelListing[] = [];

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
      data: {
        title: 'Delete Confirmation',
        content: 'Are you sure you want to delete this hotel?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result == true) {
        this.hotelService.deleteHotel(hotelId).subscribe(() => {
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
    this.dataLoaded = false;

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
