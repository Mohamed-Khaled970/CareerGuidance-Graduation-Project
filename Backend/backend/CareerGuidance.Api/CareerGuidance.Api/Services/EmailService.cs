

namespace CareerGuidance.Api.Services
{
    public class EmailService(IOptions<MailSettings> mailSettings) : IEmailSender
    {
        private readonly MailSettings _mailSettings = mailSettings.Value;
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var messeage = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail), // الايميل اللي هبعت منه
                Subject = subject
            };

            messeage.To.Add(MailboxAddress.Parse(email)); //  الايميل اللي انا هبعتله

            // هنا هنعمل الرسالة ذات نفسها

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            messeage.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();  // we will use SmtpClient that is exist in MailKit
     
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls); // Configuration to connect to the email
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password); // email and password to connect

            await smtp.SendAsync(messeage);

            smtp.Disconnect(true);
        }
    }
}
