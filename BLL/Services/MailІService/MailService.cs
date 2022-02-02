using System.IO;
using System.Threading.Tasks;
using BLL.Settings;
using DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BLL.Services
{
    public class MailService : IMailService
    {
      private readonly MailSettings mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailDto mailRequest)
        {
            var email = new MimeMessage {Sender = MailboxAddress.Parse(mailSettings.Mail)};
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();

            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes = null;

                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        await using var ms = new MemoryStream();
                        await file.CopyToAsync(ms);
                        fileBytes = ms.ToArray();
                    }
                    builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(mailSettings.Mail, mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendInfoAboutStartStudyEmailAsync(StartStudyMailDto request)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\template_start_study.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText.Replace("[Name]", request.Name).Replace("[courseName]", request.CourseName).Replace("[dayToStart]", $"{request.DayToStart}");
            var email = new MimeMessage {Sender = MailboxAddress.Parse(mailSettings.Mail)};
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Welcome {request.Name}";
            var builder = new BodyBuilder {HtmlBody = MailText};
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(mailSettings.Mail, mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendWelcomeEmailAsync(WelcomeMailDto request)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\index.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = await str.ReadToEndAsync();
            str.Close();
            MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail).Replace("{ConfirmationLink}", $"{request.Link}");
            var email = new MimeMessage {Sender = MailboxAddress.Parse(mailSettings.Mail)};
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = $"Welcome {request.UserName}";
            var builder = new BodyBuilder {HtmlBody = MailText};
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(mailSettings.Mail, mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }  
    }
}