# farm-weather-poller
Weather poller.

![main](https://github.com/ViacheslavMelnichenko/farm-weather-poller/actions/workflows/main.yml/badge.svg?branch=main)

## Features

- Poll weather data from [WeatherApi](https://www.weatherapi.com)
- Send it to specific rabbitMQ exchange via MassTransit

## Tech

Weather poller uses a number of open source projects to work properly:

- [Asp.Net Core](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0) - is a cross-platform, high-performance, open-source framework for building modern, cloud-enabled, Internet-connected apps.
- [MassTransit](https://masstransit-project.com) - a free, open-source distributed application framework for .NET.
- [RabbitMQ](https://www.rabbitmq.com) - is the most widely deployed open source message broker.
- [Seq](https://datalust.co/seq) - is the intelligent search, analysis, and alerting server built specifically for modern structured log data.
- [Vault](https://www.vaultproject.io) - is an identity-based secret and encryption management system.
- [Docker](https://www.docker.com) - is a set of platform as a service (PaaS) products that use OS-level virtualization to deliver software in packages called containers.
- etc...

## Future plans

Migrate to helm with kubernetes

## License

MIT

**Free Software, Hell Yeah!**
