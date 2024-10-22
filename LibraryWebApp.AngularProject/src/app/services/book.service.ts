import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../models/book.model';
import { BookDTO } from '../models/book-dto.model';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private apiUrl = 'https://localhost:7000/book-service/Book';

  constructor(private http: HttpClient) { }

  getAllBooks(pageNumber: number, pageSize: number): Observable<Book[]> {
    return this.http.get<Book[]>(`${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
      headers: this.getHeaders()
    });
  }

  getBook(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/${id}`, {
      headers: this.getHeaders()
    });
  }

  getBookByISBN(isbn: string): Observable<Book[]> {
    return this.http.get<Book[]>(`${this.apiUrl}/isbn/${isbn}`, {
      headers: this.getHeaders()
    });
  }

  addBook(bookDto: BookDTO): Observable<void> {
    return this.http.post<void>(this.apiUrl, bookDto, {
      headers: this.getHeaders()
    });
  }

  updateBook(id: number, bookDto: BookDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, bookDto, {
      headers: this.getHeaders()
    });
  }

  deleteBook(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, {
      headers: this.getHeaders()
    });
  }

  returnBook(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/return/${id}`, {}, {
      headers: this.getHeaders()
    });
  }

  checkoutBook(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/checkout/${id}`, {}, {
      headers: this.getHeaders()
    });
  }

  addBookImage(bookId: number, imageFile: File): Observable<void> {
    const formData = new FormData();
    formData.append('imageFile', imageFile);

    return this.http.post<void>(`${this.apiUrl}/image/${bookId}`, formData, {
      headers: this.getHeaders()
    });
  }

  getBookImage(bookId: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/image/${bookId}`, {
      headers: this.getHeaders(),
      responseType: 'blob'
    });
  }

  notifyCheckoutDate(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/notify/${id}`, {}, {
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
