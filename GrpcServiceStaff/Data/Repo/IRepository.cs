namespace GrpcServiceStaff.Data.Repo
{
    public interface IRepository<T> : IDisposable where T : class, new()
    {
        Task<IQueryable<T>> Get(); // получение всех объектов
        Task<T> Create(T item); // создание объекта
        Task<T?> Update(T item); // обновление объекта
        Task<T?> Delete(T item); // удаление объекта по id
    }
}
