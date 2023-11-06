import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { UserClaim } from '../models/authentication.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService{
  private userClaims: BehaviorSubject<UserClaim> = new BehaviorSubject<UserClaim>({ username: '', role: '' });
  private baseUrl = "/api/";

  constructor(
    private http: HttpClient
  ) { 
    this.updateClaims();
  }

  fetchUserClaims(): Observable<UserClaim> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true
    };

    return this.http.get<UserClaim>(`${this.baseUrl}/auth/logged-user-claims`, httpOptions);
  }

  getUserClaims(): Observable<UserClaim> {
    return this.userClaims.asObservable();
  }

  updateClaims(): void {
    this.fetchUserClaims().subscribe(
      (userClaims: UserClaim) => {
        this.userClaims.next(userClaims);
      }
    );
  }

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
      tap((response: any) => {
        if (response.success) {
          this.updateClaims();
        }
      })
    );
  }
  
  logout(): Observable<any> {
    const logoutUrl = `${this.baseUrl}/auth/logout`;

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true
    };

    return this.http.post<any>(logoutUrl, '', httpOptions).pipe(
      tap(() => {
        this.updateClaims();
      })
    );
  }

  hasRole(expectedRole: string): Observable<boolean> {
    return new Observable<boolean>((observer) => {
      this.userClaims.subscribe((claims) => {
        if (claims.role != "") {
          observer.next(claims.role === expectedRole);
          observer.complete();
        } else {
          this.fetchUserClaims().subscribe(
            (userClaims: UserClaim) => {
              this.userClaims.next(userClaims);
              observer.next(claims.role === expectedRole);
              observer.complete();
            },
            () => {
              observer.next(false);
              observer.complete();
            }
          );
        }
      });
    });
  }
}
