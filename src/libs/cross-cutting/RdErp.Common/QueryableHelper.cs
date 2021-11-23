using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

using LinqToDB;

namespace RdErp
{
    public static class QueryableHelper
    {
        public static IQueryable<T> SortByStringName<T>(
            this IQueryable<T> source,
            string propertyName,
            SortDirection direction,
            params string[] allowedProps)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentException("message", nameof(propertyName));

            var propToSort = typeof(T).GetProperties()
                .FirstOrDefault(p => StringComparer.OrdinalIgnoreCase.Equals(p.Name, propertyName.Trim()));

            if (propToSort == null)
                throw new ArgumentException("Property for sorting is not valid.");

            if (allowedProps != null && allowedProps.Length != 0
                && !allowedProps.Any(p => StringComparer.OrdinalIgnoreCase.Equals(p, propertyName.Trim())))
                throw new ArgumentNullException("Property for sorting is not allowed.");

            IOrderByExpression<T> sortHelper = (IOrderByExpression<T>) Activator.CreateInstance(
                typeof(OrderByExpression<,>).MakeGenericType(
                    typeof(T),
                    propToSort.PropertyType
                ),
                propToSort,
                direction);

            return sortHelper.Sort(source);
        }

        /// <summary>
        /// Applies sorting and paging (but not searching) from <paramref name="request" />
        /// to <paramref name="source" /> queryable.
        ///
        /// If SortBy parameter is not provided then Skip and Take would not be applied
        /// (because it is not possible to slice non-ordered set).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="request"></param>
        /// <param name="allowedSortingProps"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<PageResult<T>> ApplyListRequest<T>(
            this IQueryable<T> source,
            ListRequest request,
            params string[] allowedSortingProps)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (request == null) throw new ArgumentNullException(nameof(request));

            var total = await source.CountAsync();

            if (!String.IsNullOrEmpty(request.SortBy))
            {
                source = source.SortByStringName(
                    request.SortBy,
                    request.SortDirection ?? SortDirection.Asc,
                    allowedSortingProps);

                if (request.Skip != null || request.Take != null)
                {
                    source = source.Skip(request.Skip ?? 0).Take(request.Take ?? 10);
                }
            }

            return new PageResult<T>
                {
                    Data = await source.ToArrayAsync(),
                    Total = total
                };
        }
    }

    interface IOrderByExpression<TItem>
        {
            IQueryable<TItem> Sort(IQueryable<TItem> source);
        }

    class OrderByExpression<TItem, TKey> : IOrderByExpression<TItem>
        {
            private readonly Expression<Func<TItem, TKey>> sortExpression;
            private readonly SortDirection sortDirection;

            public OrderByExpression(PropertyInfo sortBy, SortDirection? sortDirection)
            {
                this.sortDirection = sortDirection ?? SortDirection.Asc;

                var param = Expression.Parameter(typeof(TItem), "i");

                this.sortExpression = (Expression<Func<TItem, TKey>>) Expression.Lambda(
                    typeof(Func<,>).MakeGenericType(typeof(TItem), sortBy.PropertyType),
                    Expression.Property(param, sortBy),  new [] { param }
                );
            }

            public IQueryable<TItem> Sort(IQueryable<TItem> source)
            {
                return sortDirection == SortDirection.Asc
                    ? source.OrderBy(sortExpression)
                    : source.OrderByDescending(sortExpression);
            }
        }
}