import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { UserDTO } from '../models/user-dto.model';
import { LoginDTO } from '../models/login-dto.model';
import { AuthenticatedDTO } from '../models/authenticated-dto.model';
import { UserRole } from '../enums/user-role.enum';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7000/auth-service';

  constructor(private http: HttpClient) { }

  register(userDto: UserDTO): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/Auth/register`, userDto);
  }

  login(loginDto: LoginDTO): Observable<AuthenticatedDTO> {
    return this.http.post<AuthenticatedDTO>(`${this.apiUrl}/Auth/login`, loginDto);
  }

  refreshToken(authenticatedResponse: AuthenticatedDTO): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/Token/refresh`, authenticatedResponse);
  }

  revokeToken(refreshToken: string): void {
    const body = {
      refreshToken: refreshToken
    };

    this.http.post(`${this.apiUrl}/Token/revoke`, body, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
      })
    }).subscribe({
      next: (response) => {
        console.log('Токен успешно отозван', response);
      },
      error: (error) => {
        console.error('Ошибка при отзыве токена:', error);
      }
    });
  }




  revokeAllTokens(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/Token/revoke-all`, null, {
      headers: this.getAuthHeaders()
    });
  }

  getId(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/Auth/get-id`, {
      headers: this.getAuthHeaders()
    });
  }

  private getAuthHeaders(): { [header: string]: string } {
    const token = localStorage.getItem('accessToken');
    return {
      'Authorization': `Bearer ${token}`
    };
  }

  public getRoleFromToken(): Observable<UserRole | null> {
    const token = localStorage.getItem('accessToken');

    if (!token) {
      return of(null); 
    }

    const payload = token.split('.')[1];
    if (!payload) {
      return of(null);
    }

    try {
      const decodedPayload = atob(payload);
      const payloadObject = JSON.parse(decodedPayload);

      const role = payloadObject['role'] || payloadObject['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;

      if (role === 'Admin') {
        return of(UserRole.Admin);
      } else if (role === 'User') {
        return of(UserRole.User);
      } else {
        return of(null);
      }
    } catch (error) {
      console.error('Ошибка при декодировании токена', error);
      return throwError(() => new Error('Ошибка при декодировании токена'));
    }
  }

}
