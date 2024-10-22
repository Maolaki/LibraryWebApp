import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { BookService } from '../services/book.service';
import { AuthorService } from '../services/author.service';
import { BookDTO } from '../models/book-dto.model';
import { BookGenre } from '../enums/book-genre.enum';
import { AuthorDTO } from '../models/author-dto.model';

@Component({
  selector: 'app-add-book-modal',
  templateUrl: './add-book-modal.component.html',
  styleUrls: ['./add-book-modal.component.css']
})
export class AddBookModalComponent implements OnInit {
  book: BookDTO = {
    id: 0,
    isbn: '',
    title: '',
    description: '',
    genre: BookGenre.Fiction,
    authorId: 0
  };

  authors: AuthorDTO[] = [];
  selectedAuthorId: number | null = null;
  selectedImageFile: File | null = null;

  genres = Object.keys(BookGenre).filter(key => isNaN(Number(key))).map(key => ({
    id: BookGenre[key as keyof typeof BookGenre],
    name: key
  }));

  constructor(
    public modal: NgbActiveModal,
    private bookService: BookService,
    private authorService: AuthorService
  ) { }

  ngOnInit(): void {
    this.loadAuthors();
  }

  loadAuthors(): void {
    this.authorService.getAllAuthors(1, 100).subscribe(authors => {
      this.authors = authors;
    });
  }

  onAuthorSelected(event: Event): void {
    const target = event.target as HTMLSelectElement;
    const value = target.value;

    if (value) {
      const [firstName, lastName] = value.split(',');
      this.authorService.getAuthorId(firstName.trim(), lastName.trim()).subscribe(authorId => {
        this.selectedAuthorId = authorId;
        this.book.authorId = authorId;
      });
    }
  }


  onImageSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedImageFile = input.files[0];
    }
  }

  save(): void {
    if (this.selectedAuthorId) {
      this.book.authorId = this.selectedAuthorId;

      this.book.genre = +this.book.genre;

      this.bookService.addBook(this.book).subscribe({
        next: () => {
          this.bookService.getBooksByISBN(this.book.isbn!).subscribe({
            next: (books) => {
              const addedBook = books.length > 0 ? books[0] : null;
              if (addedBook) {
                if (this.selectedImageFile) {
                  this.bookService.addBookImage(addedBook.id!, this.selectedImageFile).subscribe({
                    next: () => {
                      alert('Книга добавлена с изображением');
                      this.modal.close(addedBook);
                    },
                    error: (err) => {
                      console.error('Ошибка при загрузке изображения:', err);
                      alert('Ошибка при загрузке изображения.');
                    }
                  });
                } else {
                  alert('Книга успешно добавлена.');
                  this.modal.close(addedBook);
                }
              } else {
                alert('Не удалось найти добавленную книгу по ISBN.');
              }
            },
            error: (err) => {
              console.error('Ошибка при получении книги по ISBN:', err);
              alert('Ошибка при получении книги по ISBN.');
            }
          });
        },
        error: (err) => {
          console.error('Ошибка при добавлении книги:', err);
          alert('Ошибка при добавлении книги.');
        }
      });
    } else {
      alert('Пожалуйста, выберите автора.');
    }
  }
}
