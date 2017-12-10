using ShareDrive.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShareDrive.Common
{
    public interface IDbRepository<T> where T : class, IEntity
    {
        IQueryable<T> GetAll();

        T GetById(int id);

        IQueryable<T> GetByIdQueryable(int id);

        T Create(T entity);

        bool Update(T entity);

        bool Delete(T entity);
    }
}
