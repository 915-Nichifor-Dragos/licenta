import { Component, OnDestroy, inject } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AuthService } from 'src/app/features/auth/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styles: [
    `
      .spacer {
        flex: 1;
      }
    `,
  ],
  standalone: true,
  imports: [CommonModule, RouterModule, MatToolbarModule, MatButtonModule],
})
export class NavbarComponent implements OnDestroy {
  authService = inject(AuthService);

  username: string = '';
  role: string = '';
  userDetailsSubscription: Subscription;

  constructor() {
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
      next: () => {
        this.username = '';
        this.role = '';
      },
    });
  }
}
