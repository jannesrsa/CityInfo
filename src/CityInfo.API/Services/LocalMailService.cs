using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailFrom = Startup.Configuration["mailSettings:mailToAddress"];
        private string _mailTo = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {this.GetType().Name}");
            Debug.WriteLine($"Subject {subject}");
            Debug.WriteLine($"message {message}");
        }
    }
}