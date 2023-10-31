import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserAuthentication } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { NavbarService } from 'src/app/services/navbar.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnDestroy {
  authenticatedUser: UserAuthentication | null = null;
  userDetailsSubscription: Subscription;

  constructor(
    private navbarService: NavbarService,
    private authService: AuthService
    ) {
    this.userDetailsSubscription = this.navbarService.getUserDetails().subscribe(details => {
      this.authenticatedUser = details;
    });
  }

  ngOnDestroy(): void {
    this.userDetailsSubscription.unsubscribe();
  }

  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
      },
      error: (error) => {
        console.error('Error during logout:', error);
      }
    });
  }
}
