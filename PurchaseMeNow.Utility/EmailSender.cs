using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseMeNow.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions emailOptions;

        public EmailSender(IOptions<EmailOptions> options)
        {
            emailOptions = options.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(emailOptions.SenderName, emailOptions.Sender));
                mimeMessage.To.Add(new MailboxAddress(email));
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart("html")
                {
                    Text = htmlMessage
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(emailOptions.MailServer, emailOptions.MailPort, false);

                    await client.AuthenticateAsync(emailOptions.Sender, emailOptions.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);

                }

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
