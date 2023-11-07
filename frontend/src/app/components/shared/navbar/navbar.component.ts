import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnDestroy {
  username: string = ""
  role: string = ""
  userDetailsSubscription: Subscription | undefined;

  constructor(private authService: AuthService) {
    this.userDetailsSubscription = this.authService.getUserClaims().subscribe(details => {
      if (details) {
        this.username = details.username;
        this.role = details.role
      } else {
        this.username = '';
        this.role = '';
      }
    });
  }

  ngOnDestroy(): void {
    if (this.userDetailsSubscription) {
      this.userDetailsSubscription.unsubscribe();
    }
  }

  logout(): void {
    this.authService.logout().subscribe({
      next: (response) => {
        this.username = "";
        this.role = ""
      },
    });
  }
}
