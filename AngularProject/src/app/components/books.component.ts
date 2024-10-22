import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookService } from '../services/book.service';
import { AuthService } from '../services/auth.service';
import { BookGenre } from '../enums/book-genre.enum';
import { BookDTO } from '../models/book-dto.model';
import { UserRole } from '../enums/user-role.enum';
import { AuthorDTO } from '../models/author-dto.model';
import { AuthorService } from '../services/author.service';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.css']
})
export class BooksComponent implements OnInit {
  books: BookDTO[] = [];
  totalCount: number = 0;
  pageNumber: number = 1;
  pageSize: number = 10;
  titleFilter: string | null = null;
  genreFilter: BookGenre | null = null;
  authorsFilter: AuthorDTO[] = [];
  selectedAuthorId: number | null = null;

  availabilityMap: { [isbn: string]: boolean } = {};
  imageUrls: { [bookId: number]: string | null } = {};

  genres = Object.values(BookGenre);

  isAuthenticated: boolean = false;
  isUser: boolean = false;

  constructor(
    private bookService: BookService,
    private authService: AuthService,
    private authorService: AuthorService,
    private router: Router) { }

  ngOnInit(): void {
    this.checkAuth();
    this.loadAuthors();
    this.getBooks();
  }

  loadAuthors(): void {
    this.authorService.getAllAuthors(1, 100).subscribe(authors => {
      this.authorsFilter = authors;
    });
  }

  onAuthorSelected(event: Event): void {
    const target = event.target as HTMLSelectElement;
    const value = target.value;

    if (value) {
      const [firstName, lastName] = value.split(',');
      this.authorService.getAuthorId(firstName.trim(), lastName.trim()).subscribe(authorId => {
        this.selectedAuthorId = authorId;
      });
    }
  }

  checkAuth(): void {
    const accessToken = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');

    if (accessToken && refreshToken) {
      this.isAuthenticated = true;

      this.authService.getRoleFromToken().subscribe({
        next: (role: UserRole | null) => {
          if (role === UserRole.Admin) {
            this.router.navigate(['/admin']);
          } else if (role === UserRole.User) {
            this.isUser = true;
          } else {
            this.isAuthenticated = false;
          }
        },
        error: () => {
          this.isAuthenticated = false;
          console.error('Ошибка при получении роли пользователя');
        }
      });

    }
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

  navigateTo(route: string): void {
    this.router.navigate([`/${route}`]);

  }

  getGenreName(genre: BookGenre): string {
    return BookGenre[genre];
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
          console.log('Полученные книги:', response);
          this.books = response || [];
          this.totalCount = this.books.length;

          this.books.forEach(book => {
            this.checkAvailability(book.isbn!);
            this.loadImage(book.id);
          });
          console.log('Полученные книги:', this.books);
        },
        error: (error) => {
          console.error('Ошибка при получении книг:', error);
          this.books = [];
        },
        complete: () => {
          console.log('Загрузка книг завершена.');
        }
      });
  }

  checkAvailability(isbn: string): void {
    this.bookService.getAvailableCopies(isbn).subscribe({
      next: (count) => {
        this.availabilityMap[isbn] = count > 0;
      },
      error: (error) => {
        console.error('Ошибка при проверке доступных копий:', error);
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

  viewBook(isbn: string): void {
    this.router.navigate(['/books', isbn]);
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
}
