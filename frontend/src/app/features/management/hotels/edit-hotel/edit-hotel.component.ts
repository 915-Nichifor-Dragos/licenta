import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';

@Component({
  selector: 'app-edit-hotel',
  templateUrl: './edit-hotel.component.html',
  styleUrls: ['./edit-hotel.component.scss'],
  standalone: true,
  imports: [
    MatProgressSpinnerModule,
    MatTabsModule,
    MatIconModule,
    CommonModule,
    RouterModule,
  ],
})
export class EditHotelComponent {
  dataLoaded = true;
}
