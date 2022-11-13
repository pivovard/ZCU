using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using Bank.Models;

namespace Bank.Handlers
{
    /// <summary>
    /// Client for mail comunication
    /// </summary>
    public class MailClient
    {
        public static MailClient Singleton { get; set; }
        
        public string Host = "smtp.gmail.com";
        public int Port = 587;
        public string Credentials = "jip.meditab@gmail.com";
        public string Pass = "fnjip2ik";
        public string From = "hellsbank@666.com";

        private  SmtpClient client;

        /// <summary>
        /// Creates singleton with default settings
        /// </summary>
        public MailClient()
        {
            Singleton = this;
            Singleton.MailClientSetup();
            
        }

        /// <summary>
        /// Creates singleton with given settings
        /// </summary>
        public MailClient(string path)
        {
            Singleton = GetCongfig(path);
            Singleton.MailClientSetup();
            
        }

        /// <summary>
        /// Get configuration from path
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>Mail client</returns>
        private MailClient GetCongfig(string path)
        {
            var serializer = new XmlSerializer(typeof(MailClient));
            if (File.Exists(path)) return serializer.Deserialize(XmlReader.Create(path)) as MailClient;
            else return this;
        }

        /// <summary>
        /// Setup mail client
        /// </summary>
        public void MailClientSetup()
        {

            client = new SmtpClient(Host, Port);
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(Credentials, Pass);
        }

        /// <summary>
        /// Send authorization email
        /// </summary>
        /// <param name="user">Email to user</param>
        /// <param name="code">Verification code</param>
        public void SendAuth(User user, string code)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(this.From);
            msg.To.Add(new MailAddress(user.Email));

            msg.Subject = "Login";
            msg.Body = $"Verification code for login request is {code}";

            client.Send(msg);
        }

        /// <summary>
        /// Send verification email for payment
        /// </summary>
        /// <param name="user">Email to user</param>
        /// <param name="pay">Payment</param>
        /// <param name="code">Confirmation code</param>
        public void SendPayment(User user, Payment pay, string code)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(this.From);
            msg.To.Add(new MailAddress(user.Email));

            msg.Subject = "Payment";
            msg.Body = $"Payment to account ";
            if(pay.DestAccountPrefix != null) msg.Body += $"{pay.DestAccountPrefix}-";
            msg.Body += $"{pay.DestAccount}/{pay.DestBank}, amount {pay.Amount}.\n";
            msg.Body += $"Confirmation code is {code}";

            client.Send(msg);
        }

        /// <summary>
        /// Send info to new user with pin
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="pin">Pin in no hash form</param>
        public void SendAddUser(User user, int pin)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(this.From);
            msg.To.Add(new MailAddress(user.Email));

            msg.Subject = "You have been registered in Hell's bank";
            msg.Body = $"Hello,\n\n" +
                $"you have been registered in Hell's bank.\n\n" +
                $"Your credentials are:\n" +
                $"Login: {user.Login}\n" +
                $"Pin: {pin}\n\n" +
                $"Regards,\n" +
                $"Admin team";

            client.Send(msg);
        }

        /// <summary>
        /// Send info abou edit
        /// </summary>
        /// <param name="user">User</param>
        public void SendEditUser(User user)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(this.From);
            msg.To.Add(new MailAddress(user.Email));

            msg.Subject = "Account edit";
            msg.Body = $"Hello,\n\n" +
                $"your account has been changed.\n\n" +
                $"Regards,\n" +
                $"Admin team";

            client.Send(msg);
        }

        /// <summary>
        /// Send info abou removing user
        /// </summary>
        /// <param name="user">User</param>
        public void SendDeleteUser(User user)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(this.From);
            msg.To.Add(new MailAddress(user.Email));

            msg.Subject = "Account deleted";
            msg.Body = $"Hello,\n\n" +
                $"your account has been deleted.\n\n" +
                $"Regards,\n" +
                $"Admin team";

            client.Send(msg);
        }
    }
}
