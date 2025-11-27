using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Repositories;
using TLS.BHL.Infra.App.Services;
using TLS.Core.Service;
using TLS.Core.Data;

namespace TLS.BHL.Infra.Business.Services
{
    public class UserService : ServiceBase, IUserService
    {
        //private IUserRepository UserRepository => GetRepository<IUserRepository>();
        private IUserRepository UserRepository;
        public UserService(IServiceProvider serviceProvider, IUserRepository userRepository) : base(serviceProvider)
        {
            UserRepository = userRepository;
        }

        public async Task<IEnumerable<UserEntity>> GetListUser(string? keyword = null)
        {
            return await UserRepository.GetListUser(keyword);
        }

        public async Task<PaginatedList<UserEntity>> SearchUserAsync(Dictionary<string, Dictionary<string, string?>>? searchCriteria, string? sortField, string? sortOrder, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return await UserRepository.SearchUserAsync(searchCriteria, sortField, sortOrder, pageIndex, pageSize);
        }

        public async Task<int> UpdateUser(UpdateUserDTO user, CancellationToken cancellationToken)
        {
            return await UserRepository.UpdateUser(user, cancellationToken);
        }
    }
}
