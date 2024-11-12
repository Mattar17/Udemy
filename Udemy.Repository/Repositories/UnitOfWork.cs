using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Contracts;
using Udemy.Repository.Context;

namespace Udemy.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public ICourseRepository CourseRepository { get; }

        public UnitOfWork(ApplicationDbContext dbContext,ICourseRepository courseRepository)
        {
            _dbContext = dbContext;
            CourseRepository = courseRepository;
        }
        public async Task<int>  CompleteAsync()
            => await _dbContext.SaveChangesAsync();

        public void Dispose()
            => _dbContext.Dispose();
    }
}
