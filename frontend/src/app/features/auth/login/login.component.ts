import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { MatSnackBar } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, ReactiveFormsModule],
})
export class LoginComponent {
  formBuilder = inject(FormBuilder);
  authService = inject(AuthService);
  snackBar = inject(MatSnackBar);
  router = inject(Router);

  loginForm = this.formBuilder.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  login() {
    if (this.loginForm.valid) {
      const usernameControl = this.loginForm.get('username');
      const passwordControl = this.loginForm.get('password');

      if (!usernameControl || !passwordControl) {
        return;
      }

      const userData = {
        username: usernameControl.value,
        password: passwordControl.value,
      };

      this.authService.login(userData).subscribe(
        () => {
          this.snackBar.open('Log in was successful', 'Close', {
            duration: 2000,
          });
          this.router.navigate(['/home']);
        },
        () => {
          this.snackBar.open('Failed to log in', 'Close', {
            duration: 2000,
          });
        }
      );
    }
  }
}
