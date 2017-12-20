using ShareDrive.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareDrive.Common
{
    public interface IDbRepository<T> where T : class, IEntity
    {
        IQueryable<T> GetAll();

        T GetById(int id);

        IQueryable<T> GetByIdQueryable(int id);

        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity);

        bool Delete(T entity);
    }
}
