using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class NotifyCheckoutDateHandler : IRequestHandler<NotifyCheckoutDateCommand, Unit>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;
        private readonly IEmailService _emailService;

        public NotifyCheckoutDateHandler(IUnitOfWork<ImageDTO> unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(NotifyCheckoutDateCommand request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetAsync(b => b.Id == request.BookId);
            if (book != null && book.ReturnDateTime <= DateTime.Now)
            {
                var user =  await _unitOfWork.Users.GetAsync(u => u.Id == book.UserId);
                if (user != null)
                {
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        throw new ArgumentNullException(nameof(user.Email), "User email must be provided.");
                    }

                    var subject = "Напоминание по возвращению книги!";
                    var body = $"Дорогой {user.Username},<br/><br/>" +
                               $"Это напоминание, что книгу '{book.Title}' Вы должны были вернуть {book.ReturnDateTime.ToString()}.<br/><br/>" +
                               "Пожалуйста, верните книгу, если не хотите проблем.<br/><br/>" +
                               "Спасибо вам :>";
                    _emailService.SendEmail(user.Email, subject, body);
                }
            }

            return await Task.FromResult(Unit.Value);
        }
    }
}
