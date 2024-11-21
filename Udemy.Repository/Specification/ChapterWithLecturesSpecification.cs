using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Contracts;
using Udemy.Domain.Models;

namespace Udemy.Repository.Specification
{
    public class ChapterWithLecturesSpecification : SpecificationBase<CourseChapter>
    {
        public ChapterWithLecturesSpecification(int id):base(x=>x.Id == id)
        {
            AddInclude(l => l.Chapter_Lectures); 
        }
    }
}
