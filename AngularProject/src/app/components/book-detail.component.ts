import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../services/book.service';
import { BookDTO } from '../models/book-dto.model';
import { AuthorDTO } from '../models/author-dto.model';
import { Observable } from 'rxjs';
import { BookGenre } from '../enums/book-genre.enum';
import { AuthorService } from '../services/author.service';

@Component({
  selector: 'app-book-detail',
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.css']
})
export class BookDetailComponent implements OnInit {
  book: BookDTO | null = null;
  author: AuthorDTO | null = null;
  isAvailable: boolean = false;
  availableCopiesCount: number = 0;
  imageUrl: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private authorService: AuthorService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const bookIsbn = this.route.snapshot.paramMap.get('isbn');
    if (bookIsbn) {
      this.getBookDetails(bookIsbn);
    }
  }

  goBack(): void {
    this.router.navigate(['/books']);
  }

  getBookDetails(bookIsbn: string): void {
    this.bookService.getBooksByISBN(bookIsbn).subscribe({
      next: (books) => {
        if (books.length > 0) {
          this.book = books[0];
          this.loadImage(this.book.id!);
          this.checkAvailability(this.book.isbn!);
          this.getAuthor(this.book.authorId).subscribe(author => {
            this.author = author;
          });
        } else {
          alert('Книга не найдена.');
        }
      },
      error: (error) => {
        console.error('Ошибка при получении книги по ISBN:', error);
        alert('Ошибка при получении книги.');
      },
      complete: () => {
        console.log('Получение книги завершено.');
      }
    });
  }

  loadImage(bookId: number): void {
    this.bookService.getBookImage(bookId).subscribe({
      next: (imageBlob) => {
        this.imageUrl = URL.createObjectURL(imageBlob);
      },
      error: (err) => {
        console.error('Ошибка при получении изображения книги:', err);
        this.imageUrl = null;
      }
    });
  }

  getAuthor(id: number): Observable<AuthorDTO> {
    return this.authorService.getAuthor(id);
  }

  checkAvailability(isbn: string): void {
    this.bookService.getAvailableCopies(isbn).subscribe({
      next: (count) => {
        this.availableCopiesCount = count;
        this.isAvailable = this.availableCopiesCount > 0;
      },
      error: (error) => {
        console.error('Ошибка при проверке доступных копий:', error);
      },
      complete: () => {
        console.log('Проверка доступности завершена.');
      }
    });
  }

  checkoutBook(): void {
    if (this.book && this.isAvailable) {
      this.bookService.checkoutBook(this.book.id!).subscribe({
        next: () => {
          alert('Книга успешно взята.');
          this.checkAvailability(this.book!.isbn!);
        },
        error: (error) => {
          console.error('Ошибка при взятии книги:', error);
          alert('Не удалось взять книгу. Попробуйте еще раз.');
        },
        complete: () => {
          console.log('Завершено получение результата по взятию книги.');
        }
      });
    } else {
      alert('Книга недоступна для заимствования.');
    }
  }

  getGenreName(genre: BookGenre): string {
    return BookGenre[genre];
  }
}
