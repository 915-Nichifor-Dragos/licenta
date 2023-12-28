import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  Observable,
  debounceTime,
  distinctUntilChanged,
  startWith,
  switchMap,
} from 'rxjs';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';

import { UserService } from '../user.service';
import { HotelService } from '../../hotels/hotel.service';

import { AppPageHeaderComponent } from 'src/app/shared/page-header/page-header.component';
import { UserManagementHotelListing } from '../../hotels/hotel.model';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatOptionModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatAutocompleteModule,
    MatIconModule,
    MatOptionModule,
    AppPageHeaderComponent,
  ],
})
export class AddUserComponent implements OnInit {
  formBuilder = inject(FormBuilder);
  userService = inject(UserService);
  hotelService = inject(HotelService);
  snackBar = inject(MatSnackBar);
  router = inject(Router);

  selectedHotelName = '';
  selectedHotelId = '';

  options: UserManagementHotelListing[] = [];

  hotelControl = new FormControl('', [Validators.required]);

  userFormGroup = this.formBuilder.group(
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
      role: ['', [Validators.required]],
    },
    { validator: this.passwordConfirmationValidator }
  );

  ngOnInit() {
    this.options = [];

    this.hotelControl.valueChanges
      .pipe(
        startWith(''),
        debounceTime(300),
        distinctUntilChanged(),
        switchMap((value: any) => this.fetchAutocompleteOptions(value))
      )
      .subscribe((options: UserManagementHotelListing[]) => {
        this.options = options;
      });
  }

  fetchAutocompleteOptions(value: string): Observable<any> {
    return this.hotelService.getHotelAutocomplete(value);
  }

  optionSelected(event: any) {
    this.selectedHotelName = event.option.viewValue;
    this.selectedHotelId = event.option.value;
  }

  onClearInput() {
    this.selectedHotelName = '';
    this.selectedHotelId = '';
  }

  getFirstNameErrorMessage() {
    const firstNameControl = this.userFormGroup.get('firstName');

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
    const lastNameControl = this.userFormGroup.get('lastName');

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
    const emailControl = this.userFormGroup.get('email');

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
    const birthDateControl = this.userFormGroup.get('birthDate');

    if (!birthDateControl) {
      return;
    }

    if (birthDateControl.hasError('required')) {
      return 'You must enter a value for birth date';
    }

    return '';
  }

  getGenderErrorMessage() {
    const genderControl = this.userFormGroup.get('gender');

    if (!genderControl) {
      return;
    }

    if (genderControl.hasError('required')) {
      return 'You must enter a value for gender';
    }

    return '';
  }

  getUsernameErrorMessage() {
    const usernameControl = this.userFormGroup.get('username');

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
    const passwordControl = this.userFormGroup.get('password');

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
    const confirmPasswordControl = this.userFormGroup.get('confirmPassword');

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

  getRoleErrorMessage() {
    const roleControl = this.userFormGroup.get('role');

    if (!roleControl) {
      return;
    }

    if (roleControl.hasError('required')) {
      return 'You must enter a value for role';
    }

    return '';
  }

  getHotelErrorMessage() {
    if (!this.hotelControl) {
      return;
    }

    if (this.hotelControl.hasError('required')) {
      return 'You must enter a value for hotel';
    }

    return '';
  }

  passwordConfirmationValidator(formGroup: FormGroup) {
    const passwordControl = formGroup.get('password');
    const confirmPasswordControl = formGroup.get('confirmPassword');

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

  createUser() {
    if (this.userFormGroup.valid && this.hotelControl.valid) {
      const usernameControl = this.userFormGroup.get('username')!;
      const passwordControl = this.userFormGroup.get('password')!;
      const firstNameControl = this.userFormGroup.get('firstName')!;
      const lastNameControl = this.userFormGroup.get('lastName')!;
      const emailControl = this.userFormGroup.get('email')!;
      const birthDateControl = this.userFormGroup.get('birthDate')!;
      const genderControl = this.userFormGroup.get('gender')!;
      const roleControl = this.userFormGroup.get('role')!;

      const userData = {
        username: usernameControl.value,
        password: passwordControl.value,
        firstName: firstNameControl.value,
        lastName: lastNameControl.value,
        email: emailControl.value,
        birthDate: birthDateControl.value.toISOString().split('T')[0],
        gender: parseInt(genderControl.value),
        roleId: parseInt(roleControl.value),
        hotelId: this.selectedHotelId,
      };

      this.userService.addUser(userData).subscribe(
        () => {
          this.snackBar.open('User was added successful', 'Close', {
            duration: 2000,
          });
          this.router.navigate(['/user-management']);
        },
        () => {
          this.snackBar.open('User registration failed', 'Close', {
            duration: 2000,
          });
        }
      );
    }
  }
}
