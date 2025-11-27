using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.Core.Data;
using System.Linq.Dynamic.Core;
namespace TLS.Lib.Data.Sql
{
    public static class PagingExtensions
    {
        public static PaginatedList<T> ToPaging<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class
        {
            return Create(source.AsNoTracking(), pageNumber, pageSize);
        }
        public static async Task<PaginatedList<T>> ToPagingAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class
        {
            return await CreateAsync(source.AsNoTracking(), pageNumber, pageSize);
        }

        private static PaginatedList<T> Create<T>(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
        private static async Task<PaginatedList<T>> CreateAsync<T>(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
        /// <summary>
        /// System.Linq.Dynamic.Core;
        ///  (Dynamic LINQ): Tốt nhất khi muốn giữ nguyên truy vấn IQueryable để sắp xếp trực tiếp trên SQL Server, đảm bảo hiệu năng với dữ liệu lớn.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="sortColumn"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        public static IQueryable<T> SortData<T>(this IQueryable<T> data, string sortColumn, bool isAscending)
        {
            // Sử dụng Dynamic LINQ để sắp xếp trên IQueryable
            string sortQuery = isAscending ? $"{sortColumn} ascending" : $"{sortColumn} descending";
            return data.OrderBy(sortQuery);
        }

        public static IQueryable<T> ApplyStringCondition<T>(this IQueryable<T> query, string propertyName, string operatorType, string value)
        {
            switch (operatorType.ToLower())
            {
                case "eq":
                    query = query.Where(p => EF.Property<string>(p, propertyName) == value);
                    break;
                case "contains":
                    query = query.Where(p => EF.Property<string>(p, propertyName).Contains(value));
                    break;
                case "startswith":
                    query = query.Where(p => EF.Property<string>(p, propertyName).StartsWith(value));
                    break;
                case "endswith":
                    query = query.Where(p => EF.Property<string>(p, propertyName).EndsWith(value));
                    break;
            }
            return query;
        }

        public static IQueryable<T> ApplyDecimalCondition<T>(this IQueryable<T> query, string propertyName, string operatorType, string value)
        {
            if (decimal.TryParse(value, out decimal decimalValue))
            {
                switch (operatorType.ToLower())
                {
                    case "eq":
                        query = query.Where(p => EF.Property<decimal>(p, propertyName) == decimalValue);
                        break;
                    case "gt":
                        query = query.Where(p => EF.Property<decimal>(p, propertyName) > decimalValue);
                        break;
                    case "lt":
                        query = query.Where(p => EF.Property<decimal>(p, propertyName) < decimalValue);
                        break;
                }
            }
            return query;
        }
        public static IQueryable<T> ApplyIntCondition<T>(this IQueryable<T> query, string propertyName, string operatorType, string value)
        {
            if (int.TryParse(value, out int intValue))
            {
                switch (operatorType.ToLower())
                {
                    case "eq":
                        query = query.Where(p => EF.Property<int>(p, propertyName) == intValue);
                        break;
                    case "gt":
                        query = query.Where(p => EF.Property<int>(p, propertyName) > intValue);
                        break;
                    case "lt":
                        query = query.Where(p => EF.Property<int>(p, propertyName) < intValue);
                        break;
                }
            }
            return query;
        }
        public static IQueryable<T> ApplyLongCondition<T>(this IQueryable<T> query, string propertyName, string operatorType, string value)
        {
            if (long.TryParse(value, out long longValue))
            {
                switch (operatorType.ToLower())
                {
                    case "eq":
                        query = query.Where(p => EF.Property<int>(p, propertyName) == longValue);
                        break;
                    case "gt":
                        query = query.Where(p => EF.Property<int>(p, propertyName) > longValue);
                        break;
                    case "lt":
                        query = query.Where(p => EF.Property<int>(p, propertyName) < longValue);
                        break;
                }
            }
            return query;
        }
        public static IQueryable<T> ApplyDateTimeCondition<T>( this IQueryable<T> query, string propertyName, string operatorType, string value)
        {
            if (DateTime.TryParse(value, out DateTime dateTimeValue))
            {
                switch (operatorType.ToLower())
                {
                    case "eq":
                        query = query.Where(p => EF.Property<DateTime>(p, propertyName) == dateTimeValue);
                        break;
                    case "gt":
                        query = query.Where(p => EF.Property<DateTime>(p, propertyName) > dateTimeValue);
                        break;
                    case "lt":
                        query = query.Where(p => EF.Property<DateTime>(p, propertyName) < dateTimeValue);
                        break;
                    case "gte":
                        query = query.Where(p => EF.Property<DateTime>(p, propertyName) >= dateTimeValue);
                        break;
                    case "lte":
                        query = query.Where(p => EF.Property<DateTime>(p, propertyName) <= dateTimeValue);
                        break;
                }
            }
            return query;
        }

    }
}
