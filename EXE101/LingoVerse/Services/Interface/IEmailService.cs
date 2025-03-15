using LingoVerse.Models;

namespace LingoVerse.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
