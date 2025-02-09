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
        private readonly ApplicationDbContext dbContext;

        public StudentCourseRepo(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public  bool IsEnrolled(string UserId , string CourseId)
        {
            var Result = dbContext.Student_Courses.Where(sc => sc.UserId == UserId && sc.CourseId == CourseId);

            if (Result.Count() > 0)
                return true;

            else return false;
        }
    }
}
