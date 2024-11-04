namespace LibraryWebApp.BookService.Domain.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string email, string subject, string body);
        Task SendEmailAsync(string to, string subject, string body);
    }
}
