using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Remontinka.Client.DataLayer.EntityFramework
{
    /// <summary>
    ///   Расширения для paging.
    /// </summary>
    public static class LinqExtensions
    {
        ///used by LINQ to SQL
        public static IQueryable<TSource> Page<TSource>(this IOrderedQueryable<TSource> source, int page, int pageSize)
        {
            return pageSize == int.MaxValue ? source : source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        //used by LINQ
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return pageSize == int.MaxValue ? source : source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Represents the SQL between statement.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        public static IQueryable<TSource> Between<TSource, TKey>(this IQueryable<TSource> source,
                                                                 Expression<Func<TSource, TKey>> keySelector, TKey low,
                                                                 TKey high) where TKey : IComparable<TKey>
        {
            Expression key = Expression.Invoke(keySelector,
                                               keySelector.Parameters.ToArray());
            Expression lowerBound = Expression.GreaterThanOrEqual
                (key, Expression.Constant(low));
            Expression upperBound = Expression.LessThanOrEqual
                (key, Expression.Constant(high));
            Expression and = Expression.AndAlso(lowerBound, upperBound);
            Expression<Func<TSource, bool>> lambda =
                Expression.Lambda<Func<TSource, bool>>(and, keySelector.Parameters);
            return source.Where(lambda);
        }
    }
}
