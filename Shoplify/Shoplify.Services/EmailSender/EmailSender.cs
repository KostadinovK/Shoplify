namespace Shoplify.Services.EmailSender
{
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;

    public class EmailSender : IEmailSender
    {
        private const string EmailAdress = "shoplify.manager@gmail.com";
        private const string Password = "Hrosimi123";
        private const string Username = "Kostadin Kostadinov";

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var toAddress = new MailAddress(email);
            var fromAddress = new MailAddress(EmailAdress, Username);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, Password),
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
