using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Repository.Specification
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity>
            (IQueryable<TEntity> inputQuerable,SpecificationBase<TEntity> specification) where TEntity : class
        {
            IQueryable<TEntity> Queryable = inputQuerable;

            if (specification.Criteria is not null)
            {
                Queryable = Queryable.Where(specification.Criteria);
            }

            if (specification.IncludeExpressions is not null)
            {
                Queryable = specification.IncludeExpressions.Aggregate(Queryable , (current , IncludeExpression)
                    => current.Include(IncludeExpression));
            }

            return Queryable;
        }
    }
}
