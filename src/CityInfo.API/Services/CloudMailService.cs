﻿using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private string _mailFrom = "noreply@mycompany.com";
        private string _mailTo = "admin@mycompany.com";

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {this.GetType().Name}");
            Debug.WriteLine($"Subject {subject}");
            Debug.WriteLine($"message {message}");
        }
    }
}