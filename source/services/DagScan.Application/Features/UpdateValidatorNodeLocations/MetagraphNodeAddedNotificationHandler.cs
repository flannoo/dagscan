using DagScan.Application.Domain;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.UpdateValidatorNodeLocations;

public sealed class MetagraphNodeAddedNotificationHandler(
    IBackgroundJobClient backgroundJobClient,
    ILogger<MetagraphNodeAddedNotificationHandler> logger)
    : INotificationHandler<MetagraphValidatorNodeAdded>
{
    public Task Handle(MetagraphValidatorNodeAdded notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "MetagraphNode with ID {MetagraphValidatorNodeId} added, enriching location information",
            notification.MetagraphValidatorNodeId.Value);

        backgroundJobClient.Schedule<UpdateMetagraphValidatorNodeLocationJob>("ip-lookup", job =>
            job.Execute(notification.MetagraphValidatorNodeId), TimeSpan.FromSeconds(30));

        return Task.CompletedTask;
    }
}
