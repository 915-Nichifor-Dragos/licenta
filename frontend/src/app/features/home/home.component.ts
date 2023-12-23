import { Component, Input, inject } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { UserClaim } from 'src/app/components/auth/auth.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  standalone: true,
})
export class HomeComponent {
  authService = inject(AuthService);

  @Input() userClaims!: UserClaim;
}
