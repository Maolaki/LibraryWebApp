import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthorDTO } from '../models/author-dto.model';
import { BookDTO } from '../models/book-dto.model';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  private apiUrl = 'https://localhost:7000/author-service/Author';

  constructor(private http: HttpClient) { }

  getAuthorId(firstName: string, lastName: string): Observable<number> {
    const params = { firstName, lastName };
    return this.http.get<number>(`${this.apiUrl}/by-name`, { params });
  }

  getAllAuthors(pageNumber: number, pageSize: number): Observable<AuthorDTO[]> {
    return this.http.get<AuthorDTO[]>(`${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
    });
  }

  getAuthor(id: number): Observable<AuthorDTO> {
    return this.http.get<AuthorDTO>(`${this.apiUrl}/${id}`, {
    });
  }

  addAuthor(authorDto: AuthorDTO): Observable<void> {
    return this.http.post<void>(this.apiUrl, authorDto, {
      headers: this.getAuthHeaders()
    });
  }

  updateAuthor(id: number, authorDto: AuthorDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, authorDto, {
      headers: this.getAuthHeaders()
    });
  }

  deleteAuthor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  getAllBooksByAuthor(id: number, pageNumber: number = 1, pageSize: number = 10): Observable<BookDTO[]> {
    return this.http.get<BookDTO[]>(`${this.apiUrl}/${id}/books?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
    });
  }

  private getAuthHeaders(): { [header: string]: string } {
    const token = localStorage.getItem('accessToken');
    return {
      'Authorization': `Bearer ${token}`
    };
  }
}
