import { Injectable, OnInit } from '@angular/core';
import { SessionService } from './session.service'; 
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, finalize, tap } from 'rxjs';
import { UserAuthentication } from '../models/user.model';
import { NavbarService } from './navbar.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnInit {

  private userRole: string = "";
  private username: string = "";
  private baseUrl = 'https://localhost:7059/api';

  constructor(
    private sessionService: SessionService,
    private navbarService: NavbarService,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.getUserUsername();
    this.getUserRole();
  }

  private getUserUsername() {
    this.fetchUserUsername().subscribe(
      (username: string) => {
        this.username = username;
      },
      (error) => {
        console.error('Error fetching user roles', error);
      }
    );
  }

  private getUserRole(): void {
    this.fetchUserRole().subscribe(
      (role: string) => {
        this.userRole = role;
      },
      (error) => {
        console.error('Error fetching user roles', error);
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

  fetchUserUsername(): Observable<string> {
    return this.http.get<string>('api/auth/logged-user-username');
  }

  fetchUserRole(): Observable<string> {
    return this.http.get<string>('/api/auth/logged-user-role');
  }

  hasRole(expectedRole: string): Observable<boolean> {
    return new Observable<boolean>((observer) => {
      if (this.userRole !== "") {
        observer.next(this.userRole === expectedRole);
        observer.complete();
      } else {
        this.fetchUserRole().subscribe(
          (role: string) => {
            this.userRole = role;
            observer.next(this.userRole === expectedRole);
            observer.complete();
          },
          (error) => {
            console.error('Error fetching user roles', error);
            observer.next(false);
            observer.complete();
          }
        );
      }
    });
  }

  isAuthenticated(): boolean {
    return !!this.sessionService.getDecodedUserDetails();
  }

  getUserDetails(): any {
    return this.sessionService.getDecodedUserDetails();
  }
}
