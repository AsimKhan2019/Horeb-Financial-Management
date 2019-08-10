using System;
using System.Linq.Expressions;

namespace Horeb.Pesistency.Specification
{
    public interface ISpecification<TEntity>
        where TEntity : class
    {
        Expression<Func<TEntity, bool>> SatisfiedBy();
    }
}
