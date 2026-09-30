namespace ClimaPulse.Repositories;

public interface IRepository<T>
{
    void Add(T item);
    void Remove(T item);
    List<T> GetAll();
    List<T> Query(Func<T, bool> filter);
}
