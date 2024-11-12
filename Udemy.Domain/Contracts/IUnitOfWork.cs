using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Domain.Contracts
{
    public interface IUnitOfWork:IDisposable
    {
        ICourseRepository CourseRepository { get; }
        Task<int> CompleteAsync(); 
    }
}
