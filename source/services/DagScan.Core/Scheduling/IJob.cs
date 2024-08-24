namespace DagScan.Core.Scheduling;

public interface IJob
{
    string Schedule { get; }
    Task Execute();
}

public interface ISeedJob
{
    int Order { get; }
    string Schedule { get; }
    Task Execute();
}
