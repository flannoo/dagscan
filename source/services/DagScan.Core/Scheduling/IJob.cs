namespace DagScan.Core.Scheduling;

public interface IJob
{
    string Schedule { get; }
    Task Execute();
}
