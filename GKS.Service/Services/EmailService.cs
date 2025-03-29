﻿using GKS.Core.Entities;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using GKS.Core.IServices;


namespace GKS.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task<bool> SendEmailAsync(EmailRequest request)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("GKS", configuration["GOOGLE_USER_EMAIL"]));
            emailMessage.To.Add(new MailboxAddress(request.To, request.To));
            emailMessage.Subject = request.Subject;

            var bodyBuilder = new BodyBuilder { TextBody = request.Body };
            emailMessage.Body = bodyBuilder.ToMessageBody();



            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(configuration["SMTP_SERVER"], int.Parse(configuration["SMTP_PORT"]), SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(configuration["GOOGLE_USER_EMAIL"], configuration["PASSWORD"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
    
}
