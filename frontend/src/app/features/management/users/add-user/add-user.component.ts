import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';

import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from 'src/app/components/auth/auth.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css'],
  standalone: true,
  imports: [
    MatFormFieldModule,
    ReactiveFormsModule,
    MatOptionModule,
    MatDatepickerModule,
  ],
})
export class AddUserComponent {
  formBuilder = inject(FormBuilder);
  authService = inject(AuthService);
  snackBar = inject(MatSnackBar);
  router = inject(Router);

  userForm = this.formBuilder.group(
    {
      username: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(30),
          Validators.pattern('^[a-zA-Z0-9]+$'),
        ],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(30),
          Validators.pattern(
            '^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]+$'
          ),
        ],
      ],
      confirmPassword: ['', [Validators.required]],
      firstName: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(100),
        ],
      ],
      lastName: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(100),
        ],
      ],
      email: [
        '',
        [
          Validators.required,
          Validators.pattern(
            /^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]{3,}\.[A-Za-z]{2,4}$/
          ),
        ],
      ],
      birthDate: ['', [Validators.required]],
      gender: ['', [Validators.required]],
      address: ['', Validators.maxLength(100)],
      bio: ['', Validators.maxLength(1000)],
      profilePicture: [
        '',
        Validators.pattern('([a-zA-Z0-9s_\\.-:])+(.jpg|.png|.jpeg)$'),
      ],
    },
    { validator: this.passwordConfirmationValidator }
  );

  createUser() {
    if (this.userForm.valid) {
      if (this.userForm.valid) {
        const usernameControl = this.userForm.get('username')!;
        const passwordControl = this.userForm.get('password')!;
        const firstNameControl = this.userForm.get('firstName')!;
        const lastNameControl = this.userForm.get('lastName')!;
        const emailControl = this.userForm.get('email')!;
        const birthDateControl = this.userForm.get('birthDate')!;
        const genderControl = this.userForm.get('gender')!;
        const addressControl = this.userForm.get('address')!;
        const bioControl = this.userForm.get('bio')!;
        //const imageUrlControl = this.userForm.get('imageUrl')!;

        const userData = {
          username: usernameControl.value,
          password: passwordControl.value,
          firstName: firstNameControl.value,
          lastName: lastNameControl.value,
          email: emailControl.value,
          birthDate: birthDateControl.value.toISOString(),
          gender: parseInt(genderControl.value),
          address: addressControl?.value,
          bio: bioControl?.value,
          //imageUrl: imageUrlControl?.value
        };

        this.authService.register(userData).subscribe(
          (response) => {
            this.snackBar.open('Register was successful', 'Close', {
              duration: 2000,
            });
            this.router.navigate(['/login']);
          },
          (error) => {
            this.snackBar.open('Register failed', 'Close', {
              duration: 2000,
            });
          }
        );
      }
    }
  }

  passwordConfirmationValidator(formGroup: FormGroup) {
    const password = formGroup.get('password');
    const confirmPassword = formGroup.get('confirmPassword');

    if (
      password &&
      confirmPassword &&
      password.value !== confirmPassword.value
    ) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword?.setErrors(null);
    }
  }
}
