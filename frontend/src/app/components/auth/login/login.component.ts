import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { MatSnackBar } from '@angular/material/snack-bar';

import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const usernameControl = this.loginForm.get('username')!;
      const passwordControl = this.loginForm.get('password')!;

      const userData = {
        username: usernameControl.value,
        password: passwordControl.value
      };

      this.authService.login(userData).subscribe(
        (response: any) => {
          this.snackBar.open('Log in was successful', 'Close', {
            duration: 2000,
          });
          this.router.navigate(['/home']);
        },
        (error: any) => {
          this.snackBar.open('Failed to log in', 'Close', {
            duration: 2000,
          });
        }
      );
      
    } 
    else {
      // Set up a div that says invalid form
    }
  }
}
