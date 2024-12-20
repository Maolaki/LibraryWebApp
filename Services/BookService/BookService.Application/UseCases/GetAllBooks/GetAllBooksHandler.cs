﻿using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Exceptions;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetAllBooksHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<Book>>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;

        public GetAllBooksHandler(IUnitOfWork<ImageDTO> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Book>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var uniqueBooks = await _unitOfWork.Books.GetAllAsync(request.PageNumber, request.PageSize);

            if (!uniqueBooks.Any())
            {
                throw new NotFoundException("Books not found.");
            }

            return await Task.FromResult(uniqueBooks);
        }
    }
}
