import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookService } from '../services/book.service';
import { BookDTO } from '../models/book-dto.model';
import { BookGenre } from '../enums/book-genre.enum';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EditBookModalComponent } from './edit-book-modal.component';
import { AddBookModalComponent } from './add-book-modal.component';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-admin-book-list',
  templateUrl: './admin-book-list.component.html',
  styleUrls: ['./admin-book-list.component.css']
})
export class AdminBookListComponent implements OnInit {
  books: BookDTO[] = [];
  totalCount: number = 0;
  pageNumber: number = 1;
  pageSize: number = 10;
  titleFilter: string | null = null;
  genreFilter: BookGenre | null = null;
  imageUrls: { [key: number]: string | null } = {};
  selectedAuthorId: number | null = null;

  genres = Object.values(BookGenre);

  constructor(private bookService: BookService, private authService: AuthService, private modalService: NgbModal, private router: Router) {}

  ngOnInit(): void {
    this.getBooks();
  }

  getBooks(): void {
    this.bookService.getAllBooksWithFilters(
      this.pageNumber,
      this.pageSize,
      this.titleFilter,
      this.selectedAuthorId,
      this.genreFilter !== null ? BookGenre[this.genreFilter] : null
    )
      .subscribe({
        next: (response) => {
          this.books = response || [];
          this.totalCount = this.books.length;

          this.books.forEach(book => {
            this.loadImage(book.id);
          });
        },
        error: (err) => {
          console.error('Ошибка при получении списка книг:', err);
        }
      });
  }

  getGenreName(genre: BookGenre): string {
    return BookGenre[genre];
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

  getImage(bookId: number): string | null {
    return this.imageUrls[bookId] || null;
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.getBooks();
  }

  onGenreChange(selectedGenre: BookGenre | null): void {
    this.genreFilter = selectedGenre;
    this.onSearch();
  }

  onPageChange(page: number): void {
    this.pageNumber = page;
    this.getBooks();
  }

  logout(): void {
    const refreshToken = localStorage.getItem('refreshToken');
    if (refreshToken) {
      this.authService.revokeToken(refreshToken);

      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      this.router.navigate(['/login']);
    }
  }

  openEditModal(book: BookDTO): void {
    const modalRef = this.modalService.open(EditBookModalComponent, {
      backdrop: 'static',
      centered: true
    });
    modalRef.componentInstance.book = { ...book };

    modalRef.result.then(
      (updatedBook) => {
        if (updatedBook) {
          this.updateBook(updatedBook);
        }
      },
      () => {
        console.log('Модальное окно закрыто без изменений');
      }
    );
  }


  updateBook(book: BookDTO): void {
    if (!book.id) {
      console.error('Book ID is undefined. Cannot update the book.');
      return;
    }

    this.bookService.updateBook(book.id, book).subscribe({
      next: () => {
        alert('Книга успешно обновлена.');
        this.getBooks();
      },
      error: (err) => {
        console.error('Ошибка при обновлении книги:', err);
      }
    });
  }

  deleteBook(bookId: number): void {
    if (confirm('Вы уверены, что хотите удалить эту книгу?')) {
      this.bookService.deleteBook(bookId).subscribe({
        next: () => {
          alert('Книга удалена.');
          this.getBooks();
        },
        error: (err) => {
          console.error('Ошибка при удалении книги:', err);
        }
      });
    }
  }

  openAddBookModal(): void {
    const modalRef = this.modalService.open(AddBookModalComponent, {
      backdrop: 'static',
      centered: true 
    });

    modalRef.result.then(
      (newBook: BookDTO) => {
        if (newBook) {
          this.books.push(newBook);
          this.totalCount++;
        }
      },
      () => {
        console.log('Модальное окно закрыто без добавления книги');
      }
    );
  }
}
