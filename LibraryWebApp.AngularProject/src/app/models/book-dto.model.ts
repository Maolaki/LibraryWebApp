import { BookGenre } from '../enums/book-genre.enum';

export interface BookDTO {
  isbn: string | null;
  title: string | null;
  description: string | null;
  genre: BookGenre;
  authorId: number;
  userId?: number; 
  checkoutDateTime?: Date;
  returnDateTime?: Date;
}
