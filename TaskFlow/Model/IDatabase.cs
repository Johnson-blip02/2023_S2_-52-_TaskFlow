namespace TaskFlow.Model;

public interface IDatabase<T>
{
    List<T> GetData();
    void Insert(T data);
    void InsertAll(List<T> data);
    void Delete(T data);
}
