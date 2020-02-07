namespace Shoplify.Services.EmailSender
{
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    public class EmailSender : IEmailSender
    {
        private string emailAdress;
        private string password;
        private string username;

        public IConfiguration Configuration { get; }

        public EmailSender([FromServices] IConfiguration config)
        {
            Configuration = config;

            emailAdress = Configuration.GetValue<string>("EmailSender:EmailAddress");
            password = Configuration.GetValue<string>("EmailSender:Password");
            username = Configuration.GetValue<string>("EmailSender:Username");
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var toAddress = new MailAddress(email);
            var fromAddress = new MailAddress(emailAdress, username);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, password),
                Timeout = 20000
            };

            using (var mailMessage = new MailMessage(fromAddress, toAddress))
            {
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                await smtp.SendMailAsync(mailMessage);
            }
        }
    }
}
