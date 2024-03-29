﻿namespace PromoWeb.Services.EmailSender
{
    public class EmailSettings
    {
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Ssl { get; set; }
    }
}
