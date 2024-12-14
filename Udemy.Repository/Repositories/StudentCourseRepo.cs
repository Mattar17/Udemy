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
    public class StudentCourseRepo : GenericRepository<Student_Course>,IStudentCourseRepo
    {
        public StudentCourseRepo(ApplicationDbContext dbContext):base(dbContext)
        {
            
        }
    }
}
