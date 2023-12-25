import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { MatSnackBar } from '@angular/material/snack-bar';

import { AuthService } from '../auth.service';

import { AppPageHeaderComponent } from 'src/app/shared/page-header/page-header.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    AppPageHeaderComponent,
  ],
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

  getUsernameErrorMessage() {
    if (this.loginForm.get('username')!.hasError('required')) {
      return 'You must enter a value for username';
    }

    return '';
  }

  getPasswordErrorMessage() {
    if (this.loginForm.get('password')!.hasError('required')) {
      return 'You must enter a value for password';
    }

    return '';
  }

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
