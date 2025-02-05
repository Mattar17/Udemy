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
    public class ReviewRepository : GenericRepository<CourseReview>,IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            
        }
    }
}
