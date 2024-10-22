import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { BookDTO } from '../models/book-dto.model';
import { BookService } from '../services/book.service';
import { BookGenre } from '../enums/book-genre.enum';

@Component({
  selector: 'app-edit-book-modal',
  templateUrl: './edit-book-modal.component.html',
  styleUrls: ['./edit-book-modal.component.css']
})
export class EditBookModalComponent implements OnInit {
  book!: BookDTO;
  selectedImageFile: File | null = null;

  genres = Object.keys(BookGenre).filter(key => isNaN(Number(key))).map(key => ({
    id: BookGenre[key as keyof typeof BookGenre],
    name: key
  }));

  constructor(
    public modal: NgbActiveModal,
    private bookService: BookService
  ) { }

  ngOnInit(): void { }

  getGenreName(genre: BookGenre): string {
    return BookGenre[genre];
  }

  onImageSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedImageFile = input.files[0];
    }
  }

  save(): void {
    this.book.genre = +this.book.genre;

    this.bookService.updateBook(this.book.id!, this.book).subscribe({
      next: () => {
        if (this.selectedImageFile) {
          this.bookService.addBookImage(this.book.id!, this.selectedImageFile).subscribe({
            next: () => {
              alert('Изображение успешно загружено и книга обновлена.');
              this.modal.close(this.book);
            },
            error: (err) => {
              console.error('Ошибка при загрузке изображения:', err);
              alert('Ошибка при загрузке изображения.');
            }
          });
        } else {
          alert('Книга успешно обновлена.');
          this.modal.close(this.book);
        }
      },
      error: (err) => {
        console.error('Ошибка при обновлении книги:', err);
        alert('Ошибка при обновлении книги.');
      }
    });
  }

}
