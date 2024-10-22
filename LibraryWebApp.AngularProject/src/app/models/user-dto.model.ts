import { UserRole } from '../enums/user-role.enum';

export interface UserDTO {
  username: string | null;
  email: string | null;
  password: string | null;
  role: UserRole;
}
