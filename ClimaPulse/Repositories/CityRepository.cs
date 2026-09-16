using ClimaPulse.Models;

namespace ClimaPulse.Repositories;

public class CityRepository : IRepository<City>
{
    private readonly string _connectionString;

    public CityRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Add(City item) => throw new NotImplementedException();
    public void Remove(City item) => throw new NotImplementedException();
    public List<City> GetAll() => throw new NotImplementedException();
    public List<City> Query(Func<City, bool> filter) => throw new NotImplementedException();
}
