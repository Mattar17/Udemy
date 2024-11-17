using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Repository.Specification
{
    public abstract class SpecificationBase<TEntity> where TEntity : class
    {
        protected SpecificationBase(Expression<Func<TEntity , bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<TEntity , bool>> Criteria { get; }

        public List<Expression<Func<TEntity , object>>> IncludeExpressions { get; } = new();

        protected void AddInclude(Expression<Func<TEntity , object>> includeExpression)
            => IncludeExpressions.Add(includeExpression);

    }
}
