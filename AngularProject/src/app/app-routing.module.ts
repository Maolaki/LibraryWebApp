import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './components/register.component';
import { LoginComponent } from './components/login.component';
import { BooksComponent } from './components/books.component';
import { BookDetailComponent } from './components/book-detail.component';
import { AdminBookListComponent } from './components/admin-book-list.component';
import { MyBooksComponent } from './components/my-books.component';

const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'my-books', component: MyBooksComponent },
  { path: 'books', component: BooksComponent },
  { path: 'books/:isbn', component: BookDetailComponent },
  { path: 'admin', component: AdminBookListComponent },
  { path: '', redirectTo: '/books', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
