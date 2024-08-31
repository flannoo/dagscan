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

        backgroundJobClient.Schedule<UpdateHypergraphValidatorNodeLocationJob>("ip-lookup", job =>
            job.Execute(notification.HypergraphValidatorNodeId), TimeSpan.FromSeconds(30));

        return Task.CompletedTask;
    }
}
