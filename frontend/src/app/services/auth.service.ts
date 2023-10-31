import { Injectable } from '@angular/core';
import { SessionService } from './session.service'; 
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, finalize, tap } from 'rxjs';
import { UserAuthentication } from '../models/user.model';
import { NavbarService } from './navbar.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private sessionService: SessionService,
    private navbarService: NavbarService,
    private http: HttpClient
  ) { }

  private baseUrl = 'https://localhost:7059/api';

  login(username: string, password: string): Observable<any> {
    const loginUrl = `${this.baseUrl}/auth/login`;
  
    const credentials = {
      username: username,
      password: password
    };
  
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true
    };
  
    return this.http.post<any>(loginUrl, credentials, httpOptions).pipe(
      tap((response: UserAuthentication) => {
        this.sessionService.saveHashedUserDetails(response.username, response.role, () => {
          this.navbarService.updateUserDetails();
        });
      })
    );
  }
  

  logout(): Observable<any> {
    const logoutUrl = `${this.baseUrl}/auth/logout`;

    return this.http.post<any>(logoutUrl, '').pipe(
      tap({
        next: () => {
          this.sessionService.clearHashedUserDetails();
          this.navbarService.updateUserDetails();
        },
        error: (error) => {
          console.error(error);
        }
      })
    );
  }

  isAuthenticated(): boolean {
    return !! this.sessionService.getDecodedUserDetails();
  }

  getUserDetails(): any {
    return this.sessionService.getDecodedUserDetails();
  }
}