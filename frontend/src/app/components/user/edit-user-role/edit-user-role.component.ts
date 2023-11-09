import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserEditRole } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-edit-user-role',
  templateUrl: './edit-user-role.component.html',
  styleUrls: ['./edit-user-role.component.css']
})
export class EditUserRoleComponent {
  userForm!: FormGroup;
  userId: string = "";
  userInfo?: UserEditRole;

  constructor(
    private formBuilder: FormBuilder,
    private toastrService: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService
  ) {
    this.route.params.subscribe(params => {
      this.userId = params['id'];

      this.userService.getUserRoleEdit(this.userId).subscribe(response => {
        this.userInfo = response;
        this.userForm = this.formBuilder.group({
          username: new FormControl({ value: this.userInfo.username, disabled: true }),
          role: new FormControl(this.userInfo.role)
        });
      })
    });
  }
  
  ngOnInit() {
    this.userForm = this.formBuilder.group({
      username: ['', Validators.required],
      role: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.userForm.valid) {
      const usernameControl = this.userForm.get('username')!;
      const roleControl = this.userForm.get('role')!;

      const userData = {
        username: usernameControl.value,
        role: roleControl.value
      };

      this.userService.updateUserRole(userData).subscribe(
        (response: any) => {
          this.toastrService.success('Update was successful');
          this.router.navigate(['/user-management']);
        },
        (error: any) => {
          this.toastrService.error('Update failed');
        }
      );
      
    } 
  }
}
