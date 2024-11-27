using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Domain.Interfaces;
using Quartz;

namespace LibraryWebApp.BookService.Application.Entities
{
    public class BookReminderJob : IJob
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;
        private readonly IEmailService _emailService;

        public BookReminderJob(IUnitOfWork<ImageDTO> unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var overdueBooks = await _unitOfWork.Books.GetAllAsync(1, int.MaxValue);

            var filteredBooks = overdueBooks
                .Where(b => b.ReturnDateTime < DateTime.Now)
                .ToList();

            foreach (var book in overdueBooks)
            {
                var user = await _unitOfWork.Users.GetAsync(u => u.Id == book.UserId);
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
