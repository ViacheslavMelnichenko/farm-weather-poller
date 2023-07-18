// <copyright file="IEventPublisher.cs" company="EmergeTech LLC">
//   Copyright (c) EmergeTech LLC. All rights reserved.
//   Reproduction or transmission in whole or in part, in any form or
//   by any means, electronic, mechanical or otherwise, is prohibited
//   without the prior written consent of the copyright owner.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using Farm.Weather.Contracts.IntegrationEvents;

namespace Farm.Weather.Poller.Services;

public interface IEventPublisher
{
    Task SendIntegrationCommandAsync(IWeatherRawDataCreated[] integrationEvents,
                                     CancellationToken cancellationToken = default);
}