import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { AuthService } from '../auth.service';

import { AppPageHeaderComponent } from 'src/app/shared/page-header/page-header.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatOptionModule,
    MatNativeDateModule,
    MatSelectModule,
    RouterModule,
    MatButtonModule,
    AppPageHeaderComponent,
    MatStepperModule,
  ],
})
export class RegisterComponent {
  formBuilder = inject(FormBuilder);
  authService = inject(AuthService);
  snackBar = inject(MatSnackBar);
  router = inject(Router);

  userDetailsFormGroup = this.formBuilder.group({
    firstName: [
      '',
      [Validators.required, Validators.minLength(2), Validators.maxLength(100)],
    ],
    lastName: [
      '',
      [Validators.required, Validators.minLength(2), Validators.maxLength(100)],
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
  });

  userCredentialsFormGroup = this.formBuilder.group(
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
    },
    { validator: this.passwordConfirmationValidator }
  );

  userAdditionalInfoFormGroup = this.formBuilder.group({
    address: ['', Validators.maxLength(100)],
    bio: ['', Validators.maxLength(1000)],
    profilePicture: [
      '',
      Validators.pattern('([a-zA-Z0-9s_\\.-:])+(.jpg|.png|.jpeg)$'),
    ],
  });

  getFirstNameErrorMessage() {
    const firstNameControl = this.userDetailsFormGroup.get('firstName');

    if (!firstNameControl) {
      return;
    }

    if (firstNameControl.hasError('required')) {
      return 'You must enter a value for first name';
    }

    if (firstNameControl.hasError('minlength')) {
      return 'The first name must be at least 2 characters';
    }

    if (firstNameControl.hasError('maxlength')) {
      return 'The first name must be at mose 100 characters';
    }

    return '';
  }

  getLastNameErrorMessage() {
    const lastNameControl = this.userDetailsFormGroup.get('lastName');

    if (!lastNameControl) {
      return;
    }

    if (lastNameControl.hasError('required')) {
      return 'You must enter a value for first name';
    }

    if (lastNameControl.hasError('minlength')) {
      return 'The last name must be at least 2 characters';
    }

    if (lastNameControl.hasError('maxlength')) {
      return 'The last name must be at mose 100 characters';
    }

    return '';
  }

  getEmailErrorMessage() {
    const emailControl = this.userDetailsFormGroup.get('email');

    if (!emailControl) {
      return;
    }

    if (emailControl.hasError('required')) {
      return 'You must enter a value for email';
    }

    if (emailControl.hasError('pattern')) {
      return 'Email must include @ and a domain. Characters <>/(),. are not allowed"';
    }

    return '';
  }

  getBirthDateErrorMessage() {
    const birthDateControl = this.userDetailsFormGroup.get('birthDate');

    if (!birthDateControl) {
      return;
    }

    if (birthDateControl.hasError('required')) {
      return 'You must enter a value for birth date';
    }

    return '';
  }

  getGenderErrorMessage() {
    const genderControl = this.userDetailsFormGroup.get('gender');

    if (!genderControl) {
      return;
    }

    if (genderControl.hasError('required')) {
      return 'You must enter a value for gender';
    }

    return '';
  }

  getUsernameErrorMessage() {
    const usernameControl = this.userCredentialsFormGroup.get('username');

    if (!usernameControl) {
      return;
    }

    if (usernameControl.hasError('required')) {
      return 'You must enter a value for username';
    }

    if (usernameControl.hasError('minlength')) {
      return 'The username must be at least 4 characters';
    }

    if (usernameControl.hasError('maxlength')) {
      return 'The username must be at mose 30 characters';
    }

    if (usernameControl.hasError('pattern')) {
      return 'The username must only contain alphanumeric characters';
    }

    return '';
  }

  getPasswordErrorMessage() {
    const passwordControl = this.userCredentialsFormGroup.get('password');

    if (!passwordControl) {
      return;
    }

    if (passwordControl.hasError('required')) {
      return 'You must enter a value for username';
    }

    if (passwordControl.hasError('minlength')) {
      return 'The username must be at least 8 characters';
    }

    if (passwordControl.hasError('maxlength')) {
      return 'The username must be at mose 30 characters';
    }

    if (passwordControl.hasError('pattern')) {
      return `No white spaces are allowed. Must have at least 1 lowercase letter,
        digit and special character`;
    }

    return '';
  }

  getConfirmPasswordErrorMessage() {
    const confirmPasswordControl =
      this.userCredentialsFormGroup.get('confirmPassword');

    if (!confirmPasswordControl) {
      return;
    }

    if (confirmPasswordControl.hasError('required')) {
      return 'You must enter a value for confirm password';
    }

    if (confirmPasswordControl.hasError('passwordMismatch')) {
      return 'Must be the same as the password';
    }

    return '';
  }

  getAddressErrorMessage() {
    const addressControl = this.userAdditionalInfoFormGroup.get('address');

    if (!addressControl) {
      return;
    }

    if (addressControl.hasError('maxlength')) {
      return 'The address can be at most 100 characters';
    }

    return '';
  }

  getBioErrorMessage() {
    const bioControl = this.userAdditionalInfoFormGroup.get('bio');

    if (!bioControl) {
      return;
    }

    if (bioControl.hasError('maxlength')) {
      return 'The bio can be at most 1000 characters';
    }

    return '';
  }

  getProfilePictureErrorMessage() {
    const profilePictureControl =
      this.userCredentialsFormGroup.get('profilePicture');

    if (!profilePictureControl) {
      return;
    }

    if (profilePictureControl.hasError('pattern')) {
      return 'Must be .jpg, .png, .jpeg and under 10MB';
    }

    return '';
  }

  passwordConfirmationValidator(formGroup: FormGroup) {
    const passwordControl = formGroup.get('password');
    const confirmPasswordControl = formGroup.get('confirmPassword');
    console.log(confirmPasswordControl);
    if (!passwordControl || !confirmPasswordControl) {
      return;
    }

    if (passwordControl.value !== confirmPasswordControl.value) {
      confirmPasswordControl.setErrors({ passwordMismatch: true });
    } else {
      if (confirmPasswordControl.getError('passwordMismatch')) {
        confirmPasswordControl.setErrors({ passwordMismatch: false });
      }
    }
  }

  createAccount() {
    if (
      this.userAdditionalInfoFormGroup.valid &&
      this.userCredentialsFormGroup.valid &&
      this.userDetailsFormGroup.valid
    ) {
      const usernameControl = this.userCredentialsFormGroup.get('username')!;
      const passwordControl = this.userCredentialsFormGroup.get('password')!;
      const firstNameControl = this.userDetailsFormGroup.get('firstName')!;
      const lastNameControl = this.userDetailsFormGroup.get('lastName')!;
      const emailControl = this.userDetailsFormGroup.get('email')!;
      const birthDateControl = this.userDetailsFormGroup.get('birthDate')!;
      const genderControl = this.userDetailsFormGroup.get('gender')!;
      const addressControl = this.userAdditionalInfoFormGroup.get('address')!;
      const bioControl = this.userAdditionalInfoFormGroup.get('bio')!;
      //const imageUrlControl = this.userAdditionalInfoFormGroup.get('imageUrl')!;

      if (!birthDateControl.value || !genderControl.value) {
        return;
      }

      const userData = {
        username: usernameControl.value,
        password: passwordControl.value,
        firstName: firstNameControl.value,
        lastName: lastNameControl.value,
        email: emailControl.value,
        birthDate: birthDateControl.value.toString(),
        gender: parseInt(genderControl.value),
        address: addressControl?.value,
        bio: bioControl?.value,
        //imageUrl: imageUrlControl?.value
      };

      this.authService.register(userData).subscribe(
        () => {
          this.snackBar.open('Register was successful', 'Close', {
            duration: 2000,
          });
          this.router.navigate(['/login']);
        },
        () => {
          this.snackBar.open('Register failed', 'Close', {
            duration: 2000,
          });
        }
      );
    }
  }
}
