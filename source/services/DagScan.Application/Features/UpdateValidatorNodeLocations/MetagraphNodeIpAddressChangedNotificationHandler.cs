using DagScan.Application.Domain;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DagScan.Application.Features.UpdateValidatorNodeLocations;

public sealed class MetagraphNodeIpAddressChangedNotificationHandler(
    IBackgroundJobClient backgroundJobClient,
    ILogger<MetagraphNodeIpAddressChangedNotificationHandler> logger)
    : INotificationHandler<MetagraphValidatorNodeIpAddressChanged>
{
    public Task Handle(MetagraphValidatorNodeIpAddressChanged notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "MetagraphNode with ID {MetagraphValidatorNodeId} IP Address changed, enriching location information",
            notification.MetagraphValidatorNodeId.Value);

        backgroundJobClient.Enqueue<UpdateMetagraphValidatorNodeLocationJob>("ip-lookup", job =>
            job.Execute(notification.MetagraphValidatorNodeId));

        return Task.CompletedTask;
    }
}
