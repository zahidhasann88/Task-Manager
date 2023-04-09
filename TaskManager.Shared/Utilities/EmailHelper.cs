using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Shared.Utilities
{
    public static class EmailHelper
    {
        public static void SendEmail(string to, string subject, string body)
        {
            var fromAddress = new MailAddress("your_email@example.com", "Your Name");
            var toAddress = new MailAddress(to);
            const string fromPassword = "your_password";
            const string smtpServer = "smtp.example.com";
            const int smtpPort = 587;
            var smtp = new SmtpClient
            {
                Host = smtpServer,
                Port = smtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }

}
