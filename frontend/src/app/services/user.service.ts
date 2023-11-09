import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserEditRole } from '../models/user.model';

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

  deleteUser(userId: string): Observable<any> {
    const url = `${this.baseUrl}/users`;
    
    let params = new HttpParams()
      .set('id', userId!.toString())

    return this.http.delete(url, { params });
  }

  getUserRoleEdit(userId: string): Observable<UserEditRole> {
    const url = `${this.baseUrl}/users/role/${userId}`;

    return this.http.get(url) as Observable<UserEditRole>;
  }

  updateUserRole(userData: any): Observable<any> {
    const url = `${this.baseUrl}/users/role`;

    let params = new HttpParams()
      .set('username', userData.username)
      .set('changedRole', userData.role);

    return this.http.put(url, null, { params: params });
  }
}
