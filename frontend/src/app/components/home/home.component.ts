import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  username: string = ""
  userDetailsSubscription: Subscription | undefined;

  constructor(private authService: AuthService) {
    this.userDetailsSubscription = this.authService.getUserClaims().subscribe(details => {
      if (details) {
        this.username = details.username;
      } else {
        this.username = '';
      }
    });
  }

  ngOnDestroy(): void {
    if (this.userDetailsSubscription) {
      this.userDetailsSubscription.unsubscribe();
    }
  }
}
