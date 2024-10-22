import { BookGenre } from '../enums/book-genre.enum';

export interface Book {
  id: number;
  isbn: string | null;
  title: string | null;
  description: string | null;
  genre: BookGenre;
  authorId: number;
  userId?: number; 
  checkoutDateTime?: Date; 
  returnDateTime?: Date; 
  image?: string | null; 
  imageContentType?: string | null;
}
