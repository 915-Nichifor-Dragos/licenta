import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = "/api/";

  constructor(
    private http: HttpClient
  ) { }

  getSubordinates(hotelId?: string, pageSize?: number, pageIndex?: number, isAscending?: boolean, sortAttribute?: string): Observable<any> {
    var url = `${this.baseUrl}/users/subordinates`;

    let params = new HttpParams()
      .set('pageSize', pageSize!.toString())
      .set('pageIndex', pageIndex!.toString())
      .set('isAscending', isAscending!.toString())
      .set('sortAttribute', sortAttribute!.toString());

    if (hotelId != "all") {
      url = `${this.baseUrl}/users/subordinates/${hotelId}`;
    }
  
    return this.http.get<any>(url, { params });
  }
}
