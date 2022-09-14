using TOTP_BugTracker.Models;

namespace TOTP_BugTracker.Services.Interfaces
{
    public interface INotificationService
    {
        public Task AddNotificationAsync(Notification notification);
        public Task<bool> SendEmailNotificationAsync(Notification notiication, string emailSubject);
    }
}
