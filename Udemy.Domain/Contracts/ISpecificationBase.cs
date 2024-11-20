﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Udemy.Domain.Contracts
{
    public interface ISpecificationBase<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity , bool>> Criteria { get; set; }

        public List<Expression<Func<TEntity , object>>> IncludeExpressions {get; set;}

        protected void AddInclude(Expression<Func<TEntity , object>> includeExpression);
          
    }
}