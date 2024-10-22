import { Component, OnInit } from '@angular/core';
import { BookService } from '../services/book.service';
import { BookDTO } from '../models/book-dto.model';
import { AuthService } from '../services/auth.service';
import { BookGenre } from '../enums/book-genre.enum';

@Component({
  selector: 'app-my-books',
  templateUrl: './my-books.component.html',
  styleUrls: ['./my-books.component.css']
})
export class MyBooksComponent implements OnInit {
  myBooks: BookDTO[] = [];
  totalCount: number = 0;
  pageNumber: number = 1;
  pageSize: number = 10;

  imageUrls: { [bookId: number]: string | null } = {};

  constructor(private bookService: BookService, private authService: AuthService) { }

  ngOnInit(): void {
    this.getMyBooks();
  }

  getGenreName(genre: BookGenre): string {
    return BookGenre[genre];
  }

  getMyBooks(): void {
    this.authService.getId().subscribe({
      next: (userId) => {
        this.bookService.getAllBooks(this.pageNumber, this.pageSize).subscribe({
          next: (books) => {
            this.myBooks = books.filter(book => book.userId === userId);
            this.totalCount = this.myBooks.length;
          },
          error: (error) => {
            console.error('Ошибка при получении всех книг:', error);
          }
        });
      },
      error: (error) => {
        console.error('Ошибка при получении ID пользователя:', error);
      }
    });
  }

  returnBook(bookId: number): void {
    this.bookService.returnBook(bookId).subscribe({
      next: () => {
        alert('Книга успешно возвращена.');
        this.getMyBooks(); 
      },
      error: (error) => {
        console.error('Ошибка при возврате книги:', error);
        alert('Не удалось вернуть книгу. Попробуйте еще раз.');
      }
    });
  }

  loadImage(bookId: number): void {
    this.bookService.getBookImage(bookId).subscribe({
      next: (imageBlob) => {
        const imageUrl = URL.createObjectURL(imageBlob);
        this.imageUrls[bookId] = imageUrl;
      },
      error: (err) => {
        console.error('Ошибка при получении изображения книги:', err);
        this.imageUrls[bookId] = null;
      }
    });
  }

}
