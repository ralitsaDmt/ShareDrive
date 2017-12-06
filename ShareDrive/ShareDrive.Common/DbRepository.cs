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
            try
            {
                this.entities.Remove(entity);
                this.Save();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public IQueryable<T> GetAll()
        {
            return this.entities;
        }

        public T GetById(int id)
        {
            return this.entities.FirstOrDefault(x => x.Id == id);
        }

        public bool Update(T entity)
        {
            try
            {
                this.entities.Attach(entity);
                var entry = this.context.Entry(entity);
                entry.State = EntityState.Modified;

                this.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void Save()
        {
            this.context.SaveChanges();
        }
    }
}
