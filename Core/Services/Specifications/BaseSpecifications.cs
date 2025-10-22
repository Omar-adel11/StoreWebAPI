using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services.Specifications
{
    public class BaseSpecifications<Tkey, TEntity> : ISpecifications<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        public List<Expression<Func<TEntity, object>>> Includes { get;  set ; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, bool>>? Criteria { get ; set ; }
       
        public Expression<Func<TEntity, object>>? OrderBy { get ; set ; }
        public Expression<Func<TEntity, object>>? OrderBydescending { get; set ; }
        public int skip { get ; set ; }
        public int take { get ; set ; }
        public bool isPagination { get; set; }

        public BaseSpecifications(Expression<Func<TEntity, bool>>? expression)
        {
            Criteria = expression;
        }

        public void ApplyPagination(int PageSize, int PageIndex)
        {
            isPagination = true;
            skip = (PageIndex - 1) * PageSize;
            take = PageSize;
        }
    }
}
