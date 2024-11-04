using LibraryWebApp.BookService.Application.Interfaces;
using LibraryWebApp.BookService.Domain.Interfaces;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class NotifyCheckoutDateUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public NotifyCheckoutDateUseCase(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public void Execute(int id)
        {
            var book = _unitOfWork.Books.Get(b => b.Id == id);
            if (book != null && book.ReturnDateTime <= DateTime.Now)
            {
                var user = _unitOfWork.Users.Get(u => u.Id == book.UserId);
                if (user != null)
                {
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        throw new ArgumentNullException(nameof(user.Email), "User email must be provided.");
                    }

                    var subject = "Напоминание по возвращению книги!";
                    var body = $"Дорогай {user.Username},<br/><br/>" +
                               $"Это напоминание, что книгу '{book.Title}' Вы должны были вернуть {book.ReturnDateTime.ToString()}.<br/><br/>" +
                               "Пожалуйста, верните книгу, если не хотите проблем.<br/><br/>" +
                               "Спасибо вам :>";
                    _emailService.SendEmail(user.Email, subject, body);
                }
            }
        }
    }
}
