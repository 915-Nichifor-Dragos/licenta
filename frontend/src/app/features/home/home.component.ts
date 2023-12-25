import { Component, Input, inject } from '@angular/core';

import { AuthService } from '../auth/auth.service';

import { UserClaim } from 'src/app/features/auth/auth.model';

import { AppPageHeaderComponent } from 'src/app/shared/page-header/page-header.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  standalone: true,
  imports: [AppPageHeaderComponent],
})
export class HomeComponent {
  authService = inject(AuthService);

  @Input() userClaims!: UserClaim;
}
