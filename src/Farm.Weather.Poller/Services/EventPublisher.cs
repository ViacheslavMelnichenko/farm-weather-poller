// <copyright file="EventPublisher.cs" company="EmergeTech LLC">
// Copyright (c) EmergeTech LLC. All rights reserved.
// Reproduction or transmission in whole or in part, in any form or
// by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using Farm.Weather.Contracts.IntegrationEvents;
using Farm.Weather.Poller.Options;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Farm.Weather.Poller.Services;

public class EventPublisher : IEventPublisher
{
    private readonly Uri _queue;
    private readonly IBus _bus;

    public EventPublisher(IBus bus, IOptions<CommandBusOptions> options)
    {
        _bus = bus;
        _queue = new Uri($"queue:{options.Value.WeatherDataQueue}");
    }

    public async Task SendIntegrationCommandAsync(
        IWeatherRawDataCreated[] integrationEvents,
        CancellationToken cancellationToken)
    {
        var endpoint = await _bus.GetSendEndpoint(_queue);
        await endpoint.SendBatch(integrationEvents, cancellationToken);
    }
}