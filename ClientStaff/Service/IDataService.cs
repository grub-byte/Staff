using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientStaff.Service
{
    internal interface IDataService<T> where T : class
    {
        IAsyncEnumerable<T> Get(); // получение всех объектов
        Task<T?> Create(T item); // создание объекта
        Task<T?> Update(T item); // обновление объекта
        Task<T?> Delete(T item); // удаление объекта по id
    }
}
