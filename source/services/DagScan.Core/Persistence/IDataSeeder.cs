namespace DagScan.Core.Persistence;

public interface IDataSeeder
{
    Task SeedAsync();
    int Order { get; }
}

public interface IRequiredDataSeeder : IDataSeeder
{
}
