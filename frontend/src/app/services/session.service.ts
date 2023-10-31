import { Injectable } from '@angular/core';
import { UserAuthentication } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private readonly HASHED_USER_DETAILS_KEY = 'hashedUserDetails';

  constructor() { }

  async hashUserDetails(username: string, role: string): Promise<string> {
    const combinedDetails = `${username}-${role}`;
    const encodedDetails = btoa(combinedDetails);

    return encodedDetails;
  }

  saveHashedUserDetails(username: string, role: string, callback: () => void): void {
    this.hashUserDetails(username, role).then((encodedDetails) => {
      sessionStorage.setItem(this.HASHED_USER_DETAILS_KEY, encodedDetails);
      callback();
    }).catch((error) => {
      console.error('Error hashing user details:', error);
    });
  }

  getDecodedUserDetails(): UserAuthentication | null {
    const encodedDetails = sessionStorage.getItem(this.HASHED_USER_DETAILS_KEY);

    if (encodedDetails) {
      const decodedDetails = atob(encodedDetails);
      const [username, role] = decodedDetails.split('-');
      
      return { username, role } as UserAuthentication;
    }

    return null;
  }

  clearHashedUserDetails(): void {
    sessionStorage.removeItem(this.HASHED_USER_DETAILS_KEY);
  }
}
