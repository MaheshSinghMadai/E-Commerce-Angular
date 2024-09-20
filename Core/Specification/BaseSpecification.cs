using Core.Interfaces;
using System.Linq.Expressions;

namespace Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        private readonly Expression<Func<T, bool>> criteria;
        public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            this.criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria => criteria;

        public Expression<Func<T, object>> OrderBy  {get; private set;}

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public bool IsDistinct { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void ApplyDistinct()
        {
            IsDistinct = true;
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            this.OrderBy = orderBy;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDesc)
        {
            this.OrderByDescending = orderByDesc;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            if(criteria != null)
            {
                query = query.Where(criteria);
            }

            return query;
        }
    }

    public class BaseSpecification<T, TResult> : BaseSpecification<T>, ISpecification<T, TResult>
    {
        private readonly Expression<Func<T, bool>> criteria;
        public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            this.criteria = criteria;
        }

        public Expression<Func<T, TResult>> Select { get; private set; }
        protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
        {
            Select = selectExpression;
        }
    }
}
