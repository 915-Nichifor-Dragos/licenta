import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { UserManagementHotelListing } from './hotel.model';

@Injectable({
  providedIn: 'root',
})
export class HotelService {
  private baseUrl = '/api/';

  http = inject(HttpClient);

  getHotelAutocomplete(
    hotelName: string
  ): Observable<UserManagementHotelListing[]> {
    var url = `${this.baseUrl}/hotels/user-management-hotels`;
    const params = { name: hotelName };

    return this.http.get<UserManagementHotelListing[]>(url, { params });
  }

  getHotels(
    pageSize?: number,
    pageIndex?: number,
    isAscending?: boolean,
    sortAttribute?: string
  ): Observable<any> {
    var url = `${this.baseUrl}/hotels`;

    let params = new HttpParams()
      .set('pageSize', pageSize!.toString())
      .set('pageIndex', pageIndex!.toString())
      .set('isAscending', isAscending!.toString())
      .set('sortAttribute', sortAttribute!.toString());

    return this.http.get<any>(url, { params });
  }

  deleteHotel(hotelId: string): Observable<any> {
    const url = `${this.baseUrl}/hotels`;

    let params = new HttpParams().set('id', hotelId);

    return this.http.delete(url, { params });
  }
}
