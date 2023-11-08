import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserManagementHotelListing } from '../models/hotel.model';

@Injectable({
  providedIn: 'root'
})
export class HotelService {
  private baseUrl = "/api/";

  constructor(
    private http: HttpClient
  ) { }

  getHotelAutocomplete(hotelName: string): Observable<UserManagementHotelListing[]> {
    var url = `${this.baseUrl}/hotels/user-management-hotels`;
    const params = { name: hotelName };

    return this.http.get<UserManagementHotelListing[]>(url, { params });
  }
}
