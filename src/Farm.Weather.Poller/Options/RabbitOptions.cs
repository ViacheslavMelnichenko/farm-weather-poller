﻿namespace Farm.Weather.Poller.Options
{
    public class RabbitOptions
    {
        public const string RabbitMq = "RabbitMqHost";

        public string Url { get; set; } = string.Empty;
        public string VirtualHost { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}