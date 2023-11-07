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
      },
      (error: any) => {
        
      }
    );
  }

  login(userData: any): Observable<any> {
    const loginUrl = `${this.baseUrl}/auth/login`;

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true
    };
  
    return this.http.post<any>(loginUrl, userData, httpOptions).pipe(
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

  register(userData: any): Observable<any> {
    const registerUrl = `${this.baseUrl}/auth/register`;
    
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true
    };

    return this.http.post(registerUrl, userData, httpOptions);
  }

  async hasRole(expectedRole: string): Promise<boolean> {
    const claims = this.userClaims.getValue();

    if (claims.role !== '') {
      return claims.role === expectedRole;
    } else {
      try {
        const userClaims = await this.fetchUserClaims().toPromise();
        this.userClaims.next(userClaims!);

        return userClaims!.role === expectedRole;
      } catch (error) {
        return false;
      }
    }
  }

}
