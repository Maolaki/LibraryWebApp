import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { RegisterComponent } from './components/register.component';
import { LoginComponent } from './components/login.component';
import { BooksComponent } from './components/books.component';
import { BookDetailComponent } from './components/book-detail.component';
import { AdminBookListComponent } from './components/admin-book-list.component';
import { EditBookModalComponent } from './components/edit-book-modal.component';
import { AddBookModalComponent } from './components/add-book-modal.component';
import { MyBooksComponent } from './components/my-books.component';


@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    BooksComponent,
    BookDetailComponent,
    AdminBookListComponent,
    EditBookModalComponent,
    AddBookModalComponent,
    MyBooksComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,  
    HttpClientModule,
    AppRoutingModule,
    NgxPaginationModule,
    FormsModule,
    NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
