using DagScan.Application.Domain;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.UpdateValidatorNodeLocations;

public sealed class HypergraphNodeAddedNotificationHandler(
    IBackgroundJobClient backgroundJobClient,
    ILogger<HypergraphNodeAddedNotificationHandler> logger)
    : INotificationHandler<HypergraphValidatorNodeAdded>
{
    public Task Handle(HypergraphValidatorNodeAdded notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "HypergraphNode with ID {HypergraphValidatorNodeId} added, enriching location information",
            notification.HypergraphValidatorNodeId.Value);

        backgroundJobClient.Enqueue<UpdateHypergraphValidatorNodeLocationJob>("ip-lookup", job =>
            job.Execute(notification.HypergraphId, notification.HypergraphValidatorNodeId));

        return Task.CompletedTask;
    }
}
