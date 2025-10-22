using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface ISpecifications<TKey,TEntity> where TEntity : BaseEntity<TKey>
    {
         List<Expression<Func<TEntity,object>>> Includes { get; set; }
         Expression<Func<TEntity,bool>>? Criteria { get; set; }

        Expression <Func<TEntity, object>>? OrderBy { get; set; }
        Expression <Func<TEntity, object>>? OrderBydescending { get; set; }
         int skip { get; set; }
        int take { get; set; }
        bool isPagination { get; set; }


    }
}
