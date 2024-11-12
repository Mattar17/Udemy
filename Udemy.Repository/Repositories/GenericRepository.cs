﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Contracts;
using Udemy.Repository.Context;

namespace Udemy.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(T item)
            => await _dbContext.AddAsync(item);

        public void Delete(T item)
            => _dbContext.Remove(item);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbContext.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(T id)
            => await _dbContext.Set<T>().FindAsync(id);

        public void Update(T item)
            => _dbContext.Update(item);
    }
}
