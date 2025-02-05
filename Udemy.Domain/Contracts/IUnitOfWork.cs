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
        ICourseChapterRepository ChapterRepository { get; }
        ILectureRepository LectureRepository { get; }
        IStudentCourseRepo StudentCourseRepo { get; }
        IReviewRepository ReviewRepository { get; }
        Task<int> CompleteAsync(); 
    }
}
