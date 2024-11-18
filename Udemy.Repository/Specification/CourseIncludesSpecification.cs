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
            
        }

        
    }
}
