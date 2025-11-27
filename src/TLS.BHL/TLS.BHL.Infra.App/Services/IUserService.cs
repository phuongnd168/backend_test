using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.Core.Service;
using TLS.Core.Data;

namespace TLS.BHL.Infra.App.Services
{
    public interface IUserService : IService
    {
        Task<IEnumerable<UserEntity>> GetListUser(string? keyword = null);
        Task<PaginatedList<UserEntity>> SearchUserAsync(Dictionary<string, Dictionary<string, string?>>? searchCriteria, string? sortField, string? sortOrder, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<int> UpdateUser(UpdateUserDTO user, CancellationToken cancellationToken);
    }
}
