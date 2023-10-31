import { Injectable } from '@angular/core';
import { SessionService } from './session.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserAuthentication } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class NavbarService {
  private userDetailsSubject: BehaviorSubject<UserAuthentication | null>;

  constructor(
    private sessionService: SessionService
    ) {
    this.userDetailsSubject = new BehaviorSubject(this.sessionService.getDecodedUserDetails());
  }

  getUserDetails(): Observable<UserAuthentication | null> {
    return this.userDetailsSubject.asObservable();
  }

  updateUserDetails(): void {
    const userDetails = this.sessionService.getDecodedUserDetails();
    this.userDetailsSubject.next(userDetails);
  }
}
