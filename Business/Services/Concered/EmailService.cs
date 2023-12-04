using Business.Helpers;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Business.Services.Abtraction;

namespace Business.Services.Concered
{
    public class EmailService :IEmailService
    {
        private readonly EmailSettings emailSettings;
        public EmailService(IOptions<EmailSettings> options)
        {
            this.emailSettings = options.Value;
        }

        public async Task SendEmailAsync(Mailrequest mailrequest,string email)
        {
            var Email = new MimeMessage();
            Email.Sender = MailboxAddress.Parse(emailSettings.Email);
            Email.To.Add(MailboxAddress.Parse(email));
            Email.Subject = mailrequest.Subject;
            var builder = new BodyBuilder();


            //byte[] fileBytes;
            //if (System.IO.File.Exists("Attachment/dummy.pdf"))
            //{
            //    FileStream file = new FileStream("Attachment/dummy.pdf", FileMode.Open, FileAccess.Read);
            //    using (var ms = new MemoryStream())
            //    {
            //        file.CopyTo(ms);
            //        fileBytes = ms.ToArray();
            //    }
            //    builder.Attachments.Add("attachment.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
            //    builder.Attachments.Add("attachment2.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
            //}

            builder.HtmlBody = mailrequest.Body;
            Email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(Email);
            smtp.Disconnect(true);
        }
    }

   

}
