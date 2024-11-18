using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Models;

namespace Udemy.Repository.Specification
{
    public class GetCourseByIdSpecification:SpecificationBase<Course>
    {
        public GetCourseByIdSpecification(string id) : base(c=>c.Id == id)
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.Course_Chapters);
            AddInclude(x => x.CourseReviews);
        }
    }
}
