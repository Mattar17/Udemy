using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;
using Udemy.Repository.Context;

namespace Udemy.Repository.Repositories
{
    public class CourseRepository : GenericRepository<Course>,ICourseRepository 
    {
        public CourseRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            dbContext.Coureses.Include(c => c.Category);
            dbContext.Coureses.Include(c=>c.Instructor);
        }
    }
}
