import { Country } from '../enums/country.enum';

export interface AuthorDTO {
  firstName: string | null;
  lastName: string | null;
  dateOfBirth: string; 
  country: Country;
}
