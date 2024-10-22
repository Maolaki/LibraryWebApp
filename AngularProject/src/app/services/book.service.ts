import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { BookDTO } from '../models/book-dto.model';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private apiUrl = 'https://localhost:7000/book-service/Book';

  constructor(private http: HttpClient) { }

  getAllBooks(pageNumber: number, pageSize: number): Observable<BookDTO[]> {
    return this.http.get<BookDTO[]>(`${this.apiUrl}`, {
      params: { pageNumber: pageNumber.toString(), pageSize: pageSize.toString() }
    });
  }

  getAllBooksWithFilters(pageNumber: number, pageSize: number, title: string | null, authorId: number | null, genre: string | null): Observable<BookDTO[]> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (title) {
      params = params.set('title', title);
    }

    if (authorId) {
      params = params.set('authorId', authorId);
    }

    if (genre) {
      params = params.set('genre', genre);
    }

    return this.http.get<BookDTO[]>(`${this.apiUrl}/filtered`, {
      params: params
    }).pipe(
      catchError(error => {
        console.error('Ошибка при загрузке книг:', error);
        return of([]);
      })
    );
  }

  getBook(id: number): Observable<BookDTO> {
    return this.http.get<BookDTO>(`${this.apiUrl}/${id}`, {
    });
  }

  getBooksByISBN(isbn: string): Observable<BookDTO[]> {
    return this.http.get<BookDTO[]>(`${this.apiUrl}/isbn/${isbn}`, {
    });
  }

  getAvailableCopies(isbn: string): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/${isbn}/availableCopies`, {
    });
  }

  addBook(bookDto: BookDTO): Observable<void> {
    return this.http.post<void>(this.apiUrl, bookDto, {
      headers: this.getAuthHeaders()
    });
  }

  updateBook(id: number, bookDto: BookDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, bookDto, {
      headers: this.getAuthHeaders()
    });
  }

  deleteBook(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  returnBook(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/return/${id}`, {}, {
      headers: this.getAuthHeaders()
    });
  }

  checkoutBook(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/checkout/${id}`, {}, {
      headers: this.getAuthHeaders()
    });
  }

  addBookImage(bookId: number, imageFile: File): Observable<void> {
    const formData = new FormData();
    formData.append('imageFile', imageFile);

    return this.http.post<void>(`${this.apiUrl}/image/${bookId}`, formData, {
      headers: this.getAuthHeaders()
    });
  }

  getBookImage(bookId: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/image/${bookId}`, {
      responseType: 'blob'
    });
  }

  notifyCheckoutDate(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/notify/${id}`, {}, {
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
