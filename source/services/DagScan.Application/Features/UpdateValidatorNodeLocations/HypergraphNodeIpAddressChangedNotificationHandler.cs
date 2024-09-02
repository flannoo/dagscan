using DagScan.Application.Domain;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.UpdateValidatorNodeLocations;

public sealed class HypergraphNodeIpAddressChangedNotificationHandler(
    IBackgroundJobClient backgroundJobClient,
    ILogger<HypergraphNodeIpAddressChangedNotificationHandler> logger)
    : INotificationHandler<HypergraphValidatorNodeIpAddressChanged>
{
    public Task Handle(HypergraphValidatorNodeIpAddressChanged notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "HypergraphNode with ID {HypergraphValidatorNodeId} IP Address changed, enriching location information",
            notification.HypergraphValidatorNodeId.Value);

        backgroundJobClient.Enqueue<UpdateHypergraphValidatorNodeLocationJob>("ip-lookup", job =>
            job.Execute(notification.HypergraphValidatorNodeId));

        return Task.CompletedTask;
    }
}
