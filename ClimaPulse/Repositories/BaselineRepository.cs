using ClimaPulse.Models;

namespace ClimaPulse.Repositories;

public class BaselineRepository : IRepository<ClimateBaseline>
{
    private readonly string _connectionString;

    public BaselineRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Add(ClimateBaseline item) => throw new NotImplementedException();
    public void Remove(ClimateBaseline item) => throw new NotImplementedException();
    public List<ClimateBaseline> GetAll() => throw new NotImplementedException();
    public List<ClimateBaseline> Query(Func<ClimateBaseline, bool> filter) => throw new NotImplementedException();

    public ClimateBaseline GetByCityMonth(int cityId, int month)
        => throw new NotImplementedException();
}
