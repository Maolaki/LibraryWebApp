import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Author } from '../models/author.model';
import { AuthorDTO } from '../models/author-dto.model';
import { Book } from '../models/book.model';

@Injectable({
  providedIn: 'root'
})
export class AuthorsService {
  private apiUrl = 'https://localhost:7000/author-service/Author';

  constructor(private http: HttpClient) { }

  getAllAuthors(pageNumber: number, pageSize: number): Observable<Author[]> {
    return this.http.get<Author[]>(`${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
      headers: this.getHeaders()
    });
  }

  getAuthor(id: number): Observable<Author> {
    return this.http.get<Author>(`${this.apiUrl}/${id}`, {
      headers: this.getHeaders()
    });
  }

  addAuthor(authorDto: AuthorDTO): Observable<void> {
    return this.http.post<void>(this.apiUrl, authorDto, {
      headers: this.getHeaders()
    });
  }

  updateAuthor(id: number, authorDto: AuthorDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, authorDto, {
      headers: this.getHeaders()
    });
  }

  deleteAuthor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, {
      headers: this.getHeaders()
    });
  }

  getAllBooksByAuthor(id: number, pageNumber: number = 1, pageSize: number = 10): Observable<Book[]> {
    return this.http.get<Book[]>(`${this.apiUrl}/${id}/books?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
      headers: this.getHeaders()
    });
  }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('accessToken'); 
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }
}
