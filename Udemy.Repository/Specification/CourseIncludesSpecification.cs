using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;

namespace Udemy.Repository.Specification
{
    public class CourseIncludesSpecification : SpecificationBase<Course>
    {
        public CourseIncludesSpecification() : base(null)
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.Course_Chapters);
            AddInclude(x => x.CourseReviews);
            AddInclude(x => x.Instructor);
        }
        
    }
}
