using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Udemy.Domain.Contracts;

namespace Udemy.Repository.Specification
{
    public abstract class SpecificationBase<TEntity>:ISpecificationBase<TEntity> where TEntity : class
    {
        protected SpecificationBase(Expression<Func<TEntity , bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<TEntity , bool>> Criteria { get; set; }

        public List<Expression<Func<TEntity , object>>> IncludeExpressions { get; set; } = new();

        public void AddInclude(Expression<Func<TEntity , object>> includeExpression)
            => IncludeExpressions.Add(includeExpression);

    }
}
