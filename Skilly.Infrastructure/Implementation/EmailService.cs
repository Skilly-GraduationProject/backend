﻿using Skilly.Infrastructure.Abstract;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace Skilly.Infrastructure.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
        }

        public async Task SendEmailAsync(string email, string userName, string subject, string verificationCode)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                    smtpClient.EnableSsl = true;

                    string imageUrl = "https://i.ibb.co/jkFTcHdK/photo-2025-06-01-22-44-09.jpg";

                    string htmlMessage = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #ffffff;
                                margin: 0;
                                padding: 20px;
                            }}
                            .container {{
                                background-color: #ffffff;
                                border-radius: 8px;
                                padding: 20px;
                                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                            }}
                            .header {{
                                text-align: center;
                                padding-bottom: 20px;
                            }}
                            .verification-code {{
                                font-size: 36px;
                                font-weight: bold;
                                color: #000000;
                                margin: 20px 0;
                                text-align: center;
                            }}
                            .footer {{
                                text-align: center;
                                margin-top: 20px;
                                font-size: 14px;
                                color: #888;
                            }}
                            .message {{
                                color: black;
                                background-color: #ffffff;
                                padding: 20px;
                                border-radius: 5px;
                                font-size: 20px;
                                text-align: center;
                            }}
                            code {{
                                display: inline-block;
                                background-color: #f0f0f0;
                                padding: 5px;
                                border-radius: 5px;
                                margin: 20px 0;
                                font-size: 18px;
                                color: black;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h2 style='color: #000000;'>Email Verification</h2>
                            </div>
                            <img src='{imageUrl}' alt='Verification Icon' style='display: block; margin: 0 auto; max-width: 100px;' />
                            <div class='message'>
                                <p>Dear {userName},</p>
                                <p>Thank you for registering with us. Please verify your email address using the code below:</p>
                                <code>{verificationCode}</code>
                                <p>If you did not request this, please ignore this message.</p>
                                <p>Best Regards,<br>Skilly Support Team</p>
                            </div>
                            <div class='footer'>
                                <p>&copy; {DateTime.Now.Year} Skilly. All rights reserved.</p>
                            </div>
                        </div>
                    </body>
                    </html>";

                    string plainTextMessage = $"Hello {userName},\n\nYour verification code is: {verificationCode}\n\nIf you didn’t request this, just ignore this message.\n\nThanks,\nSkilly Support Team";

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpUser, "Skilly Support"),
                        Subject = subject,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(email);

                    // Add both plain text and HTML views
                    mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(plainTextMessage, null, "text/plain"));
                    mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlMessage, null, "text/html"));

                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
