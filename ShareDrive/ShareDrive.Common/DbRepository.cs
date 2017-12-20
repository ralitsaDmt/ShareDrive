using Microsoft.EntityFrameworkCore;
using ShareDrive.Data;
using ShareDrive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<T> CreateAsync(T model)
        {
            T entity = (await this.entities.AddAsync(model)).Entity;
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

        public IQueryable<T> GetByIdQueryable(int id)
        {
            return this.entities.Where(x => x.Id == id);
        }

        public Task<T> UpdateAsync(T entity)
        {
            this.entities.Attach(entity);
            var entry = this.context.Entry(entity);
            entry.State = EntityState.Modified;
            this.Save();

            return Task.FromResult(entity);
        }

        private void Save()
        {
            this.context.SaveChanges();
        }
    }
}
