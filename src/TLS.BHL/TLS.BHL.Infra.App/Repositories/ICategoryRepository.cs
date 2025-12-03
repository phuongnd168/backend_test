using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLS.BHL.Infra.App.Domain.DTO.User;
using TLS.BHL.Infra.App.Domain.Entities;
using TLS.BHL.Infra.App.Domain.Helper;
using TLS.BHL.Infra.App.Domain.Models;
using TLS.Core.Data;

namespace TLS.BHL.Infra.App.Repositories
{
    public interface ICategoryRepository : IRepository
    {
        Task<ApiResponse> GetListCategory();
    }
}
