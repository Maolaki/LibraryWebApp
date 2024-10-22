import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserDTO } from '../models/user-dto.model';
import { LoginDTO } from '../models/login-dto.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7000/auth-service';

  constructor(private http: HttpClient) { }

  register(userDto: UserDTO): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/Auth/register`, userDto);
  }

  login(loginDto: LoginDTO): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Auth/login`, loginDto);
  }

  refreshToken(authenticatedResponse: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Token/refresh`, authenticatedResponse);
  }

  revokeToken(refreshToken: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/Token/revoke`, refreshToken, {
      headers: this.getAuthHeaders()
    });
  }

  revokeAllTokens(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/Token/revoke-all`, null, {
      headers: this.getAuthHeaders()
    });
  }

  private getAuthHeaders(): { [header: string]: string } {
    const token = localStorage.getItem('accessToken');
    return {
      'Authorization': `Bearer ${token}`
    };
  }
}
