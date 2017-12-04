using Microsoft.EntityFrameworkCore;
using ShareDrive.Data;
using ShareDrive.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShareDrive.Common
{
    public class DbRepository<T> : IDbRepository<T> where T : class, IEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;

        public DbRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = this.context.Set<T>();
        }

        public T Create(T model)
        {
            T entity = this.entities.Add(model).Entity;
            this.Save();

            return entity;
        }

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            return this.entities.ToList();
        }

        public T GetById(int id)
        {
            return this.entities.FirstOrDefault(x => x.Id == id);
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        private void Save()
        {
            this.context.SaveChanges();
        }
    }
}
