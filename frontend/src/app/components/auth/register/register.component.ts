import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  userForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder
    ) { }

  ngOnInit() {
    this.userForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(30)]],
      password: [''],
      confirmPassword: [''],
      firstName: [''],
      lastName: [''],
      email: [''],
      birthDate: [''],
      gender: [''],
      bio: [''],
      profilePicture: [''],
    });
  }

  onSubmit() {
    if (this.userForm.valid) {
      console.log(this.userForm.value);
    } else {

    }
  }
}
