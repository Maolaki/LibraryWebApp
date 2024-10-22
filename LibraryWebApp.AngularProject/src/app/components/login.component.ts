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
          Validators.pattern(/[A-Za-z]/),  // Должна быть хотя бы одна буква
          Validators.pattern(/[0-9]/),     // Должна быть хотя бы одна цифра
          Validators.pattern(/[\W_]/)      // Должен быть хотя бы один спец. символ
        ]
      ]
    });
  }

  login(): void {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          console.log('Login successful', response);
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem('refreshToken', response.refreshToken);
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          console.error('Login error: ', err);
        }
      });
    }
  }

  // Изменяем имя геттера с login на loginControl
  get loginControl() {
    return this.loginForm.get('login');
  }

  get passwordControl() {
    return this.loginForm.get('password');
  }
}
