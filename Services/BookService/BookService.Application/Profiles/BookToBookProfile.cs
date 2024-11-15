using LibraryWebApp.BookService.Domain.Entities;
using AutoMapper;

namespace BookService.Application.Profiles
{
    public class BookToBookProfile : Profile
    {
        public BookToBookProfile() 
        {
            CreateMap<Book, Book>();
        }
    }
}
