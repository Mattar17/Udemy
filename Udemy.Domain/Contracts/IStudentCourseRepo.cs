using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Models;

namespace Udemy.Domain.Contracts
{
    public interface IStudentCourseRepo:IGenericRepository<Student_Course>
    {
        bool IsEnrolled(string UserId , string CourseId);
    }
}
