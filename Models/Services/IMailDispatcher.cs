using System.Net;
using System.Net.Mail;

namespace MicrosoftAssignment2.Models.Services
{
    public interface IMailDispatcher
    {
        Task DispatchMailAsync(string recipient, string topic, string htmlContent);
    }

    public class MailDispatcher : IMailDispatcher
    {
        private readonly SmtpClient _emailClient;
        private readonly string _senderAddress;

        public MailDispatcher(IConfiguration config)
        {
            _senderAddress = config["MailSettings:SenderAddress"];
            _emailClient = new SmtpClient(config["MailSettings:MailServer"])
            {
                Port = int.Parse(config["MailSettings:ServerPort"]),
                Credentials = new NetworkCredential(
                    _senderAddress,
                    config["MailSettings:SenderPassword"]),
                EnableSsl = true,
            };
        }

        public async Task DispatchMailAsync(string recipient, string topic, string htmlContent)
        {
            var email = new MailMessage()
            {
                From = new MailAddress(_senderAddress),
                Subject = topic,
                Body = htmlContent,
                IsBodyHtml = true
            };
            email.To.Add(recipient);

            await _emailClient.SendMailAsync(email);
        }
    }
}