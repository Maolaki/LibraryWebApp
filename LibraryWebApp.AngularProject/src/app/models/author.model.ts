import { Country } from '../enums/country.enum';
import { Book } from './book.model';

export interface Author {
  id: number;
  firstName: string | null;
  lastName: string | null;
  dateOfBirth: string; 
  country: Country; 
  books?: Book[]; 
}
