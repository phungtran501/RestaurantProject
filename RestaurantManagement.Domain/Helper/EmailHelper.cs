using Microsoft.Extensions.Options;
using RestaurantManagement.Domain.Configuration;
using RestaurantManagement.Domain.Model;
using System.Net;
using System.Net.Mail;

namespace RestaurantManagement.Domain.Helper
{
    public class EmailHelper : IEmailHelper
    {
        EmailConfig _emailConfig;

        public EmailHelper(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmail(EmailRequest emailRequest)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(_emailConfig.Provider, _emailConfig.Port);
                smtpClient.Credentials = new NetworkCredential(_emailConfig.DefaultSender, _emailConfig.Password);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;

                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(emailRequest.EmailCustomer);
                mailMessage.To.Add(_emailConfig.DefaultSender);
                mailMessage.Subject = emailRequest.Subject;
                mailMessage.Body = emailRequest.Content;
                

                if (emailRequest.AttachmentFilePaths.Length > 0)
                {
                    foreach (var path in emailRequest.AttachmentFilePaths)
                    {
                        Attachment attachment = new Attachment(path);
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                await smtpClient.SendMailAsync(mailMessage);

                mailMessage.Dispose();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
