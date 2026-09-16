using ClimaPulse.Models;

namespace ClimaPulse.Repositories;

public class AnomalyLogRepository : IRepository<AnomalyLog>
{
    private readonly string _connectionString;

    public AnomalyLogRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Add(AnomalyLog item) => throw new NotImplementedException();
    public void Remove(AnomalyLog item) => throw new NotImplementedException();
    public List<AnomalyLog> GetAll() => throw new NotImplementedException();
    public List<AnomalyLog> Query(Func<AnomalyLog, bool> filter) => throw new NotImplementedException();

    public void Acknowledge(int logId) => throw new NotImplementedException();
    public void ExportCsv() => throw new NotImplementedException();
    public void ExportJson() => throw new NotImplementedException();
}
