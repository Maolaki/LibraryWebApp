using LibraryWebApp.BookService.Application.Interfaces;
using LibraryWebApp.BookService.Domain.Interfaces;
using Quartz;

namespace LibraryWebApp.BookService.Application.Entities
{
    public class BookReminderJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public BookReminderJob(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var overdueBooks = _unitOfWork.Books.GetAll()
                .Where(b => b.ReturnDateTime < DateTime.Now)
                .ToList();

            foreach (var book in overdueBooks)
            {
                var user = _unitOfWork.Users.Get(u => u.Id == book.UserId);
                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    var subject = "Книжный долг!";
                    var body = $"Дорогай {user.Username},<br/><br/>" +
                               $"Это напоминание, что книгу '{book.Title}' Вы должны были вернуть {book.ReturnDateTime.ToString()}.<br/><br/>" +
                               "Пожалуйста, верните книгу, если не хотите проблем.<br/><br/>" +
                               "Спасибо вам :>";
                    await _emailService.SendEmailAsync(user.Email, subject, body);
                }
            }
        }
    }
}
