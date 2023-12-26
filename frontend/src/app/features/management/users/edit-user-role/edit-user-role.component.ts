import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatOptionModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

import { UserEditRole } from '../user.model';

import { UserService } from '../user.service';

import { AppPageHeaderComponent } from 'src/app/shared/page-header/page-header.component';

@Component({
  selector: 'app-edit-user-role',
  templateUrl: './edit-user-role.component.html',
  styleUrls: ['./edit-user-role.component.scss'],
  standalone: true,
  imports: [
    MatProgressSpinnerModule,
    ReactiveFormsModule,
    MatOptionModule,
    MatFormFieldModule,
    CommonModule,
    MatFormFieldModule,
    RouterModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    AppPageHeaderComponent,
  ],
})
export class EditUserRoleComponent implements OnInit {
  userId: string = '';
  userInfo?: UserEditRole;

  dataLoaded = false;

  formBuilder = inject(FormBuilder);
  snackBar = inject(MatSnackBar);
  router = inject(Router);
  route = inject(ActivatedRoute);
  userService = inject(UserService);

  userForm = this.formBuilder.group({
    username: ['', Validators.required],
    role: ['', Validators.required],
  });

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.userId = params['id'];

      this.userService.getUserRoleEdit(this.userId).subscribe((response) => {
        this.userInfo = response;
        this.userForm.setValue({
          username: this.userInfo.username,
          role: this.userInfo.role,
        });
        this.dataLoaded = true;
      });
    });
  }

  onSubmit() {
    if (this.userForm.valid) {
      const usernameControl = this.userForm.get('username')!;
      const roleControl = this.userForm.get('role')!;

      const userData = {
        username: usernameControl.value,
        role: roleControl.value,
      };

      this.userService.updateUserRole(userData).subscribe(
        () => {
          this.snackBar.open('Role update was successful', 'Close', {
            duration: 2000,
          });
          this.router.navigate(['/user-management']);
        },
        () => {
          this.snackBar.open('Role update failed', 'Close', {
            duration: 2000,
          });
        }
      );
    }
  }
}
