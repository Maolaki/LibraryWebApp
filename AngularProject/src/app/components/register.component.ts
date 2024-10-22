import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { UserRole } from '../enums/user-role.enum';
import { LoginDTO } from '../models/login-dto.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  userRoles = [
    { value: UserRole.User, label: 'User' },
    { value: UserRole.Admin, label: 'Admin' }
  ];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      email: ['', [Validators.required, Validators.email]],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(25),
          Validators.pattern(/[A-Za-z]/),
          Validators.pattern(/[0-9]/),
          Validators.pattern(/[\W_]/)
        ]
      ],
      role: [UserRole.User, Validators.required]
    });
  }

  register(): void {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe({
        next: () => {
          console.log('Registration successful');
          const loginDto: LoginDTO = {
            login: this.registerForm.value.username,
            password: this.registerForm.value.password,
          };
          this.authService.login(loginDto).subscribe({
            next: (response) => {
              console.log('Login successful', response);
              localStorage.setItem('accessToken', response.accessToken);
              localStorage.setItem('refreshToken', response.refreshToken);
              this.router.navigate(['/']);
            },
            error: (err) => {
              console.error('Login error: ', err);
            }
          });
        },
        error: (err) => {
          console.error('Registration error: ', err);
        }
      });
    }
  }

  get username() {
    return this.registerForm.get('username');
  }

  get email() {
    return this.registerForm.get('email');
  }

  get password() {
    return this.registerForm.get('password');
  }
}
