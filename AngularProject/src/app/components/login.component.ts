import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      login: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
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
      ]
    });
  }

  login(): void {
    if (this.loginForm.valid) {
      const loginDto = {
        login: this.loginForm.get('login')?.value || '',
        password: this.loginForm.get('password')?.value || ''
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
    }
  }

  get loginControl() {
    return this.loginForm.get('login');
  }

  get passwordControl() {
    return this.loginForm.get('password');
  }
}
