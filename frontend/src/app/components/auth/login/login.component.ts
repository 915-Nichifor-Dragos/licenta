import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
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
    private toastrService: ToastrService
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

    const usernameValue = usernameControl.value;
    const passwordValue = passwordControl.value;

      this.authService.login(usernameValue, passwordValue).subscribe(
        (response: any) => {
          this.toastrService.success('Log in was successful');
        },
        (error: any) => {
          this.toastrService.error('Failed to log in');
          console.error(error);
        }
      );
      
    } 
    else {
      // Set up a div that says invalid form
    }
  }
}
