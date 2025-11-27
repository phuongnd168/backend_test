using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Repositories;
using TLS.BHL.Infra.Data.SQL.Contexts;
using TLS.Core.Data;
using TLS.Lib.Data.Sql;
using System.Linq.Dynamic.Core;

namespace TLS.BHL.Infra.Data.SQL.Repositories
{
    public class UserRepository : BHLRepositoryBase<UserRepository>, IUserRepository
    {
        private IBHLDbContext Context;
        public UserRepository(IServiceProvider serviceProvider, IBHLDbContext context) : base(serviceProvider)
        {
            Context = context;
        }

        public async Task<IEnumerable<UserEntity>> GetListUser(string? keyword = null)
        {
            return await Context.Users.Where(m => keyword.IsNullOrEmpty() || m.FullName.Contains(keyword)).ToListAsync();

        }

        public async Task<PaginatedList<UserEntity>> SearchUserAsync(Dictionary<string, Dictionary<string, string?>>? searchCriteria, string? sortField, string? sortOrder, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            if (pageSize == int.MaxValue)
                pageSize = int.MaxValue - 1;
            var usersQuery = Context.Users.AsQueryable().Where(u => !u.Deleted);
            if (searchCriteria != null && searchCriteria.Any())
            {
                foreach (var criteria in searchCriteria)
                {
                    string propertyName = criteria.Key;
                    var conditions = criteria.Value;
                    foreach (var condition in conditions)
                    {
                        string operatorType = condition.Key;
                        string value = !string.IsNullOrEmpty(condition.Value) ? condition.Value : "";
                        // Kiểm tra kiểu dữ liệu của thuộc tính
                        var propertyType = typeof(UserEntity).GetProperty(propertyName)?.PropertyType;
                        if (propertyType != null)
                        {
                            if (propertyType == typeof(string) && !string.IsNullOrEmpty(value))
                            {
                                usersQuery = ApplyStringCondition(usersQuery, propertyName, operatorType, value);
                            }
                            else if (propertyType == typeof(int) && !string.IsNullOrEmpty(value))
                            {

                            }
                            else if (propertyType == typeof(bool) && !string.IsNullOrEmpty(value))
                            {

                            }
                            else if (propertyType == typeof(decimal) && !string.IsNullOrEmpty(value))
                            {

                            }
                            else if (propertyType == typeof(DateTime) && !string.IsNullOrEmpty(value))
                            {

                            }
                        }
                        //switch (operatorType.ToLower())
                        //{
                        //    case "eq":
                        //        usersQuery = usersQuery.Where(p => EF.Property<string>(p, propertyName) == value);
                        //        break;
                        //    case "contains":
                        //        usersQuery = usersQuery.Where(p => EF.Property<string>(p, propertyName).Contains(value));
                        //        break;
                        //    case "startswith":
                        //        usersQuery = usersQuery.Where(p => EF.Property<string>(p, propertyName).StartsWith(value));
                        //        break;
                        //    case "endswith":
                        //        usersQuery = usersQuery.Where(p => EF.Property<string>(p, propertyName).EndsWith(value));
                        //        break;
                        //    case "gt":
                        //        if (decimal.TryParse(value, out decimal decimalValue))
                        //        {
                        //            usersQuery = usersQuery.Where(p => EF.Property<decimal>(p, propertyName) > decimalValue);
                        //        }
                        //        break;
                        //    case "lt":
                        //        if (decimal.TryParse(value, out decimalValue))
                        //        {
                        //            usersQuery = usersQuery.Where(p => EF.Property<decimal>(p, propertyName) < decimalValue);
                        //        }
                        //        break;
                        //    // Thêm các trường hợp khác nếu cần
                        //    default:
                        //        break;
                        //}
                        
                    }
                }
                if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
                {
                    var sortFieldType = typeof(UserEntity).GetProperty(sortField)?.PropertyType;
                    if (sortFieldType != null)
                    {
                        if (sortOrder.ToUpper().Equals("DESC"))
                        {
                            usersQuery = usersQuery.SortData( sortField, false);
                        }
                        else
                        {
                            usersQuery = usersQuery.SortData( sortField, true);
                        }
                    }
                }
            }

            //var users = (await usersQuery.ToListAsync());

            return await usersQuery.ToPagingAsync(pageIndex, pageSize);
        }

        public async Task<int> UpdateUser(UpdateUserDTO user, CancellationToken cancellationToken)
        {
            UserEntity? entity;
            if ((user.Id ?? 0) > 0)
            {
                entity = await Context.Users.FindAsync(new object[] { user.Id });

                //entity = await Context.Users.FindAsync(new { user.Id });
                //if (entity == null)
                //{
                //    throw new Exception($"Not found user with Id = {user.Id}");
                //}

                entity.FullName = user.FullName;
                entity.Mobile = user.Mobile;
                entity.Email = user.Email;
                entity.Address = user.Address;
                entity.Avatar = user.Avatar;
                entity.Status = user.Status;
                entity.Type = user.Type;
            }
            else
            {
                entity = new UserEntity
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Mobile = user.Mobile,
                    Email = user.Email,
                    Address = user.Address,
                    Avatar = user.Avatar,
                    Status = user.Status,
                    Type = user.Type,
                    Created_at = DateTime.Now,
                    Created_by = user.Updated_by,
                    Deleted = false,
                };
                Context.Users.Add(entity);
            }

            //var res = Context.SaveChanges();
            var res = await Context.SaveChangesAsync(cancellationToken);

            return res > 0 ? entity.Id : 0;
        }
        //private IQueryable<UserEntity> SortData<UserEntity>(IQueryable<UserEntity> data, string sortColumn, bool isAscending)
        //{
        //    // Lấy thông tin thuộc tính bằng Reflection
        //    PropertyInfo propertyInfo = typeof(UserEntity).GetProperty(sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        //    if (propertyInfo == null)
        //    {
        //        throw new ArgumentException($"Thuộc tính '{sortColumn}' không tồn tại trên kiểu '{typeof(UserEntity).Name}'");
        //    }

        //    // Sắp xếp bằng LINQ và Reflection
        //    if (isAscending)
        //    {
        //        return data.OrderBy(x => propertyInfo.GetValue(x, null));
        //    }
        //    else
        //    {
        //        return data.OrderByDescending(x => propertyInfo.GetValue(x, null));
        //    }
        //}

        public IQueryable<T> SortData<T>(IQueryable<T> data, string sortColumn, bool isAscending)
        {
            // Sử dụng Dynamic LINQ để sắp xếp trên IQueryable
            string sortQuery = isAscending ? $"{sortColumn} ascending" : $"{sortColumn} descending";
            return data.OrderBy(sortQuery);
        }
        private IQueryable<UserEntity> ApplyStringCondition(IQueryable<UserEntity> query, string propertyName, string operatorType, string value)
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

        private IQueryable<UserEntity> ApplyDecimalCondition(IQueryable<UserEntity> query, string propertyName, string operatorType, string value)
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
        private IQueryable<UserEntity> ApplyIntCondition(IQueryable<UserEntity> query, string propertyName, string operatorType, string value)
        {
            if (int.TryParse(value, out int decimalValue))
            {
                switch (operatorType.ToLower())
                {
                    case "eq":
                        query = query.Where(p => EF.Property<int>(p, propertyName) == decimalValue);
                        break;
                    case "gt":
                        query = query.Where(p => EF.Property<int>(p, propertyName) > decimalValue);
                        break;
                    case "lt":
                        query = query.Where(p => EF.Property<int>(p, propertyName) < decimalValue);
                        break;
                }
            }
            return query;
        }
        private IQueryable<UserEntity> ApplyDateTimeCondition(IQueryable<UserEntity> query, string propertyName, string operatorType, string value)
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
