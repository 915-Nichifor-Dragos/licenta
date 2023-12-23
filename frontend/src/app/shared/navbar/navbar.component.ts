import { CommonModule } from '@angular/common';
import { Component, OnDestroy } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/components/auth/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule],
})
export class NavbarComponent implements OnDestroy {
  username: string = '';
  role: string = '';
  userDetailsSubscription: Subscription | undefined;

  constructor(private authService: AuthService) {
    this.userDetailsSubscription = this.authService
      .getUserClaims()
      .subscribe((details) => {
        if (details) {
          this.username = details.username;
          this.role = details.role;
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
        this.username = '';
        this.role = '';
      },
    });
  }
}
